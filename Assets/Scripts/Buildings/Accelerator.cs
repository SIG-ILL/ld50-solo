using UnityEngine;

public class Accelerator : Building {
	[SerializeField]
	private AcceleratorField field;

	public override void Activate() {
		base.Activate();

		field.Activate();
	}
}
