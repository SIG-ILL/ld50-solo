using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(BuildingProperties))]
public class Building : MonoBehaviour {
	private event Action<ResourcesData> incomeGeneratedEvent;
	public event Action<ResourcesData> IncomeGenerated {
		add { incomeGeneratedEvent += value; }
		remove { incomeGeneratedEvent -= value; }
	}

	public virtual void Activate() {
		StartCoroutine(IncomeCoroutine());
	}

	private IEnumerator IncomeCoroutine() {
		BuildingProperties buildingProperties = GetComponent<BuildingProperties>();

		while(true) {
			yield return new WaitForSeconds(buildingProperties.ResourceGenerationIntervalInSeconds);

			if(incomeGeneratedEvent != null) {
				incomeGeneratedEvent(buildingProperties.ResourcesGain);
			}
		}
	}
}