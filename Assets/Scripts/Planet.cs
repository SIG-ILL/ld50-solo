using UnityEngine;

public class Planet : MonoBehaviour {
	[SerializeField]
	private float gravitationalForce = 9.81f;

	public float GravitationalForce { get { return gravitationalForce; } }

	private void OnCollisionEnter2D(Collision2D collision) {
		Debug.LogFormat("Time for collision to take place: {0}", Time.timeSinceLevelLoad);
	}
}
