using Godot;

public class Eraser_Property : VBoxContainer {

	Slider_BaseProperty sizeProperty;

	public override void _Ready() {

		GetNode<Slider_BaseProperty>("Size").Connect(nameof(Slider_BaseProperty.ValueChanged), this, nameof(onSize_Changed));
		base._Ready();
	}

	void onSize_Changed(int value) {

		ToolProperties.cursorSize = value;
	}
}
