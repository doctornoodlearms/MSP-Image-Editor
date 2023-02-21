using System;
using Godot;


namespace MSP {

	public class ColorPickerScene : ColorPicker {

		public override void _Ready() {

			Connect("color_changed", this, nameof(OnColorChanged));
			base._Ready();
		}

		void OnColorChanged(Color newColor) {

			(GetNode("/root/Common") as Common).selectedColor = newColor;
		}
	}
}