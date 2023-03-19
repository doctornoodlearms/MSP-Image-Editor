using Godot;

public class BaseProperty : Object {

	public string name = "";

	public override string ToString() {

		return "[BaseProperty]: " + name;
	}
}