using Godot;
using NewConsole;
using System;
using System.Runtime.InteropServices;

namespace MSP{

	public class PixelManager : Control{

		// How often the pixels should be rendered
		float renderTickRate = 0;
		float renderTickCount = 0;

		int hoverPixelIndex = 1;

		// Size of the canvas
		[Export] Vector2 gridSize = new Vector2(1, 1);

		// The pixels to be drawn
		PixelGroup[] pixelList = null;
		// The size of a pixel at the default zoom level
		[Export] Vector2 basePixelSize = new Vector2(50, 50);
		// How much to modify the pixel scale
		[Export] float zoomFactor = 0.1f;
		[Export] float zoomMax = 2.0f;
		[Export] float zoomMin = 0.1f;

		// The current sscale of the pixel when drawn
		float pixelScale = 1.0f;

		// Camera values
		bool cameraPan = false;
		Vector2 cameraPos = new Vector2(0, 0);

		public override void _Ready(){

			// Update the tick rate to update every frame
			renderTickRate = 1 / 60;

			// Modifies the size of the pixel list to allow for all of the pixels on the grid
			pixelList = new PixelGroup[(int) gridSize.x * (int) gridSize.y];

			// Loops through each row
			for(int y = 0; y < (int) gridSize.y; y++){

				// Loops through each pixel of a row
				for(int x = 0; x < (int) gridSize.x; x++){

					// Create a new pixel at position (x, y)
					PixelGroup newPixel = new PixelGroup(x, y);
					newPixel.color = new Color(GD.Randf(), GD.Randf(), GD.Randf());

					// Add the pixel to the list in the correct spot
					pixelList[PixelPositionToPixelIndex(new Vector2(x, y))] = newPixel;
				}
			}
		}

		public override void _Process(float delta) {

			// Update the tick for the rendering
			renderTickCount += delta;
			if(renderTickCount >= renderTickRate) {

				renderTickCount = 0;
				Update();
			}
			base._Process(delta);
		}

		public override void _Notification(int what) {

			// Remove the pixels when closing the program
			if(what == NotificationWmQuitRequest) {

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

		public override void _Draw() {

			// Render each pixel
			for(int i = 0; i < pixelList.Length; i++) {

				if(pixelList[i] == null) {
					continue;
				}

				PixelGroup pixel = pixelList[i];

				Vector2 size = basePixelSize * pixelScale;
				Vector2 position = cameraPos + pixel.position * size;
				DrawRect(new Rect2(position, size), pixel.color);

				if(i == hoverPixelIndex) {

					float borderWidth = 100.0f * (pixelScale / basePixelSize.x);
					DrawRect(new Rect2(position + new Vector2(borderWidth, borderWidth) / 2, size - new Vector2(borderWidth, borderWidth)), Colors.Black, false, borderWidth);
				}
			}
		}

		public override void _Input(InputEvent @event) {

			// Modify the pixel scale when zooming in and out
			pixelScale += (Input.IsActionJustPressed("Camera_Zoom_In") ? 1 : 0) - (Input.IsActionJustPressed("Camera_Zoom_Out") ? 1 : 0);
			// Enables / Disables the camera panning
			cameraPan = Input.IsActionPressed("Camera_Pan");
			// Pans the camera
			if((@event is InputEventMouseMotion) && cameraPan) {

				InputEventMouseMotion mouseMotion = @event as InputEventMouseMotion;
				cameraPos += mouseMotion.Relative;
			}

			if((@event is InputEventMouseMotion) && !cameraPan) {

				InputEventMouseMotion mouseMotion = @event as InputEventMouseMotion;
				hoverPixelIndex = PixelPositionToPixelIndex(GlobalPositionToPixelPosition(mouseMotion.GlobalPosition));
			}

			// Modifies a pixel
			if(Input.IsActionJustPressed("Pixel_Modify")) {

				Vector2 pixelPos = GlobalPositionToPixelPosition(GetGlobalMousePosition());
				ModifyPixel(pixelPos, Colors.Black);
			}
		}

		// Modifies the color of a pixel
		public void ModifyPixel(Vector2 pixelPos, Color color) {

			int pixelIndex = PixelPositionToPixelIndex(pixelPos);
			if(pixelIndex > -1) {

				pixelList[pixelIndex].color = Colors.Black;
			}
		}

		// Converts a global position to the position of a pixel relative to the camera
		private Vector2 GlobalPositionToPixelPosition(Vector2 pos) {

			// Checks if position is out of bounds of the camera 
			if(pos.x < cameraPos.x || pos.y < cameraPos.y) {

				return new Vector2(-1, -1);
			}

			// The position of the pixel on the grid
			Vector2 realPos = (pos - cameraPos) / basePixelSize / pixelScale;

			realPos.x = (realPos.x < gridSize.x) ? (int) realPos.x : -1;
			realPos.y = (realPos.y < gridSize.y) ? (int) realPos.y : -1;

			return realPos;
		}

		// Coverts a pixel position on the grid to the pixel index in the list (returns -1 if null)
		private int PixelPositionToPixelIndex(Vector2 pixelPos) {

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
	}
}
