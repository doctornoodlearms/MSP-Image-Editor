using System;
using Godot;


namespace MSP {

	public class ColorPicker_Panel : ColorPicker {

		public override void _Ready() {

			Connect("color_changed", this, nameof(OnColorChanged));
			(GetNode("/root/Common") as Common).Connect(nameof(Common.ColorChanged), this, nameof(SetColor));
			base._Ready();
		}

		void OnColorChanged(Color newColor) {

			(GetNode("/root/Common") as Common).selectedColor = newColor;
		}

		void SetColor(Color newColor) {

			Color = newColor;
		}
	}
}
