using UnityEngine;

public class Repulsor : Building {
	[SerializeField]
	private RepulsorField field;

	public override void Activate() {
		base.Activate();

		field.Activate();
	}
}
