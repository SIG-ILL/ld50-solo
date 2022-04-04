using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
	[SerializeField]
	private Button exitButton;

	private void Awake() {
		if(Application.platform == RuntimePlatform.WebGLPlayer) {
			exitButton.gameObject.SetActive(false);
		}
	}
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
