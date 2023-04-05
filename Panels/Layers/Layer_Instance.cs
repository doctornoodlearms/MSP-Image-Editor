using Godot;
using MSP;

public class Layer_Instance : Control {

	[Signal] public delegate void LayerPressed(int index);
	[Signal] public delegate void LayerRemoved(int index);
	[Signal] public delegate void LayerVisibility(int index, bool visible);

	public string layerName {

		get { return GetNode<Label>("Label").Text; }
		set { GetNode<Label>("Label").Text = value; }
	}

	bool _layerVisible = true;

	public override void _Ready() {

		GetNode<Button>("Delete").Connect("pressed", this, nameof(onDelete_Pressed));
		GetNode<Button>("Visible").Connect("toggled", this, nameof(onVisible_Pressed));
		base._Ready();
	}

	public override void _GuiInput(InputEvent @event) {

		if(@event is InputEventMouseButton) {
		
			InputEventMouseButton mouseEvent = (InputEventMouseButton) @event;
			if(!mouseEvent.Pressed || mouseEvent.ButtonIndex != 1) {

				return;
			}
			EmitSignal(nameof(LayerPressed), GetIndex() - 1);
		}
		base._GuiInput(@event);
	}

	void onDelete_Pressed() {

		EmitSignal(nameof(LayerRemoved), GetIndex());
	}

	void onVisible_Pressed(int toggle) {

		EmitSignal(nameof(LayerVisibility), GetIndex() - 1, toggle);
	}
}
