using Godot;

public class Pencil_Property : VBoxContainer {

	Slider_BaseProperty sizeProperty;
	Slider_BaseProperty randProperty;

	public override void _Ready() {

		GetNode<Slider_BaseProperty>("Size").Connect(nameof(Slider_BaseProperty.ValueChanged), this, nameof(onSize_Changed));
		GetNode<Slider_BaseProperty>("RandColor").Connect(nameof(Slider_BaseProperty.ValueChanged), this, nameof(onRandColor_Changed));
		base._Ready();
	}

	void onSize_Changed(float value) {

		ToolProperties.cursorSize = (int) value;
	}

	void onRandColor_Changed(float value) {

		ToolProperties.colorRandomize = value / 100;
	}
}
