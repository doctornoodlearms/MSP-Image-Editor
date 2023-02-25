using Godot;
using Godot.Collections;
using MSP.Tools;

namespace MSP.Actions{

	public class ActionHistory : Node {

		[Signal] public delegate void RepeatAction(Color color, int index); 

		Array<Action> history = new Array<Action>();

		public override void _Input(InputEvent @event) {

			if(Input.IsActionJustPressed("Action_Redo") && Input.IsPhysicalKeyPressed((int) KeyList.Control)) {

				RevertAction();
			}
			base._Input(@event);
		}

		public void RecordAction(Color color, int index) {

			history.Add(new Action(color, index));
		}

		void RevertAction() {

			if(history.Count < 1) {

				return;
			}

			Action currentAction = history[history.Count - 1];

			Common.self.pixelList[currentAction.pixelIndex].color = currentAction.color;
			history.RemoveAt(history.Count - 1);
		}
	}
}