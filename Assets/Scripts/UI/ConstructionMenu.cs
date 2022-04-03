using UnityEngine;
using UnityEngine.UI;

public class ConstructionMenu : MonoBehaviour {
	[SerializeField]
	private ResourcesManager resourcesManager;
	[SerializeField]
	private ConstructionManager constructionManager;

	[SerializeField]
	private Image spaceportButtonImage;
	[SerializeField]
	private Image acceleratorButtonImage;
	[SerializeField]
	private Image repulsortButtonImage;
	[SerializeField]
	private Image metalMineButtonImage;
	[SerializeField]
	private Image fuelRefineryButtonImage;
	[SerializeField]
	private Image powerPlantButtonImage;
	[SerializeField]
	private Image radarButtonImage;
	[SerializeField]
	private Image rocketPartButtonImage;

	[SerializeField]
	private Image rocketProgressBar;

	[SerializeField]
	private GameObject spaceportPrefab;
	[SerializeField]
	private GameObject acceleratorPrefab;
	[SerializeField]
	private GameObject repulsorPrefab;
	[SerializeField]
	private GameObject metalMinePrefab;
	[SerializeField]
	private GameObject fuelRefineryPrefab;
	[SerializeField]
	private GameObject powerPlantPrefab;
	[SerializeField]
	private GameObject radarPrefab;

	private bool hasBuiltRadar;

	private void Awake() {
		hasBuiltRadar = false;

		resourcesManager.ResourcesChanged += OnResourcesChanged;
		constructionManager.RocketPartBuilt += OnRocketPartBuilt;
		constructionManager.RadarBuilt += OnRadarBuilt;
	}

	private void Start() {
		UpdateButtonStates(resourcesManager.CurrentResources);
	}

	private void OnRadarBuilt() {
		hasBuiltRadar = true;
		radarButtonImage.color = Color.grey;
		radarButtonImage.GetComponent<Button>().interactable = false;
	}

	private void OnRocketPartBuilt(float progress) {
		rocketProgressBar.fillAmount = progress;
	}

	private void OnResourcesChanged(ResourcesData resources) {
		UpdateButtonStates(resources);
	}

	private void UpdateButtonStates(ResourcesData resources) {
		BuildingProperties spaceportProperties = spaceportPrefab.GetComponent<BuildingProperties>();
		BuildingProperties acceleratorProperties = acceleratorPrefab.GetComponent<BuildingProperties>();
		BuildingProperties repulsortProperties = repulsorPrefab.GetComponent<BuildingProperties>();
		BuildingProperties metalMineProperties = metalMinePrefab.GetComponent<BuildingProperties>();
		BuildingProperties fuelRefineryProperties = fuelRefineryPrefab.GetComponent<BuildingProperties>();
		BuildingProperties powerPlantProperties = powerPlantPrefab.GetComponent<BuildingProperties>();
		BuildingProperties radarProperties = radarPrefab.GetComponent<BuildingProperties>();

		SetBuildingButtonResourcesAvailableIndication(resources, spaceportProperties, spaceportButtonImage);
		SetBuildingButtonResourcesAvailableIndication(resources, acceleratorProperties, acceleratorButtonImage);
		SetBuildingButtonResourcesAvailableIndication(resources, repulsortProperties, repulsortButtonImage);
		SetBuildingButtonResourcesAvailableIndication(resources, metalMineProperties, metalMineButtonImage);
		SetBuildingButtonResourcesAvailableIndication(resources, fuelRefineryProperties, fuelRefineryButtonImage);
		SetBuildingButtonResourcesAvailableIndication(resources, powerPlantProperties, powerPlantButtonImage);
		if(!hasBuiltRadar) {
			SetBuildingButtonResourcesAvailableIndication(resources, radarProperties, radarButtonImage);
		}		

		ResourcesData rocketPartCost = constructionManager.RocketPartCost;
		if(resources.Fuel < rocketPartCost.Fuel || resources.Metal < rocketPartCost.Metal) {
			rocketPartButtonImage.color = Color.red;
			rocketPartButtonImage.GetComponent<Button>().interactable = false;
		}
		else {
			rocketPartButtonImage.color = Color.white;
			rocketPartButtonImage.GetComponent<Button>().interactable = true;
		}
	}

	private void SetBuildingButtonResourcesAvailableIndication(ResourcesData resources, BuildingProperties buildingProperties, Image button) {
		if(resources.Fuel < buildingProperties.FuelCost || resources.Metal < buildingProperties.MetalCost) {
			button.color = new Color(254 / 255.0f, 168 / 255.0f, 6 / 255.0f);
		}
		else {
			button.color = Color.white;
		}
	}

	public void OnSpaceportButtonClicked() {
		constructionManager.BuildBuilding(spaceportPrefab);
	}

	public void OnAcceleratorButtonClicked() {
		constructionManager.BuildBuilding(acceleratorPrefab);
	}

	public void OnRepulsorButtonClicked() {
		constructionManager.BuildBuilding(repulsorPrefab);
	}

	public void OnMetalMineButtonClicked() {
		constructionManager.BuildBuilding(metalMinePrefab);
	}

	public void OnFuelRefineryButtonClicked() {
		constructionManager.BuildBuilding(fuelRefineryPrefab);
	}

	public void OnPowerPlantButtonClicked() {
		constructionManager.BuildBuilding(powerPlantPrefab);
	}

	public void OnRadarButtonClicked() {
		constructionManager.BuildBuilding(radarPrefab);
	}

	public void OnBuildRocketPartButtonClicked() {
		constructionManager.BuildRocketPart();
	}
}
