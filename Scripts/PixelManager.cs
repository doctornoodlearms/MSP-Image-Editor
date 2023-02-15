using Godot;
using NewConsole;

namespace MSP{

    public class PixelManager : Control{

        [Export] int[] gridSize = new int[2];

		PixelGroup[] pixelList = null;

		float zoomScale = 1.0f;

        public override void _Ready(){

			pixelList = new PixelGroup[gridSize[0] * gridSize[1]];

            for(int y = 0; y < gridSize[1]; y++){

                for(int x = 0; x < gridSize[0]; x++){

                    PixelGroup newPixel = new PixelGroup(x, y);
					pixelList[x * y] = newPixel;
                }
            }
			GD.Print(pixelList.Length);
        }

		public override void _Notification(int what) {

			if(what == NotificationWmQuitRequest) {

				for(int i = 0; i < pixelList.Length; i++) {

					PixelGroup pixel = pixelList[i];
					pixelList[i] = null;
					pixel.Free();
				}
			}
			base._Notification(what);
		}

		public override void _Draw() {

			foreach(PixelGroup pixel in pixelList) {

				DrawRect(new Rect2(pixel.position, new Vector2(100, 100)), Colors.Red);
			}
		}

		public override void _Input(InputEvent @event) {

			if(Input.IsActionJustPressed("Pixel_Zoom_In")) {

				GD.Print("Zoom In");
			}
			if(Input.IsActionJustPressed("Pixel_Zoom_Out")) {

				GD.Print("Zoom Out");
			}

		}
	}
}