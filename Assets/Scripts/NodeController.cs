using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeController : MonoBehaviour
{

	[SerializeField]
	private float rotationSpeed;

	private SpriteRenderer spRend;
	private Color originalColor;

	private Coroutine currentBlink;

	void Start()
	{
		spRend = GetComponent<SpriteRenderer>();
		originalColor = spRend.color;
		currentBlink = null;
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		transform.Rotate(Vector3.forward * rotationSpeed);
	}

	public void OnTriggerEnter2D(Collider2D collision)
	{
		if (currentBlink != null)
		{
			StopCoroutine(currentBlink);
		}
		currentBlink = StartCoroutine(Blink());
	}

	IEnumerator Blink()
	{
		float dampingColor = 0.8f;
		float progress = 0;
		float r = originalColor.r;
		float g = originalColor.g;
		float b = originalColor.b;
		while (progress < 1)
		{
			spRend.color = new Color(LeanTween.linear(dampingColor - r + r, r, progress), LeanTween.linear(dampingColor - r + g, g, progress), LeanTween.linear(dampingColor - r + b, b, progress));
			progress += Time.deltaTime;
			yield return null;
		}
	}
}
