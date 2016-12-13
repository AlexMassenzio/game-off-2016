using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileInput : MonoBehaviour {

	private InputManager myIM;

	// Use this for initialization
	void Start () {
		if(Application.platform != RuntimePlatform.Android && !Application.isEditor)
		{
			this.gameObject.SetActive(false);
		}
		myIM = GameObject.FindGameObjectWithTag("InputManager").GetComponent<InputManager>();
	}
	
	public void ReportPointerDown(string input)
	{
		myIM.ReportButtonDown(input);
	}

	public void ReportPointerUp(string input)
	{
		myIM.ReportButtonUp(input);
	}
}
