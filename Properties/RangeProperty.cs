using Godot;

public class RangeProperty : BaseProperty {

	public int value = 1;
	public int min = 0;
	public int max = 0;

	public override string ToString() {

		string format= "[RangeProperty]: {0}, Min: {1}, Max: {2}";
		return string.Format(format, name, min, max);
	}
}

