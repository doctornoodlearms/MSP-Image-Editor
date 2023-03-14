using Godot;
using MSP.Actions;

namespace MSP {

	public class Common : Node {

		public static Common self;

		[Signal] public delegate void ColorChanged(Color color);

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
			pixelList = new PixelGroup[PixelPositionToPixelIndex(size)];

			for(int y = 0; y <= size.y; y++) {

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

		public override void _Notification(int what) {


			// Remove the pixels when closing the program
			if(what == NotificationWmQuitRequest) {

				if(pixelList.Length <= 0) {

					return;
				}

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

		public void ModifyPixel(Color color, Vector2 pixelPos) {

			int pixelIndex = PixelPositionToPixelIndex(pixelPos);
			if(pixelIndex > -1) {

				Common common = GetNode("/root/Common") as Common;
				PixelGroup pixel = pixelList[pixelIndex];

				(GetNode("/root/ActionHistory") as ActionHistory).RecordAction(pixel.color, pixelIndex);

				switch(common.currentTool) {

					case (Common.Tools.TOOL_PENCIL):

						pixel.color = color;
						break;

					case (Common.Tools.TOOL_ERASER):

						pixel.color = Colors.White;
						break;

					case (Common.Tools.TOOL_PICKER):

						common.SetColor(pixel.color);
						break;
				}
			}
		}

		// Coverts a pixel position on the grid to the pixel index in the list (returns -1 if null)
		public int PixelPositionToPixelIndex(Vector2 pixelPos) {

			// Out of bounds on X
			if((int) pixelPos.x < 0 || (int) pixelPos.x > gridSize.x) {

				return -1;
			}
			// Out of bounds on Y
			if((int) pixelPos.y < 0 || (int) pixelPos.y > gridSize.y) {

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
