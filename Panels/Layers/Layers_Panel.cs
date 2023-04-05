using Godot;
using MSP;

public class Layers_Panel : VBoxContainer {

	PackedScene _scene;

	LineEdit layerName;
	Button addLayer;

	Theme layerMainTheme;
	Theme layerSelectTheme;

	public override void _EnterTree() {

		_scene = GD.Load<PackedScene>("res://Panels/Layers/Layer_Instance.tscn");

		layerMainTheme = GD.Load<Theme>("res://Themes/LayerMain_Theme.tres");
		layerSelectTheme = GD.Load<Theme>("res://Themes/LayerSelected_Theme.tres");
		base._EnterTree();
	}

	public override void _Ready() {

		layerName = GetNode<LineEdit>("Buttons/Name");
		addLayer = GetNode<Button>("Buttons/AddLayer");

		addLayer.Connect("pressed", this, nameof(onAddLayer_Pressed));
	}

	void AddNewLayer() {
	
		Layer_Instance newLayer = _scene.Instance<Layer_Instance>();
		newLayer.layerName = layerName.Text;
		newLayer.Connect(nameof(Layer_Instance.LayerPressed), this, nameof(onLayer_Pressed));
		newLayer.Connect(nameof(Layer_Instance.LayerRemoved), this, nameof(onLayer_Removed));
		newLayer.Connect(nameof(Layer_Instance.LayerVisibility), this, nameof(onLayer_Visibility));
		AddChild(newLayer);
	}

	void onAddLayer_Pressed() {

		AddNewLayer();
	}

	void onLayer_Pressed(int layerIndex) {

		foreach(Control i in GetChildren()) {

			if(i is Layer_Instance) {

				i.Theme = layerMainTheme;
			}
		}
		GetChild<Control>(layerIndex + 1).Theme = layerSelectTheme;
		Common.self.currentLayer = layerIndex;
	}

	void onLayer_Removed(int layerIndex) {

		GetChild(layerIndex).QueueFree();
		Common.self.removeLayerQueue = layerIndex - 1;
	}

	void onLayer_Visibility(int layerIndex, bool layerVisible) {

		Common.self.layerUpdate = new LayerUpdate(layerIndex, layerVisible);
	}
}
