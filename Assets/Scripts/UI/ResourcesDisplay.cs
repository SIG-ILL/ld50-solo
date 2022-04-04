using UnityEngine;
using TMPro;

public class ResourcesDisplay : MonoBehaviour {
	[SerializeField]
	private ResourcesManager resourcesManager;

	[SerializeField]
	private TextMeshProUGUI energyText;
	[SerializeField]
	private TextMeshProUGUI fuelText;
	[SerializeField]
	private TextMeshProUGUI metalText;

	private void Awake() {
		resourcesManager.ResourcesChanged += OnResourcesChanged;
	}

	private void Start() {
		OnResourcesChanged(resourcesManager.CurrentResources);
	}

	private void OnResourcesChanged(ResourcesData resources) {
		energyText.SetText("E: {0}", resources.EnergyBalance);
		fuelText.SetText("F: {0}", resources.Fuel);
		metalText.SetText("M: {0}", resources.Metal);
	}
}
