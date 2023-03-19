using Godot;
using System.Reflection;

public class Pencil_Property : VBoxContainer {

	HSlider sizeValueSlider;
	LineEdit sizeValueEdit;

	public override void _Ready() {

		sizeValueSlider = GetNode<HSlider>("Size/ValueSlider");
		sizeValueEdit = GetNode<LineEdit>("Size/ValueEdit");

		sizeValueSlider.Connect("value_changed", this, nameof(onSizeSlider_Changed));
		sizeValueEdit.Connect("text_entered", this, nameof(onSizeEdit_Changed));
		base._Ready();
	}

	void onSizeSlider_Changed(float value) {

		sizeValueEdit.Text = value.ToString() + "px";
		ToolProperties.cursorSize = (int) value;
	}

	void onSizeEdit_Changed(string text) {

		text = text.Replace("px", "");

		int value = int.Parse(text);

		value = value < ToolProperties.maxCursorSize ? value : ToolProperties.maxCursorSize;
		value = value < 1 ? 1 : value;

		sizeValueEdit.Text = value.ToString() + "px";
		sizeValueSlider.Value = value;
		ToolProperties.cursorSize = value;
	}
}
