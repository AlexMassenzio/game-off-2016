using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeController : MonoBehaviour
{

	[SerializeField]
	private float rotationSpeed;

	private SpriteRenderer spRend;

	void Start()
	{
		spRend = GetComponent<SpriteRenderer>();
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		transform.Rotate(Vector3.forward * rotationSpeed);
	}

	public void OnTriggerEnter2D(Collider2D collision)
	{
		StartCoroutine(Blink());
	}

	IEnumerator Blink()
	{
		float dampingColor = 0.8f;
		float progress = 0;
		float r = spRend.color.r;
		float g = spRend.color.g;
		float b = spRend.color.b;
		while (progress < 1)
		{
			spRend.color = new Color(LeanTween.linear(dampingColor - r + r, r, progress), LeanTween.linear(dampingColor - r + g, g, progress), LeanTween.linear(dampingColor - r + b, b, progress));
			progress += Time.deltaTime;
			yield return null;
		}
	}
}
