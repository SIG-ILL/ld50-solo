using UnityEngine;

public class BuildingProperties : MonoBehaviour {
	[SerializeField]
	private int energyCost = 0;
	public int EnergyCost { get { return energyCost; } }

	[SerializeField]
	private int fuelCost = 0;
	public int FuelCost { get { return fuelCost; } }

	[SerializeField]
	private int metalCost = 0;
	public int MetalCost { get { return metalCost; } }

	public ResourcesData ResourcesCost { get { return new ResourcesData { EnergyBalance = energyCost, Fuel = fuelCost, Metal = metalCost }; } }

	[SerializeField]
	private int energyGain = 0;
	public int EnergyGain { get { return energyGain; } }

	[SerializeField]
	private int fuelGain = 0;
	public int FuelGain { get { return fuelGain; } }

	[SerializeField]
	private int metalGain = 0;
	public int MetalGain { get { return metalGain; } }

	[SerializeField]
	float resourceGenerationIntervalInSeconds = 2.0f;
	public float ResourceGenerationIntervalInSeconds { get { return resourceGenerationIntervalInSeconds; } }

	public ResourcesData ResourcesGain { get { return new ResourcesData { EnergyBalance = energyGain, Fuel = fuelGain, Metal = metalGain }; } }

	[SerializeField]
	bool canBeBuildOnPlanet = true;
	public bool CanBeBuildOnPlanet { get { return canBeBuildOnPlanet; } }

	[SerializeField]
	bool canBeBuildInSpace = false;
	public bool CanBeBuildInSpace { get { return canBeBuildInSpace; } }

	[SerializeField]
	bool canBeBuildOnMoon = false;
	public bool CanBeBuildOnMoon { get { return canBeBuildOnMoon; } }
}
