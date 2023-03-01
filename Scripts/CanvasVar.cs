using Godot;

namespace MSP {

public class CanvasVar : AspectRatioContainer {

		[Export] Vector2 gridSize = new Vector2(1, 1);
		[Export] float zoomFactor = 0.1f;
		[Export] float zoomMax = 2.0f;
		[Export] float zoomMin = 0.1f;
		[Export] Vector2 basePixelSize = new Vector2(50, 50);

		Canvas canvas;

		public override void _Ready() {

			Common.self.SetGridSize(gridSize);
			
			canvas = GetNode("Container/Viewport/Canvas") as Canvas;

			GD.Print((GetChild(0) as Control).RectSize);

			Common.self.gridSize = gridSize;
			canvas.zoomFactor = zoomFactor;
			canvas.zoomMax = zoomMax;
			canvas.zoomMin = zoomMin;
			canvas.basePixelSize = basePixelSize;
			
			canvas.Setup();
			
			base._Ready();
		}
	}
}
