using Godot;

public class Eraser_Property : VBoxContainer {

	HSlider sizeValueSlider;
	LineEdit sizeValueEdit;

	public override void _Ready() {

		sizeValueSlider = GetNode<HSlider>("Size/ValueSlider");
		sizeValueEdit = GetNode<LineEdit>("Size/ValueEdit");

		UpdateValue(ToolProperties.cursorSize);

		sizeValueSlider.Connect("value_changed", this, nameof(onSizeSlider_Changed));
		sizeValueEdit.Connect("text_entered", this, nameof(onSizeEdit_Changed));
		base._Ready();
	}

	void UpdateValue(int value) {

		value = value < ToolProperties.maxCursorSize ? value : ToolProperties.maxCursorSize;
		value = value < 1 ? 1 : value;

		ToolProperties.cursorSize = value;

		sizeValueEdit.Text = value.ToString() + "px";
		sizeValueSlider.Value = value;
	}

	void onSizeSlider_Changed(float value) {

		UpdateValue((int) value);
	}

	void onSizeEdit_Changed(string text) {

		text = text.Replace("px", "");
		int value = int.Parse(text);
		UpdateValue(value);
	}
}
