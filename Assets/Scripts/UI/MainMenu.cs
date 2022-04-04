using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
	public void OnGameInfoButtonClicked() {
		SceneManager.LoadScene("GameInfo");
	}

	public void OnPlayClicked() {
		SceneManager.LoadScene("Level1");
	}

	public void OnExitClicked() {
		Application.Quit();
	}
}
