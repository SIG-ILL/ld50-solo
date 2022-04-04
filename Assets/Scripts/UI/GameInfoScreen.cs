using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInfoScreen : MonoBehaviour {
	public void OnBackButtonClicked() {
		SceneManager.LoadScene("MainMenu");
	}
}
