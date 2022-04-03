using UnityEngine;

public class AcceleratorField : MonoBehaviour {
	[SerializeField]
	private float accelerationForce = 2.5f;
	[SerializeField]
	private int charges = 3;

	private bool isActive = false;

	public void Activate() {
		isActive = true;
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if(!isActive || charges <= 0) {
			return;
		}

		collision.GetComponent<CometMovement>().Push(accelerationForce * collision.GetComponent<CometMovement>().ForwardDirection);

		charges--;
	}
}
