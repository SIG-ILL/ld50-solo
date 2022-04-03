using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class ConstructionManager : MonoBehaviour {
	[SerializeField]
	private Sprite unableToBuildCrossSprite;

	[SerializeField]
	private ResourcesManager resourcesManager;
	[SerializeField]
	private Planet planet;

	[SerializeField]
	private int requiredRocketParts = 10;
	[SerializeField]
	private int rocketPartMetalCost = 10;
	[SerializeField]
	private int rocketPartFuelCost = 10;

	public ResourcesData RocketPartCost { get { return new ResourcesData { Energy = 0, Fuel = rocketPartFuelCost, Metal = rocketPartMetalCost }; } }

	private event Action<float> rocketPartBuiltEvent;
	public event Action<float> RocketPartBuilt {
		add { rocketPartBuiltEvent += value; }
		remove { rocketPartBuiltEvent -= value; }
	}

	private event Action rocketCompletedEvent;
	public event Action RocketCompleted {
		add { rocketCompletedEvent += value; }
		remove { rocketCompletedEvent -= value; }
	}

	private event Action radarBuiltEvent;
	public event Action RadarBuilt {
		add { radarBuiltEvent += value; }
		remove { radarBuiltEvent -= value; }
	}

	private GameObject constructionGhost;
	private GameObject unableToBuildCross;

	private int rocketPartsBuilt;

	private void Awake() {
		rocketPartsBuilt = 0;

		resourcesManager.ResourcesChanged += OnResourcesChanged;
	}	

	private void Update() {
		UpdateConstructionGhost();
	}

	public void OnClick() {
		if(constructionGhost == null) {
			return;
		}

		Vector2 cursorWorldPoint = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
		Build(cursorWorldPoint);
	}

	public void OnRightClick() {
		if(constructionGhost != null) {
			Destroy(constructionGhost);
			constructionGhost = null;
		}
	}

	public void BuildBuilding(GameObject buildingPrefab) {
		CreateConstructionGhost(buildingPrefab);
	}

	public void BuildRocketPart() {
		if(!CheckHasSufficientResourcesToBuildRocketPart()) {
			return;
		}

		resourcesManager.Subtract(new ResourcesData { Energy = 0, Fuel = rocketPartFuelCost, Metal = rocketPartMetalCost });

		rocketPartsBuilt++;

		if(rocketPartBuiltEvent != null) {
			rocketPartBuiltEvent(rocketPartsBuilt / (float)requiredRocketParts);
		}

		if(rocketPartsBuilt == requiredRocketParts) {
			if(rocketCompletedEvent != null) {
				rocketCompletedEvent();
			}
		}
	}

	private bool CheckHasSufficientResourcesToBuildRocketPart() {
		ResourcesData resources = resourcesManager.CurrentResources;
		if(resources.Fuel > rocketPartFuelCost && resources.Metal > rocketPartMetalCost) {
			return true;
		}

		return false;
	}

	private void OnResourcesChanged(ResourcesData resources) {
		ApplyGhostBuildabilityIndication(resources);
	}

	private void UpdateConstructionGhost() {
		if(constructionGhost == null) {
			return;
		}
		constructionGhost.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
		Vector2 direction = constructionGhost.transform.position - planet.transform.position;
		constructionGhost.transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg, Vector3.forward) * Quaternion.FromToRotation(Vector3.left, Vector3.up);

		ApplyGhostBuildabilityIndication(resourcesManager.CurrentResources);
	}

	private void CreateConstructionGhost(GameObject prefab) {
		OnRightClick();

		constructionGhost = Instantiate<GameObject>(prefab);
		ApplyGhostBuildabilityIndication(resourcesManager.CurrentResources);
	}

	private void Build(Vector2 position) {
		if(CheckValidBuildLocation(constructionGhost, position) && CheckHasSufficientResourcesToBuildCurrentGhost(resourcesManager.CurrentResources)) {
			resourcesManager.Subtract(constructionGhost.GetComponent<BuildingProperties>().ResourcesCost);
			constructionGhost.transform.position = position;

			if(unableToBuildCross != null) {
				Destroy(unableToBuildCross);
				unableToBuildCross = null;
			}

			GameObject newBuilding = constructionGhost;
			constructionGhost = null;
			resourcesManager.AddNewBuilding(newBuilding);

			if(newBuilding.GetComponent<Radar>() != null && radarBuiltEvent != null) {
				radarBuiltEvent();
			}
		}		
	}

	private bool CheckHasSufficientResourcesToBuildCurrentGhost(ResourcesData currentResources) {
		BuildingProperties buildingProperties = constructionGhost.GetComponent<BuildingProperties>();
		if(buildingProperties.FuelCost > currentResources.Fuel || buildingProperties.MetalCost > currentResources.Metal) {
			return false;
		}

		return true;
	}

	private bool CheckValidBuildLocation(GameObject building, Vector2 position) {
		BuildingProperties buildingProperties = building.GetComponent<BuildingProperties>();

		Collider2D[] clickTargetColliders = Physics2D.OverlapPointAll(position);

		if(clickTargetColliders.Length == 0 && !buildingProperties.CanBeBuildInSpace) {	// TODO: Additional checking is required for objects that cannot be build in space. A collider on the constructionGhost may be detected while clicking in space, in which case the 'self' collider should be ignored/filtered out.
			return false;
		}
		foreach(Collider2D collider in clickTargetColliders) {
			if(!buildingProperties.CanBeBuildOnPlanet && collider.GetComponent<Planet>() != null) {
				return false;
			}
			if(!buildingProperties.CanBeBuildOnMoon && collider.GetComponent<CometMovement>() != null) {
				return false;
			}			
		}

		return true;
	}

	private void ApplyGhostBuildabilityIndication(ResourcesData resources) {
		if(constructionGhost == null) {
			return;
		}

		if(CheckHasSufficientResourcesToBuildCurrentGhost(resources) && CheckValidBuildLocation(constructionGhost, constructionGhost.transform.position)) {
			if(unableToBuildCross == null) {
				return;
			}

			Destroy(unableToBuildCross);
			unableToBuildCross = null;
		}
		else if(unableToBuildCross == null) {
			unableToBuildCross = new GameObject("UnableToBuildCross");
			SpriteRenderer crossSpriteRenderer = unableToBuildCross.AddComponent<SpriteRenderer>();
			crossSpriteRenderer.sprite = unableToBuildCrossSprite;
			crossSpriteRenderer.sortingOrder = 5;
			unableToBuildCross.transform.parent = constructionGhost.transform;
			unableToBuildCross.transform.localPosition = Vector3.zero;
			unableToBuildCross.transform.localRotation = Quaternion.identity;
		}
	}
}
