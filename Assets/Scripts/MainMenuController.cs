using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour {

	private float fadeinTimeScale = 0.5f;

	[SerializeField]
	private Image whiteout;

	void Start () {
		StartCoroutine(Fadein());
	}
	
	void Update () {
		
	}

	IEnumerator Fadein()
	{
		Debug.Log("Start Fade In");
		float progress = 0;
		Color tempColor;
		while (progress < 0.99f)
		{
			tempColor = whiteout.color;
			tempColor.a = LeanTween.linear(1f, 0f, progress);
			whiteout.color = tempColor;
			progress += Time.deltaTime * fadeinTimeScale;
			yield return null;
		}
	}
}
