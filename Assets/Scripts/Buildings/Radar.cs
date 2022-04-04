using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class Radar : Building {
	private LineRenderer lineRenderer;
	private Transform cometMoon;
	private Coroutine lineRendererCoroutine;

	public override void Activate() {
		base.Activate();

		lineRenderer = GetComponent<LineRenderer>();
		cometMoon = FindObjectOfType<CometMovement>().transform;

		lineRendererCoroutine = StartCoroutine(LineRendererCoroutine());
	}

	public override void Deactivate() {
		base.Deactivate();

		StopCoroutine(lineRendererCoroutine);
		lineRenderer.positionCount = 0;
	}

	private IEnumerator LineRendererCoroutine() {
		while(true) {
			lineRenderer.positionCount++;
			lineRenderer.SetPosition(lineRenderer.positionCount - 1, cometMoon.position);
			yield return new WaitForSeconds(0.05f);
		}
	}
}
