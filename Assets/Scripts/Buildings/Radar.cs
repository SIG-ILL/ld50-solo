using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class Radar : Building {
	private LineRenderer lineRenderer;

	private Transform cometMoon;

	public override void Activate() {
		base.Activate();

		lineRenderer = GetComponent<LineRenderer>();
		cometMoon = FindObjectOfType<CometMovement>().transform;

		StartCoroutine(LineRendererCoroutine());
	}

	private IEnumerator LineRendererCoroutine() {
		while(true) {
			lineRenderer.positionCount++;
			lineRenderer.SetPosition(lineRenderer.positionCount - 1, cometMoon.position);
			yield return new WaitForSeconds(0.05f);
		}
	}
}
