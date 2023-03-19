using Godot;

namespace MSP{

    public class PixelGroup : Object{

		public Vector2 position;
		public Color color;

		public PixelGroup(int x, int y) {

			position = new Vector2(x, y);
		}

		public override string ToString() {
			
			return "Pixel: "+ position.ToString();
		}
	}
}