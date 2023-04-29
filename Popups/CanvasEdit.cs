using Godot;
using MSP;

public class CanvasEdit : PanelContainer {

	float x = 0;
	float y = 0;

	LineEdit editX;
	LineEdit editY;

	[Signal]
	public delegate void Confirmed(int x, int y);

	public override void _Ready() {

		editX = GetNode<LineEdit>("Margin/Vbox/SizeX/LineEdit");
		editY = GetNode<LineEdit>("Margin/Vbox/SizeY/LineEdit");

		GetNode<Button>("Margin/Vbox/Buttons/Confirm").Connect("pressed", this, nameof(onConfirm_Pressed));
		GetNode<Button>("Margin/Vbox/Buttons/Cancel").Connect("pressed", this, nameof(Close));

		editX.Connect("text_changed", this, nameof(onSizeX_TextChanged));
		editY.Connect("text_changed", this, nameof(onSizeY_TextChanged));
	}

	public void Popup() {

		x = Common.self.gridSize.x;
		y = Common.self.gridSize.y;

		editX.Text = x.ToString();
		editY.Text = y.ToString();
		Visible = true;
	}

	void Close() {

		Visible = false;
	}

	void onConfirm_Pressed() {

		EmitSignal(nameof(Confirmed), new Vector2(x, y));
		Close();
	}

	void onSizeX_TextChanged(string text) {

		if(text.IsValidInteger()) {

			x = int.Parse(text);
		}
	}
	void onSizeY_TextChanged(string text) {

		if(text.IsValidInteger()) {

			y = int.Parse(text);
		}
	}
}