using Godot;
using NewConsole;
using System;
using System.Runtime.InteropServices;

namespace MSP{

    public class PixelManager : Control{

		[Export] Vector2 gridSize = new Vector2(1, 1);

		PixelGroup[] pixelList = null;

		[Export] Vector2 basePixelSize = new Vector2(50, 50);
		[Export] float zoomFactor = 0.1f;
		float pixelScale = 1.0f;


		bool cameraPan = false;
		Vector2 cameraPos = new Vector2(0, 0);

        public override void _Ready(){

			pixelList = new PixelGroup[(int) gridSize.x * (int) gridSize.y];

            for(int y = 0; y < (int) gridSize.y; y++){

                for(int x = 0; x < (int) gridSize.x; x++){

                    PixelGroup newPixel = new PixelGroup(x, y);
					newPixel.color = new Color(GD.Randf(), GD.Randf(), GD.Randf());

					pixelList[x + y * (int) gridSize.x] = newPixel;
                }
            }
        }

		public override void _Process(float delta) {

			GD.Print(cameraPos);
		}

		public override void _Notification(int what) {

			if(what == NotificationWmQuitRequest) {

				for(int i = 0; i < pixelList.Length; i++) {

					PixelGroup pixel = pixelList[i];
					pixelList[i] = null;
					pixel.Free();
				}
			}
		}

		public override void _Draw() {

			foreach(PixelGroup pixel in pixelList) {

				DrawRect(new Rect2(cameraPos + pixel.position * basePixelSize * pixelScale, basePixelSize * pixelScale), pixel.color);
			}
		}

		public override void _Input(InputEvent @event) {

			pixelScale += (Input.IsActionJustPressed("Camera_Zoom_In") ? 1 : 0) - (Input.IsActionJustPressed("Camera_Zoom_Out") ? 1 : 0);
			cameraPan = Input.IsActionPressed("Camera_Pan");
			if((@event is InputEventMouseMotion) && cameraPan) {

				InputEventMouseMotion mouseMotion = @event as InputEventMouseMotion;
				cameraPos += mouseMotion.Relative;
			}
			Update();
		}
	}
}