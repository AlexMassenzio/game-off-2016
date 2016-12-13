using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour {

	public Dictionary<string, bool> inputActivity;

	void Start()
	{
		inputActivity = new Dictionary<string, bool>();
		inputActivity.Add("Action", false);
		inputActivity.Add("Restart", false);
		inputActivity.Add("Pause", false);
	}

	public bool GetInput(string input)
	{
		if(inputActivity.ContainsKey(input))
		{
			return Input.GetButtonDown(input) || inputActivity[input] == true;
		}

		Debug.Log("InputManager does not recognize: " + input);
		return false;
	}

	public void ReportButtonDown(string input)
	{
		if(inputActivity[input] == true)
		{
			inputActivity[input] = false;
		}
		else
		{
			inputActivity[input] = true;
		}
	}

	public void ReportButtonUp(string input)
	{
		inputActivity[input] = false;
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
