using Godot;
using MSP;

public class File : MenuButton {

	enum SaveModes {
	
		SAVEMODE_PREXPORT,
		SAVEMODE_PRIMPORT,
		SAVEMODE_EXPORT,
	};
	SaveModes saveMode;

	FileDialog dialog;

	public override void _Ready() {

		GetPopup().Connect("id_pressed", this, nameof(onPopupPressed));
		dialog = GetTree().Root.GetNode<FileDialog>("Root/FileDialog");
		dialog.Connect("file_selected", this, nameof(onFileDialog_FileSelected));
	}

	private void onFileDialog_FileSelected(string location) {

		switch(saveMode) {
		
			case SaveModes.SAVEMODE_EXPORT:
				Common.self.SaveFile(location);
				break;

			case SaveModes.SAVEMODE_PREXPORT:
				Common.self.SaveProject(location);
				break;

			case SaveModes.SAVEMODE_PRIMPORT:
				Common.self.LoadProject(location);
				break;
		}
	}

	private void onPopupPressed(int id) {

		switch(id) {
		
			case 0:
				Common.self.drawDisabled = true;
				dialog.Mode = FileDialog.ModeEnum.SaveFile;
				saveMode = SaveModes.SAVEMODE_EXPORT;
				dialog.Filters = new string[] { "*.png" };
				dialog.PopupCentered();
				break;

			case 1:
				saveMode = SaveModes.SAVEMODE_PREXPORT;
				dialog.Mode = FileDialog.ModeEnum.SaveFile;
				dialog.Filters = new string[] { "*.nimd" };
				dialog.PopupCentered();
				break;

			case 2:
				saveMode = SaveModes.SAVEMODE_PRIMPORT;
				dialog.Mode = FileDialog.ModeEnum.OpenFile;
				dialog.Filters = new string[] { "*.nimd" };
				dialog.PopupCentered();
				break;
		}
	}
}
