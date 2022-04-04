using UnityEngine;

public class Spaceport : Building {
	[SerializeField]
	private GameObject rocketSpawner;
	[SerializeField]
	private GameObject rocketPrefab;

	private GameObject rocket;

	private void Awake() {
		rocket = Instantiate<GameObject>(rocketPrefab);
		rocket.transform.position = rocketSpawner.transform.position;
		rocket.transform.parent = rocketSpawner.transform;
		rocket.transform.localRotation = Quaternion.identity;
	}

	public void LaunchRocket() {
		rocket.transform.parent = null;
		rocket.GetComponent<Rocket>().Launch();
	}
}
