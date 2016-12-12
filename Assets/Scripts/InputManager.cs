using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

	public bool GetInput(string input)
	{
		return Input.GetButtonDown(input);
	}
}
