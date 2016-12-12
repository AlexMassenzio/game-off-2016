using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour {

	Button actionButton, resetButton;

	public void Initialize(Button a, Button r)
	{
		actionButton = a;
		resetButton = r;
	}

	public bool GetInput(string input)
	{
		switch (input)
		{
			case "Action":
				return (Input.GetButtonDown("Action") || Input.touches.Length != 0); //&& !IsPointerOverUIObject());  //(Application.platform == RuntimePlatform.Android && !IsPointerOverUIObject));
				break;
		}
		return false;
	}

	private bool IsPointerOverUIObject() 
	{
		PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
		eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		List<RaycastResult> results = new List<RaycastResult>();
		EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
		Debug.Log(results.Count); 
		return results.Count > 0;
	}
}
