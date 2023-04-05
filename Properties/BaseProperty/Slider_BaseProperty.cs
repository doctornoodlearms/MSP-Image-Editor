using Godot;

[Tool]
public class Slider_BaseProperty : HBoxContainer {

	[Signal] public delegate void ValueChanged(float value);

	float _value = 0;
	string _propertyName = "";
	string _propertyUnit = "";
	float _minValue = 0;
	float _maxValue = 1;

	[Export]
	public float Value {

		get => _value;
		set => setValue(value);
	}

	[Export]
	public string PropertyName {

		get => _propertyName;
		set => setPropertyName(value);
	}

	[Export]
	public float MinValue {
		get => _minValue;
		set {
			_minValue = value;
			GetNode<HSlider>("HSlider").MinValue = value;
		}
	}

	[Export]
	public float MaxValue {

		get => _maxValue;
		set {
			_maxValue = value;
			GetNode<HSlider>("HSlider").MaxValue = value;
		}
	}
	[Export]
	public string PropertyUnit {

		get => _propertyUnit;
		set => _propertyUnit = value;
	}

	private void setPropertyName(string value) {

		_propertyName = value;
		GetNode<Label>("Label").Text = value;
	}

	private void setValue(float value) {
	
		_value = value;
		GetNode<HSlider>("HSlider").Value = value;
		GetNode<LineEdit>("LineEdit").Text = value.ToString() + _propertyUnit;
		EmitSignal(nameof(ValueChanged), _value);
	}

	private void onSlider_ValueChanged(float value) {

		GD.Print(value);
		setValue(value);
	}

	private void onLineEdit_TextEntered(string value) {

		value = value.ReplaceN(_propertyUnit, "");
		if(!value.IsValidInteger()){
			
			return;
		}
		setValue(float.Parse(value));
	}

	public override void _Ready() {

		if(!Engine.EditorHint) {

			GetNode<HSlider>("HSlider").Connect("value_changed", this, nameof(onSlider_ValueChanged));
			GetNode<LineEdit>("LineEdit").Connect("text_entered", this, nameof(onLineEdit_TextEntered));
			GetNode<HSlider>("HSlider").MaxValue = _maxValue;
			GetNode<HSlider>("HSlider").MinValue = _minValue;
		}
	}
}
