using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(BuildingProperties))]
public class Building : MonoBehaviour {
	[SerializeField]
	private GameObject noPowerIndicator;

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

	public void DeactivateDueToNoPower() {
		Deactivate();
		StartCoroutine(NoPowerIndicatorCoroutine());
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

	private IEnumerator NoPowerIndicatorCoroutine() {
		while(!isActive) {
			noPowerIndicator.SetActive(true);
			yield return new WaitForSeconds(0.6f);
			noPowerIndicator.SetActive(false);
			yield return new WaitForSeconds(0.2f);
		}
	}
}