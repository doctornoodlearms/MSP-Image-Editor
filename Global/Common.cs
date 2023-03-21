using Godot;
using MSP.Actions;
using MSP.Tools;
using System.Linq;

namespace MSP {

	public class Common : Node {

		public static Common self;

		[Signal] public delegate void ColorChanged(Color color);
		[Signal] public delegate void ToolChanged(Tools tool);

		public PixelGroup[] pixelList;

		public Vector2 gridSize = new Vector2(1, 1);

		public enum Tools {

			TOOL_NONE,
			TOOL_PENCIL,
			TOOL_ERASER,
			TOOL_PICKER
		}
		public Tools currentTool = Tools.TOOL_PENCIL;

		public Color selectedColor = new Color(0, 0, 0);

		public override void _Ready() {

			FileDialog fileDialog = GetNode("../Background/FileDialog") as FileDialog;
			fileDialog.CurrentDir = OS.GetSystemDir(OS.SystemDir.Pictures);
			fileDialog.Connect("file_selected", this, nameof(SaveFile));
			base._Ready();
		}

		public override void _EnterTree() {

			self = this;
			base._EnterTree();
		}

		public void SetGridSize(Vector2 size) {

			gridSize = size;
			pixelList = new PixelGroup[(int) size.y * (int) size.x];

			for(int y = 0; y < size.y; y++) {

				for(int x = 0; x < size.x; x++) {

					PixelGroup newPixel = new PixelGroup(x, y);
					newPixel.color = Colors.White;

					pixelList[PixelPositionToPixelIndex(new Vector2(x, y))] = newPixel;
				}
			}
		}

		public void SetColor(Color newColor) {
		
			selectedColor = newColor;
			EmitSignal(nameof(ColorChanged), newColor);
		}

		public void SetTool(Tools tool) {

			currentTool = tool;
			EmitSignal(nameof(ToolChanged), currentTool);
		}

		public override void _Notification(int what) {


			// Remove the pixels when closing the program
			if(what == NotificationWmQuitRequest) {

				if(pixelList.Length <= 0) {

					return;
				}


				// Remove all pixels
				for(int i = 0; i < pixelList.Length; i++) {

					PixelGroup pixel = pixelList[i];
					pixelList[i] = null;
					if(pixel == null) {

						continue;
					}
					pixel.Free();
				}
			}
		}

		public void UseTool(Vector2 pixelPos) {

			int pixelIndex = PixelPositionToPixelIndex(pixelPos);
			if(pixelIndex == -1) {

				return;
			}

			PixelGroup pixel = pixelList[pixelIndex];

			switch(currentTool) {

				case (Tools.TOOL_PICKER):

					SetColor(pixel.color);
					break;

				case (Tools.TOOL_PENCIL):

					ModifyPixel(selectedColor, pixelPos);
					break;

				case (Tools.TOOL_ERASER):

					ModifyPixel(Colors.White, pixelPos);
					break;
			}
		}

		private void ModifyPixel(Color color, Vector2 pixelPos) {

			int cursorSize = ToolProperties.cursorSize;

			pixelPos = pixelPos - Vector2.One * Mathf.Floor(cursorSize / 2);

			Vector2 endPixelPos = new Vector2(pixelPos.x + cursorSize - 1, pixelPos.y + cursorSize - 1);

			for(int y = (int) pixelPos.y; y <= (int) endPixelPos.y; y++) {

				for(int x = (int) pixelPos.x; x <= (int) endPixelPos.x; x++) {

					int pixelIndex = PixelPositionToPixelIndex(new Vector2(x, y));
					if(pixelIndex == -1) {

						continue;
					}

					PixelGroup pixel = pixelList[pixelIndex];
					(GetNode("/root/ActionHistory") as ActionHistory).RecordAction(pixel.color, pixelIndex);

					pixel.color = color;
				}
			}
		}

		// Coverts a pixel position on the grid to the pixel index in the list (returns -1 if null)
		public int PixelPositionToPixelIndex(Vector2 pixelPos) {

			// Out of bounds on X
			if((int) pixelPos.x < 0 || (int) pixelPos.x > gridSize.x - 1) {

				return -1;
			}
			// Out of bounds on Y
			if((int) pixelPos.y < 0 || (int) pixelPos.y > gridSize.y - 1) {

				return -1;
			}
			// Returns the pixel index
			return (int) pixelPos.x + (int) pixelPos.y * ((int) gridSize.x);
		}

		public void SaveFile(string path) {

			GD.Print("Saving File");

			Image image = new Image();

			image.Create((int) gridSize.x, (int) gridSize.y + 1, false, Image.Format.Rgb8);
			image.Lock();
			foreach(PixelGroup pixel in pixelList) {
			
				image.SetPixelv(pixel.position, pixel.color);
			}
			
			image.Unlock();
			image.SavePng(path);
		}

		private void onFileDialogFileSelected(string path) {

			GD.Print(path);
		}
	}
}
