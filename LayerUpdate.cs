
public struct LayerUpdate {

	int _index;
	bool _visible;

	public int index {
		get { return _index; }
	}
	public bool visible {
		get { return _visible; }
	}

	public LayerUpdate(int index, bool visible) {

		_index = index;
		_visible = visible;
	}
}
