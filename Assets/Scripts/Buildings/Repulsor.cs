using UnityEngine;

public class Repulsor : Building {
	[SerializeField]
	private RepulsorField field;

	public override void Activate() {
		base.Activate();

		field.Activate();
	}

	public override void Deactivate() {
		base.Deactivate();

		field.Deactivate();
	}
}
