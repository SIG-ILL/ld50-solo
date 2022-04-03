using UnityEngine;

public class RepulsorField : MonoBehaviour {
	[SerializeField]
	private float repulsionForce = 2.5f;
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

		Vector2 normal = (collision.transform.position - transform.position).normalized;
		Vector2 incomingDirection = collision.GetComponent<CometMovement>().ForwardDirection;
		Vector2 reflection = incomingDirection - (2 * Vector2.Dot(incomingDirection, normal) * normal);
		collision.GetComponent<CometMovement>().Push(repulsionForce * reflection);

		charges--;
	}
}
