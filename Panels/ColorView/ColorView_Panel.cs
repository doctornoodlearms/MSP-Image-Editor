using Godot;
using MSP;

public class ColorView_Panel : Control {

	ColorPickerButton mainColor;
	ColorPickerButton secondaryColor;

	public override void _Ready() {

		mainColor = GetNode("ColorList/MainColor") as ColorPickerButton;
		secondaryColor = GetNode("ColorList/SecondaryColor") as ColorPickerButton;

		mainColor.Connect("color_changed", this, nameof(onColorPicker_Changed));

		mainColor.Color = Common.self.selectedColor;

		base._Ready();
	}

	public override void _Input(InputEvent @event) {

		if(Input.IsActionJustPressed("Color_Swap")) {

			Common.self.selectedColor = secondaryColor.Color;
			secondaryColor.Color = mainColor.Color;
			mainColor.Color = Common.self.selectedColor;
		}
		base._Input(@event);
	}

	void onColorPicker_Changed(Color color) {
	
		GetNode<Common>("/root/Common").selectedColor = color;
	}
}
