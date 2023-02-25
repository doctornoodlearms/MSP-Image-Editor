using Godot;
using Godot.Collections;

namespace MSP.Tools {

	public class ToolScene : GridContainer {

		void OnToolPressed(Common.Tools tool) {

			(GetNode("/root/Common") as Common).currentTool = tool;
		}
	}
}