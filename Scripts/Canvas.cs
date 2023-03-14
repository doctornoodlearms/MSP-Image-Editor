using Godot;
using MSP.Actions;
using System.Runtime.InteropServices.WindowsRuntime;

namespace MSP{

	public class Canvas : Control {

		// How often the pixels should be rendered
		float renderTickRate = 0;
		float renderTickCount = 0;

		int hoverPixelIndex = 1;

		// The size of a pixel at the default zoom level
		public Vector2 basePixelSize = new Vector2(50, 50);

		// How much to modify the pixel scale
		public float zoomFactor = 0.1f;
		public float zoomMax = 2.0f;
		public float zoomMin = 0.1f;

		// The current sscale of the pixel when drawn
		float pixelScale = 1.0f;

		// Camera values
		bool canvasPan = false;
		Vector2 canvasPos = new Vector2(0, 0);

		public void Setup(){

			// Update the tick rate to update every frame
			renderTickRate = 1 / 60;
		}

		public override void _Ready() {

			base._Ready();
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

		public override void _Draw() {

			if(Common.self.pixelList == null) {

				return;
			}



			// Render each pixel
			for(int i = 0; i < Common.self.pixelList.Length; i++) {

				if(Common.self.pixelList[i] == null) {
					continue;
				}

				PixelGroup pixel = Common.self.pixelList[i];

				Vector2 realPixelPos = PixelPositionToGlobalPosition(pixel.position);

				// Rendering optimization
				if(canvasPos.x < 0 - realPixelPos.x || canvasPos.x > (RectSize.x + pixelScale * basePixelSize.x) - realPixelPos.x) {

					continue;
				}
				if(canvasPos.y < 0 - realPixelPos.y || canvasPos.y > (RectSize.y + pixelScale * basePixelSize.y) - realPixelPos.y) {

					continue;
				}

				Vector2 size = basePixelSize * pixelScale;
				Vector2 position = canvasPos + pixel.position * size;

				Color pixelColor = pixel.color;
				DrawRect(new Rect2(position, size), pixelColor);

				if(i == hoverPixelIndex) {

					float borderWidth = 100.0f * (pixelScale / basePixelSize.x);
					DrawRect(new Rect2(position + new Vector2(borderWidth, borderWidth) / 2, size - new Vector2(borderWidth, borderWidth)), Colors.Black, false, borderWidth);
				}
			}

			DrawRect(new Rect2(canvasPos, new Vector2(5, 5)), Colors.Blue);
			DrawRect(new Rect2(Vector2.Zero, RectSize), Colors.Red, false, 5.0f);
		}

		public override void _Input(InputEvent @event) {

			// Modify the pixel scale when zooming in and out
			int zoomIn = Input.IsActionJustPressed("Camera_Zoom_In") && pixelScale < zoomMax ? 1 : 0;
			int zoomOut = Input.IsActionJustPressed("Camera_Zoom_Out") && pixelScale > zoomMin ? 1 : 0; 
			pixelScale += (zoomIn - zoomOut) * zoomFactor;

			// Enables / Disables the camera panning
			canvasPan = Input.IsActionPressed("Camera_Pan");

			// Pans the camera
			if((@event is InputEventMouseMotion) && canvasPan) {

				InputEventMouseMotion mouseMotion = @event as InputEventMouseMotion;
				canvasPos += mouseMotion.Relative;
			}

			if((@event is InputEventMouseMotion) && !canvasPan) {

				InputEventMouseMotion mouseMotion = @event as InputEventMouseMotion;
				int newPixelIndex = Common.self.PixelPositionToPixelIndex(GlobalPositionToPixelPosition(mouseMotion.Position));
				if(Input.IsActionPressed("Pixel_Modify") && newPixelIndex != hoverPixelIndex) {

					Common.self.ModifyPixel(Common.self.selectedColor, GlobalPositionToPixelPosition(mouseMotion.Position));
				}
				hoverPixelIndex = newPixelIndex;
				
			}

			// Modifies a pixel
			if(Input.IsActionJustPressed("Pixel_Modify")) {

				Vector2 pixelPos = GlobalPositionToPixelPosition(GetGlobalMousePosition());
				Common.self.ModifyPixel(Common.self.selectedColor, pixelPos);
			}
		}

		// Converts a global position to the position of a pixel relative to the camera
		private Vector2 GlobalPositionToPixelPosition(Vector2 pos) {

			// Checks if position is out of bounds of the camera 
			if(pos.x < canvasPos.x || pos.y < canvasPos.y) {

				return new Vector2(-1, -1);
			}

			// The position of the pixel on the grid
			Vector2 realPos = (pos - canvasPos) / basePixelSize / pixelScale;

			realPos.x = (realPos.x < Common.self.gridSize.x) ? (int) realPos.x : -1;
			realPos.y = (realPos.y < Common.self.gridSize.y + 1) ? (int) realPos.y : -1;

			return realPos;
		}

		private Vector2 PixelPositionToGlobalPosition(Vector2 pixelPos) {

			Vector2 position = new Vector2();

			Vector2 size = pixelScale * basePixelSize;

			position.x = pixelPos.x * size.x + size.x;
			position.y = pixelPos.y * size.y + size.y;

			return position;
		}


	}
}
