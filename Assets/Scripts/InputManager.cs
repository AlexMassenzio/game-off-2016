using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

	public enum InputType {Action, UI, Empty};

	public InputType getInputType()
	{
		foreach(Touch touches in Input.touches)
		{

		}
	}
}
