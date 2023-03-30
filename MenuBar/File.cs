using Godot;
using MSP;

public class File : MenuButton {

	public override void _Ready() {

		GetPopup().Connect("id_pressed", this, nameof(onPopupPressed));
	}

	public override void _Pressed() {

		GD.Print("Pressed");
		base._Pressed();
	}

	private void onPopupPressed(int id) {

		switch(id) {
		
			case 0:
				Common.self.drawDisabled = true;
				(GetTree().Root.GetNode("Background/FileDialog") as FileDialog).PopupCentered();
				break;
		}
	}
}
