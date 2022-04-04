using UnityEngine;
using System.Collections.Generic;

public class EnergyManager : MonoBehaviour {
	// Keeps track of energy usage. If there is an energy shortage it manages prioritized building shutdown.
	// Priority order:
	//	1. Radar
	//	2. Spaceport
	//	3. Accelerator
	//	4. Repulsor
	//	5. Fuel Refinery (always keep 1 active)
	//	6. Metal mine (always keep 1 active)

	[SerializeField]
	private ResourcesManager resourcesManager;

	private List<List<Building>> buildings;

	private void Awake() {
		buildings = new List<List<Building>>();
		for(int i = 0; i < 6; i++) {
			buildings.Add(new List<Building>());
		}
	}

	public void AddNewBuilding(GameObject buildingObject) {
		Building building = buildingObject.GetComponent<Building>();

		if(building as Radar != null) {
			buildings[0].Add(building);
		}
		else if(building as Spaceport != null) {
			buildings[1].Add(building);
		}
		else if(building as Accelerator != null) {
			buildings[2].Add(building);
		}
		else if(building as Repulsor != null) {
			buildings[3].Add(building);
		}
		else if(buildingObject.name.Contains("Fuel")) {
			buildings[4].Add(building);
		}
		else if(buildingObject.name.Contains("Metal")) {
			buildings[5].Add(building);
		}
		else if(buildingObject.name.Contains("Power")) { }
		else {
			throw new System.Exception("EnergyManager tried to add a building it could not identify!");
		}

		UpdateEnergyDistribution();
	}

	private void UpdateEnergyDistribution() {
		int currentEnergyUsage = 0;

		foreach(List<Building> buildingList in buildings) {
			foreach(Building building in buildingList) {
				if(building.IsActive) {
					currentEnergyUsage += building.GetComponent<BuildingProperties>().EnergyCost;
				}
			}
		}

		int energyGenerated = resourcesManager.CurrentResources.EnergyGenerated;
		int energyDifference = energyGenerated - currentEnergyUsage;
		if(energyDifference < 0) {
			for(int outerIndex = 0; outerIndex < 4; outerIndex++) {
				List<Building> buildingList = buildings[outerIndex];
				for(int innerIndex = 0; innerIndex < buildingList.Count; innerIndex++) {
					Building building = buildingList[innerIndex];
					if(energyDifference < 0 && building.IsActive) {
						building.Deactivate();
						energyDifference += building.GetComponent<BuildingProperties>().EnergyCost;
					}
				}
			}

			if(energyDifference < 0) {
				List<Building> fuelAndMetal = new List<Building>(buildings[4]);

				for(int i = 0, insertIndex = (fuelAndMetal.Count == 0) ? 0 : 1; i < buildings[5].Count; i++) {
					fuelAndMetal.Insert(insertIndex, buildings[5][i]);
					insertIndex += (insertIndex == fuelAndMetal.Count - 1) ? 1 : 2;
				}

				int lastFuelIndex;
				int lastMetalIndex;
				int fuelCount = buildings[4].Count;
				int metalCount = buildings[5].Count;
				if(fuelCount > metalCount) {
					lastMetalIndex = (2 * metalCount) - 1;
					lastFuelIndex = lastMetalIndex + (fuelCount - metalCount);
				}
				else {
					lastFuelIndex = (2 * fuelCount - 2);
					lastMetalIndex = lastFuelIndex + (metalCount - fuelCount +1);
				}

				for(int i = 0; i < fuelAndMetal.Count; i++) {
					if(i == lastFuelIndex || i == lastMetalIndex) {
						continue;
					}

					Building building = fuelAndMetal[i];
					if(energyDifference < 0 && building.IsActive) {
						building.Deactivate();
						energyDifference += building.GetComponent<BuildingProperties>().EnergyCost;
					}
				}
			}
		}

		if(energyDifference > 0) {
			for(int outerIndex = 0; outerIndex < 4; outerIndex++) {
				List<Building> buildingList = buildings[outerIndex];
				for(int innerIndex = 0; innerIndex < buildingList.Count; innerIndex++) {
					Building building = buildingList[innerIndex];
					if(energyDifference > 0) {
						int energyCost = building.GetComponent<BuildingProperties>().EnergyCost;
						if(!building.IsActive && energyDifference - energyCost >= 0) {
							building.Activate();
							energyDifference -= energyCost;
						}
					}
				}
			}

			if(energyDifference > 0) {
				List<Building> fuelAndMetal = new List<Building>(buildings[4]);
				for(int i = 0, insertIndex = (fuelAndMetal.Count == 0) ? 0 : 1; i < buildings[5].Count; i++) {
					fuelAndMetal.Insert(insertIndex, buildings[5][i]);
					insertIndex += (insertIndex == fuelAndMetal.Count - 1) ? 1 : 2;
				}

				foreach(Building building in fuelAndMetal) {
					if(energyDifference > 0) {
						int energyCost = building.GetComponent<BuildingProperties>().EnergyCost;
						if(!building.IsActive && energyDifference - energyCost >= 0) {
							building.Activate();
							energyDifference -= energyCost;
						}
					}
				}
			}
		}
	}
}
