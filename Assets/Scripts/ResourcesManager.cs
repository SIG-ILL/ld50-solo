using System;
using UnityEngine;

public class ResourcesManager : MonoBehaviour {
	[SerializeField]
	private int initialEnergy = 10;
	[SerializeField]
	private int initialFuel = 0;
	[SerializeField]
	private int initialMetal = 10;

	public ResourcesData CurrentResources { get { return new ResourcesData { Energy = resources.Energy, Fuel = resources.Fuel, Metal = resources.Metal }; } }

	private ResourcesData resources;

	private event Action<ResourcesData> resourcesChangedEvent;
	public event Action<ResourcesData> ResourcesChanged {
		add { resourcesChangedEvent += value; }
		remove { resourcesChangedEvent -= value; }
	}

	private void Awake() {
		resources = new ResourcesData { Energy = initialEnergy, Fuel = initialFuel, Metal = initialMetal };
		if(resourcesChangedEvent != null) {
			resourcesChangedEvent(CurrentResources);
		}
	}

	public bool Subtract(ResourcesData resourcesChange) {
		if(resourcesChange.Fuel > this.resources.Fuel || resourcesChange.Metal > this.resources.Metal) {
			return false;
		}

		resources.Energy -= resourcesChange.Energy;
		resources.Fuel -= resourcesChange.Fuel;
		resources.Metal -= resourcesChange.Metal;

		if(resourcesChangedEvent != null) {
			resourcesChangedEvent(resources);
		}

		return true;
	}

	public void AddNewBuilding(GameObject buildingObject) {
		Building building = buildingObject.GetComponent<Building>();
		building.IncomeGenerated += OnIncomeGenerated;
		resources.Energy += building.GetComponent<BuildingProperties>().EnergyGain;

		if(building.GetComponent<BuildingProperties>().EnergyGain > 0 && resourcesChangedEvent != null) {
			resourcesChangedEvent(resources);
		}

		building.Activate();
	}

	private void OnIncomeGenerated(ResourcesData income) {
		resources.Fuel += income.Fuel;
		resources.Metal += income.Metal;

		if(resourcesChangedEvent != null) {
			resourcesChangedEvent(resources);
		}
	}
}
