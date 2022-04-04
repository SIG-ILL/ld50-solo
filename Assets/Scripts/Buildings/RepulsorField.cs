using UnityEngine;
using System;

public class RepulsorField : MonoBehaviour {
	[SerializeField]
	private float repulsionForce = 2.5f;
	[SerializeField]
	private int charges = 3;

	private event Action chargesDepletedEvent;
	public event Action ChargesDepleted {
		add { chargesDepletedEvent += value; }
		remove { chargesDepletedEvent -= value; }
	}

	private bool isActive = false;
	private SpriteRenderer spriteRenderer;

	private void Awake() {
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	public void Activate() {
		if(charges > 0) {
			spriteRenderer.enabled = true;
		}
		
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

		Vector2 normal = (collision.transform.position - transform.position).normalized;
		Vector2 incomingDirection = collision.GetComponent<CometMovement>().ForwardDirection;
		Vector2 reflection = incomingDirection - (2 * Vector2.Dot(incomingDirection, normal) * normal);
		collision.GetComponent<CometMovement>().Push(repulsionForce * reflection);

		charges--;

		if(charges == 0) {
			spriteRenderer.enabled = false;

			if(chargesDepletedEvent != null) {
				chargesDepletedEvent();
			}
		}
	}
}
