using Godot;
using Godot.Collections;
using MSP.Tools;
using static MSP.Actions.ActionHistory;

namespace MSP.Actions{

	public class ActionHistory : Node {

		//[Signal] public delegate void ReverseAction(Color color, int index);

		Array<Object> history = new Array<Object>();

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

		public void RecordRepeatAction(int repeatCount) {

			history.Add(new RepeatAction(repeatCount));
		}

		void onRevertActionTimeout() {

			if(history.Count < 1) {

				return;
			}

			Object action = history[history.Count - 1];

			if(action is Action) {

				ReverseAction((Action) action);
				return;
			}

			if(action is RepeatAction) {


				RepeatAction repeatAction = (RepeatAction) action;

				int removeCount = repeatAction.repeatCount;
				history.RemoveAt(history.Count - 1);

				for(int i = 0; i < removeCount; i++) {

					ReverseAction((Action) history[history.Count - 1]);
				}

				return;
				
			}
		}

		void ReverseAction(Action action) {
			
			Common.self.pixelList[action.pixelIndex].color = action.color;
			history.RemoveAt(history.Count - 1);
		}
	}
}