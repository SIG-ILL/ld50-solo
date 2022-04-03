using UnityEngine;

public class EnergyManager : MonoBehaviour {
	// Keeps track of energy usage. If there is an energy shortage it manages prioritized building shutdown.
	// Priority order:
	//	1. Spaceport
	//	2. Accelerator
	//	3. Repulsor
	//	4. Fuel Refinery
	//	5. Metal mine

	[SerializeField]
	private ResourcesManager resourcesManager;

	private void Awake() {
		resourcesManager.ResourcesChanged += OnResourcesChanged;
	}

	private void OnResourcesChanged(ResourcesData resources) {
		if(resources.Energy > 0) {
			return;
		}

		// TODO: Shut down buildings to balance out power
	}
}
