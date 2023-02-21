using Godot;
using Godot.Collections;

namespace MSP.Tools {

	public class ToolScene : GridContainer {

		[Export] Array<Common.Tools> toolList = new Array<Common.Tools>();

		public override void _Ready() {

			foreach(Common.Tools tool in toolList) {
			
				Button button = new Button();
				button.Text = tool.ToString();
				button.Connect("pressed", this, nameof(OnToolPressed), new Array(tool));

				AddChild(button);
			}
			base._Ready();
		}

		void OnToolPressed(Common.Tools tool) {

			(GetNode("/root/Common") as Common).currentTool = tool;
		}
	}
}