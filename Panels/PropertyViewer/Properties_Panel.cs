using Godot;
using MSP;
using MSP.Tools;

public class Properties_Panel : Node {

	Directory dir;

	public override void _Ready() {

		dir = new Directory();

		Common.self.Connect(nameof(Common.ToolChanged), this, nameof(onToolChanged));
	}

	public void onToolChanged(Common.Tools currentTool) {

		string sceneLocation = "res://Properties/{0}_Property.tscn";


		if(GetChildCount() > 0) {

			GetChild(0).QueueFree();
		}

		switch(currentTool) {

			case Common.Tools.TOOL_PENCIL:
				sceneLocation = string.Format(sceneLocation, "Pencil");
				break;

			case Common.Tools.TOOL_ERASER:
				sceneLocation = string.Format(sceneLocation, "Eraser");
				break;
		}

		if(!dir.FileExists(sceneLocation)) {

			return;
		}

		Control propertyScene = GD.Load<PackedScene>(sceneLocation).Instance<Control>();

		AddChild(propertyScene);
	}
}
