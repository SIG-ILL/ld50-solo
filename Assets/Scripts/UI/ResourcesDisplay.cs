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
		energyText.SetText("Energy: {0}", resources.EnergyBalance);
		fuelText.SetText("Fuel: {0}", resources.Fuel);
		metalText.SetText("Metal: {0}", resources.Metal);
	}
}
