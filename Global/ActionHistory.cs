using Godot;
using Godot.Collections;
using MSP.Tools;

namespace MSP.Actions{

	public class ActionHistory : Node {

		[Signal] public delegate void RepeatAction(Color color, int index); 

		Array<Action> history = new Array<Action>();

		Timer revertActionTimer;

		public override void _Ready() {
			
			revertActionTimer = new Timer();
			revertActionTimer.WaitTime = 0.3f;
			revertActionTimer.OneShot = false;
			revertActionTimer.Connect("timeout", this, nameof(onRevertActionTimeout));
			AddChild(revertActionTimer);
			base._Ready();
		}

		public override void _Input(InputEvent @event) {

			if(Input.IsActionJustPressed("Action_Redo") && Input.IsPhysicalKeyPressed((int) KeyList.Control)) {

				onRevertActionTimeout();
				revertActionTimer?.Start();
			}
			if(Input.IsActionJustReleased("Action_Redo") || !Input.IsPhysicalKeyPressed((int) KeyList.Control)) {

				revertActionTimer.Stop();
			}
			base._Input(@event);
		}

		public void RecordAction(Color color, int index) {

			history.Add(new Action(color, index));
		}

		void onRevertActionTimeout() {

			if(history.Count < 1) {

				return;
			}

			Action currentAction = history[history.Count - 1];

			Common.self.pixelList[currentAction.pixelIndex].color = currentAction.color;
			history.RemoveAt(history.Count - 1);
		}
	}
}