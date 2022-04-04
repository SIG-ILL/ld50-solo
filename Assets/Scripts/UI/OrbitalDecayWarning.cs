using UnityEngine;
using TMPro;
using System.Collections;

public class OrbitalDecayWarning : MonoBehaviour{
	[SerializeField]
	private CometMovement cometMovement;

	private void Awake() {
		cometMovement.OrbitalDecayStarted += OnOrbitalDecayStarted;
	}

	private void OnOrbitalDecayStarted() {
		StartCoroutine(WarningTextBlinkingCoroutine());
	}

	private IEnumerator WarningTextBlinkingCoroutine() {
		TextMeshProUGUI warningText = GetComponent<TextMeshProUGUI>();
		for(int i = 0; i < 10; i++) {
			warningText.enabled = !warningText.enabled;
			yield return new WaitForSeconds(0.6f);
		}
	}
}
