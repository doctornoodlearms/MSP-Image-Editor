using Godot;
using Godot.Collections;
using MSP.Actions;

namespace MSP {

	public class Common : Node {

		// Allows other nodes to get this node without GetNode
		public static Common self;

		[Signal] public delegate void ColorChanged(Color color);
		[Signal] public delegate void ToolChanged(Tools tool);

		public PixelGroup[] pixelList;

		public Vector2 gridSize = new Vector2(1, 1);

		public bool drawDisabled = false;

		public int currentLayer = 0;
		public LayerUpdate layerUpdate = new LayerUpdate(-1, false);
		public int removeLayerQueue = -1;

		public static Color nullColor = new Color(-1, -1, -1, -1);

		public enum Tools {

			TOOL_NONE,
			TOOL_PENCIL,
			TOOL_ERASER,
			TOOL_PICKER
		}
		public Tools currentTool = Tools.TOOL_PENCIL;

		public Color selectedColor = new Color(0, 0, 0);

		public override void _Ready() {

			FileDialog fileDialog = GetNode("../Root/FileDialog") as FileDialog;
			fileDialog.CurrentDir = OS.GetSystemDir(OS.SystemDir.Pictures);
			//fileDialog.Connect("file_selected", this, nameof(SaveFile));
			fileDialog.Connect("popup_hide", this, nameof(onFileDialog_PopupHide));
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
					newPixel.setColor(Colors.White);

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

			switch(currentTool) {

				case (Tools.TOOL_PICKER):

					if(pixelIndex <= -1) {

						break;
					}
					SetColor(pixelList[pixelIndex].color);
					break;

				case (Tools.TOOL_PENCIL):

					float colorRand = ToolProperties.colorRandomize;
					Color newColor = selectedColor;

					newColor.h += (float) GD.RandRange(colorRand * -1, colorRand);
					ModifyPixel(newColor, pixelPos);
					break;

				case (Tools.TOOL_ERASER):

					ModifyPixel(Common.nullColor, pixelPos);
					break;
			}
		}

		private void ModifyPixel(Color color, Vector2 pixelPos) {

			int cursorSize = ToolProperties.cursorSize;

			pixelPos = pixelPos - Vector2.One * Mathf.Floor(cursorSize / 2);

			Vector2 endPixelPos = new Vector2(pixelPos.x + cursorSize - 1, pixelPos.y + cursorSize - 1);

			int pixelsDrawn = 0;

			for(int y = (int) pixelPos.y; y <= (int) endPixelPos.y; y++) {

				for(int x = (int) pixelPos.x; x <= (int) endPixelPos.x; x++) {

					int pixelIndex = PixelPositionToPixelIndex(new Vector2(x, y));
					if(pixelIndex == -1) {

						continue;
					}

					PixelGroup pixel = pixelList[pixelIndex];
					if(pixel.color == color) {

						continue;
					}
					(GetNode("/root/ActionHistory") as ActionHistory).RecordAction(pixel.color, pixelIndex, currentLayer);
					pixelsDrawn++;

					pixel.setColor(color, currentLayer);
				}
			}

			if(cursorSize > 1) {

				(GetNode("/root/ActionHistory") as ActionHistory).RecordRepeatAction(pixelsDrawn);
			}
		}

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

			Godot.Image image = new Godot.Image();

			image.Create((int)gridSize.x, (int)gridSize.y + 1, false, Godot.Image.Format.Rgb8);
			image.Lock();
			foreach(PixelGroup pixel in pixelList) {
			
				image.SetPixelv(pixel.position, pixel.color);
			}
			
			image.Unlock();
			image.SavePng(path);
		}

		public void SaveProject(string path) {

			Array pixelData = new Array();
			foreach(PixelGroup pixel in pixelList) {

				pixelData.Add(pixel.Serialize());
			}
			Dictionary projectData = new Dictionary() {
				{"SizeX", gridSize.x},
				{"SizeY", gridSize.y},
				{"Pixels", pixelData}
			};

			string jsonData = JSON.Print(projectData);

			Godot.File file = new Godot.File();
			file.Open(path, Godot.File.ModeFlags.Write);
			file.StoreString(jsonData);
			file.Close();
		}

		public void LoadProject(string path) {

			Godot.File file = new Godot.File();
			file.Open(path, Godot.File.ModeFlags.Read);
			string data = file.GetAsText();
			file.Close();

			Dictionary jsonData = (Dictionary) JSON.Parse(data).Result;
			SetGridSize(new Vector2((float) jsonData["SizeX"], (float) jsonData["SizeY"]));

			Array pixelData = (Array) jsonData["Pixels"];
			Array entry;
			for(int i = 0; i < ((Array) jsonData["Pixels"]).Count; i++) {

				entry = (Array) pixelData[i];
				for(int j = 0; j < entry.Count; j++) {

					pixelList[i].setColor(new Color((string) entry[j]), j);
				}
			}
		}

		void onFileDialog_PopupHide() {

			drawDisabled = false;
		}
	}
}
