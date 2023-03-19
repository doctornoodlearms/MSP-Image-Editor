using Godot;
using Godot.Collections;

namespace MSP.Tools {

	public class Tools_Panel : GridContainer {

		void OnToolPressed(Common.Tools tool) {

			(GetNode("/root/Common") as Common).SetTool(tool);
		}
	}
}
