using Godot;
using Godot.Collections;

namespace MSP.Actions{

	public class ActionHistory : Node {

		Array<Object> history = new Array<Object>();

		Timer revertActionTimer;
		float initalTimeDelay = .5f;
		float heldTimeDelay = .01f;

		public override void _Ready() {
			
			revertActionTimer = new Timer();
			revertActionTimer.WaitTime = initalTimeDelay;
			revertActionTimer.OneShot = true;
			revertActionTimer.Connect("timeout", this, nameof(onRevertActionTimeout));
			AddChild(revertActionTimer);

			base._Ready();
		}

		public override void _Input(InputEvent @event) {

			if(Input.IsActionJustPressed("Action_Redo") && Input.IsPhysicalKeyPressed((int) KeyList.Control)) {

				revertActionTimer?.Start();
				onRevertActionTimeout();
			}
			if(Input.IsActionJustReleased("Action_Redo") || !Input.IsPhysicalKeyPressed((int) KeyList.Control)) {

				revertActionTimer.WaitTime = initalTimeDelay;
				revertActionTimer.Stop();
			}
			base._Input(@event);
		}

		public void RecordAction(Color color, int pixelIndex, int layerIndex) {

			history.Add(new Action(color, pixelIndex, layerIndex));
		}

		public void RecordRepeatAction(int repeatCount) {

			history.Add(new RepeatAction(repeatCount));
		}

		void onRevertActionTimeout() {

			if(history.Count < 1) {

				return;
			}

			if(revertActionTimer.TimeLeft <= 0) {

				revertActionTimer.WaitTime = heldTimeDelay;
			}

			Object action = history[history.Count - 1];

			if(action is Action) {

				ReverseAction((Action) action);
				revertActionTimer.Start();
			}

			if(action is RepeatAction) {


				RepeatAction repeatAction = (RepeatAction) action;

				int removeCount = repeatAction.repeatCount;
				history.RemoveAt(history.Count - 1);

				for(int i = 0; i < removeCount; i++) {

					ReverseAction((Action) history[history.Count - 1]);
				}

				revertActionTimer.Start();
			}
		}

		void ReverseAction(Action action) {
			
			Common.self.pixelList[action.pixelIndex].setColor(action.color, action.layerIndex);
			history.RemoveAt(history.Count - 1);
		}
	}
}