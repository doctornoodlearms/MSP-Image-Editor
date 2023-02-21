using System;
using Godot;

namespace MSP {

	public class Common : Node {

		public enum Tools {
		
			TOOL_PENCIL,
			TOOL_ERASER
		}
		public Tools currentTool = Tools.TOOL_PENCIL;

		public Color selectedColor = new Color(0, 0, 0);
	}
}