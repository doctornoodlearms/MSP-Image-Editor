using Godot;
using MSP;

public class Image : MenuButton {

	public override void _Ready() {

		GetPopup().Connect("id_pressed", this, nameof(onPopup_IdPressed));
		GetTree().Root.GetNode<CanvasEdit>("Root/CanvasEdit").Connect(nameof(CanvasEdit.Confirmed), this, nameof(onCanvasEdit_Confirmed));
	}

	void onPopup_IdPressed(int id) {

		switch(id) {

			case 0:

				GetTree().Root.GetNode<CanvasEdit>("Root/CanvasEdit").Popup();
				break;
		}
	}

	void onCanvasEdit_Confirmed(Vector2 size) {

		Common.self.SetGridSize(size);
	}
}