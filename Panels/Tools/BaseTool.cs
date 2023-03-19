using System;
using Godot;

namespace MSP.Tools {



	public class BaseTool : Button {

		[Export] readonly Common.Tools tool = Common.Tools.TOOL_NONE;

		public override void _Ready() {

			Connect("pressed", GetParent(), "OnToolPressed", new Godot.Collections.Array(tool));
			base._Ready();
		}
	}
}
