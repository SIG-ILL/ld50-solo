using UnityEngine;

public class Accelerator : Building {
	[SerializeField]
	private AcceleratorField field;

	private void Awake() {
		field.ChargesDepleted += OnChargesDepleted;
	}

	private void Update() {
		if(!IsActive) {
			return;
		}

		transform.Rotate(new Vector3(0, 0, Time.deltaTime * -90));
	}

	private void OnChargesDepleted() {
		// TODO: Remove energy drain from energy manager since this building has become useless now?
	}


	public override void Activate() {
		base.Activate();

		field.Activate();
	}

	public override void Deactivate() {
		base.Deactivate();

		field.Deactivate();
	}
}
