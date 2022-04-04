using UnityEngine;
using System;
using System.Collections;

public class CometMovement : MonoBehaviour {
    [SerializeField]
    private Transform planetTransform;

	[SerializeField]
	private Vector2 initialForwardVector = new Vector2(10, 0);
	[SerializeField]
	private float orbitalDecay = 0.0f;
	[SerializeField]
	private float orbitalDecayIncrease = 0.1f;
	[SerializeField]
	private float orbitalDecayIncreaseIncrease = 0.01f;
	[SerializeField]
	private float orbitalDecayStartDelayInSeconds = 0;

	public Vector3 ForwardDirection { get { return forwardVector.normalized; } }

	private event Action orbitalDecayStartedEvent;
	public event Action OrbitalDecayStarted {
		add { orbitalDecayStartedEvent += value; }
		remove { orbitalDecayStartedEvent -= value; }
	}

	private event Action planetCollisionEvent;
	public event Action PlanetCollision {
		add { planetCollisionEvent += value; }
		remove { planetCollisionEvent -= value; }
	}

	private Vector3 forwardVector;
	private bool canUpdatePosition;
	private bool isDecayingOrbit;

	private void Awake() {
		forwardVector = initialForwardVector;
		canUpdatePosition = true;
		isDecayingOrbit = false;

		StartCoroutine(OrbitalDecayStartDelayCoroutine());
		//StartCoroutine(LineRendererCoroutine());
	}

	private void FixedUpdate() {
		if(!canUpdatePosition) {
			return;
		}

		Vector3 gravityAcceleration = ((planetTransform.GetComponent<Planet>().GravitationalForce + orbitalDecay) * (planetTransform.position - transform.position).normalized);
		forwardVector += Time.deltaTime * gravityAcceleration;

		transform.position += Time.deltaTime * forwardVector;

		Vector2 direction = transform.position - planetTransform.position;
		transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg, Vector3.forward) * Quaternion.FromToRotation(Vector3.right, Vector3.up);

		orbitalDecay += orbitalDecayIncrease;
		orbitalDecayIncrease += orbitalDecayIncreaseIncrease;

		// Slow down if getting close to planet.
		if(Vector2.Distance(planetTransform.position, transform.position) < 3.0f) {
			forwardVector -= Time.deltaTime * 0.25f * forwardVector.normalized;
		}

		if(isDecayingOrbit) {
			// And slow down every update.
			forwardVector -= Time.deltaTime * 0.05f * forwardVector.normalized;
		}
	}

	private void OnCollisionEnter2D(Collision2D collision) {
		canUpdatePosition = false;

		if(planetCollisionEvent != null) {
			planetCollisionEvent();
		}

		GetComponent<ParticleSystem>().Stop();
	}

	public void Push(Vector3 pushVector) {
		forwardVector += pushVector;
	}

	private IEnumerator OrbitalDecayStartDelayCoroutine() {
		yield return new WaitForSeconds(orbitalDecayStartDelayInSeconds);

		isDecayingOrbit = true;

		if(orbitalDecayStartedEvent != null) {
			orbitalDecayStartedEvent();
		}
	}

	private IEnumerator LineRendererCoroutine() {
		LineRenderer lineRenderer = GetComponent<LineRenderer>();

		while(true) {
			lineRenderer.positionCount++;
			lineRenderer.SetPosition(lineRenderer.positionCount - 1, transform.position);
			yield return new WaitForSeconds(0.05f);
		}
	}
}
