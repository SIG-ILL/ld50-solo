using UnityEngine;

public class AcceleratorField : MonoBehaviour {
	[SerializeField]
	private float accelerationForce = 2.5f;
	[SerializeField]
	private int charges = 3;

	private bool isActive = false;
	private SpriteRenderer spriteRenderer;

	private void Awake() {
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	public void Activate() {
		spriteRenderer.enabled = true;
		isActive = true;
	}

	public void Deactivate() {
		spriteRenderer.enabled = false;
		isActive = false;
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if(!isActive || charges <= 0) {
			return;
		}

		collision.GetComponent<CometMovement>().Push(accelerationForce * collision.GetComponent<CometMovement>().ForwardDirection);

		charges--;
	}
}
