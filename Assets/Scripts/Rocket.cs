using UnityEngine;

public class Rocket : MonoBehaviour {
	[SerializeField]
	private GameObject exhaustFlame;

	private bool hasBeenLaunched;
	private Vector3 flightTarget;
	private Vector3 currentSpeed;
	private Vector3 directionOnPlanet;

	private void Awake() {
		currentSpeed = Vector3.zero;
	}

	public void Launch() {
		exhaustFlame.SetActive(true);
		hasBeenLaunched = true;

		float angle = Vector2.SignedAngle(Vector2.right, transform.position);
		flightTarget = (4 * transform.position.normalized) + (Quaternion.AngleAxis(angle, Vector3.forward) * new Vector3(2, -4, 0));
		directionOnPlanet = transform.position.normalized;

		FindObjectOfType<GameEndingManager>().TriggerGameWin();
	}

	private void Update() {
		if(!hasBeenLaunched) {
			return;
		}

		transform.position += Time.deltaTime * currentSpeed;
		transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(currentSpeed.y, currentSpeed.x) * Mathf.Rad2Deg, Vector3.forward) * Quaternion.FromToRotation(Vector3.left, Vector3.up);
		if(currentSpeed.magnitude < 1) {
			currentSpeed += Time.deltaTime * directionOnPlanet;
		}
		else {
			currentSpeed += Time.deltaTime * (flightTarget - transform.position).normalized;
		}		
	}
}
