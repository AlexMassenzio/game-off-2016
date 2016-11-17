using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	[SerializeField]
	private GameObject player;
	private Transform focus;

	private const float ZOOM_SPEED = 2;
	private float orthoDefaultSize;
	private float zoomMargin = 25;

	// Use this for initialization
	void Start ()
	{
		focus = player.transform;
		orthoDefaultSize = Camera.main.orthographicSize;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		transform.position += (focus.position - transform.position) * 0.1f;
		Vector3 temp = transform.position;
		temp.z = -10f;
		transform.position = temp;
	}
	
	public void SetFocus(Transform t)
	{
		focus = t;
	}

	public void ZoomToWidth(float width)
	{
		if (orthoDefaultSize < (float)((width + zoomMargin) * Screen.height / Screen.width * 0.5))
		{
			StopCoroutine("ResetZoomHelper");
			StartCoroutine(ZoomToWidthHelper(width));
		}
	}

	IEnumerator ZoomToWidthHelper(float width)
	{
		float progress = 0;
		while (progress < 1)
		{
			Camera.main.orthographicSize = LeanTween.easeOutBack(orthoDefaultSize, (float)((width + zoomMargin) * Screen.height / Screen.width * 0.5), progress);
			progress += Time.deltaTime * ZOOM_SPEED;
			yield return null;
		}
	}

	public void ResetZoom()
	{
		StopCoroutine("ZoomToWidthHelper");
		StartCoroutine(ResetZoomHelper());
	}

	IEnumerator ResetZoomHelper()
	{
		float startingSize = Camera.main.orthographicSize;
		float progress = 0;
		while (progress < 1)
		{
			Camera.main.orthographicSize = LeanTween.easeOutBack(startingSize, Camera.main.orthographicSize = orthoDefaultSize, progress);
			progress += Time.deltaTime * ZOOM_SPEED;
			yield return null;
		}
	}
}
