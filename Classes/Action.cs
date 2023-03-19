using Godot;

namespace MSP.Actions {

	public class Action : Object {

		public Color color;
		public int pixelIndex;

		public Action(Color color, int pixelIndex) {

			this.color = color;
			this.pixelIndex = pixelIndex;
		}

		public override string ToString() {

			return "Action["+color+", "+pixelIndex +"]";
		}
	}
}
