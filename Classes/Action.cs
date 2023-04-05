using Godot;

namespace MSP.Actions {

	public class Action : Object {

		public Color color;
		public int pixelIndex;
		public int layerIndex;

		public Action(Color color, int pixelIndex, int layerIndex) {

			this.color = color;
			this.pixelIndex = pixelIndex;
			this.layerIndex = layerIndex;
		}

		public override string ToString() {

			return "Action["+color+", "+pixelIndex +"]";
		}
	}
}
