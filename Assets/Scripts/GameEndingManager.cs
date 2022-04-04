using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameEndingManager : MonoBehaviour {
	private bool isGameEnding;

	private void Awake() {
		isGameEnding = false;
	}

	public void TriggerGameWin() {
		if(!isGameEnding) {
			isGameEnding = true;
			StartCoroutine(WinTimerCoroutine());
		}
	}

	public void TriggerGameLose() {
		if(!isGameEnding) {
			isGameEnding = true;
			StartCoroutine(LoseTimerCoroutine());
		}
	}

	private IEnumerator WinTimerCoroutine() {
		yield return new WaitForSeconds(4.0f);

		SceneManager.LoadScene("EndScreenWin");
	}

	private IEnumerator LoseTimerCoroutine() {
		yield return new WaitForSeconds(3.0f);

		SceneManager.LoadScene("EndScreenLose");
	}
}
