using Godot;
namespace MSP.Actions {

	class RepeatAction : Object{

		public int repeatCount;

		public RepeatAction(int repeatCount){

			this.repeatCount = repeatCount;
		}
	}
}