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

	private Coroutine incomeCoroutine;
	private bool isActive;
	public bool IsActive { get { return isActive; } }

	public virtual void Activate() {
		incomeCoroutine = StartCoroutine(IncomeCoroutine());
		isActive = true;
	}

	public virtual void Deactivate() {
		StopCoroutine(incomeCoroutine);
		isActive = false;
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