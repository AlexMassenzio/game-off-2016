using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalController : MonoBehaviour {

	private bool goalReached;

	// Use this for initialization
	void Start () {
		goalReached = false;
	}

	public void OnTriggerEnter2D (Collider2D collision)
	{
		if(collision.gameObject.tag == "Player")
		{
			goalReached = true;
		}
	}

	public bool IsGoalReached()
	{
		return goalReached;
	}
}
