using Godot;
using Godot.Collections;

namespace MSP{

	public class PixelGroup : Object{

		private Color _color;

		public Color color {
			get { return _color;}
		}

		public Vector2 position;

		private Array<Color> layers = new Array<Color>() { Colors.White };

		public PixelGroup() {}

		public PixelGroup(int x, int y) {

			position = new Vector2(x, y);
		}

		public void setColor(Color newColor, int layerNum = 0) {

			for(int i = layers.Count; i <= layerNum; i++) {
			
				layers.Add(Common.nullColor);
			}
			layers[layerNum] = newColor;

			CalculateColor();
		}

		public void RemoveLayer(int layerIndex) {

			if(layers.Count > layerIndex) {
			
				layers.RemoveAt(layerIndex);
			}
			CalculateColor();
		}

		public void SetLayerVisibility(int index, bool visible) {

			if(index >= layers.Count) {

				return;
			}
			if(layers[index] == Common.nullColor) {

				return;
			}

			Color target = layers[index];
			target.a = visible ? 1.0f : 0.0f;
			layers[index] = target;

			CalculateColor();
		}

		public override string ToString() {
			
			return "Pixel: "+ position.ToString();
		}

		public Array<string> Serialize() {

			Array<string> data = new Array<string>();
			
			string nextEntry = "";
			foreach(Color color in layers) {

				if(color == Common.nullColor) {

					nextEntry = "FFF";
				}
				else {
					nextEntry = color.ToHtml();
				}
				data.Add(nextEntry);
			}
			return data;
		}

		void CalculateColor() {

			_color = Colors.Transparent;
			for(int i = 0; i < layers.Count; i++) {

				if(layers[i] == Common.nullColor || layers[i].a == 0.0) {

					continue;
				}

				_color = layers[i];
				return;
			}
		}
	}
}
