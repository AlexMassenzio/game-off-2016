using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour {

	private enum LevelState {Preview, Playing, Death, Complete};
	private LevelState currentState;
	private GameObject previewTrans;
	private bool firstFrameInState;
	private bool pressed;

	[SerializeField]
	private GameObject player, goal;

	// Use this for initialization
	void Start () {
		currentState = LevelState.Preview;
		previewTrans = new GameObject();
		firstFrameInState = true;
		pressed = false;
	}
	
	// Update is called once per frame
	void Update ()
	{

		switch (currentState) {
			case LevelState.Preview:
				if (firstFrameInState)
				{
					previewTrans.transform.position = (Vector2)((goal.transform.position - player.transform.position) * 0.5f + player.transform.position);
					Camera.main.GetComponent<CameraFollow> ().SetFocus (previewTrans.transform);
					firstFrameInState = false;
				}

				if (Input.GetAxis("Action") > 0f)
				{
					if (!pressed)
					{
						currentState = LevelState.Playing;
						pressed = true;
						firstFrameInState = true;
					}
				}
				break;

			case LevelState.Playing:
				if(firstFrameInState)
				{
					Camera.main.GetComponent<CameraFollow>().SetFocus(player.transform);
					firstFrameInState = false;
				}

				if(goal.GetComponent<GoalController>().IsGoalReached())
				{
					currentState = LevelState.Complete;
					firstFrameInState = true;
				}
				break;

			case LevelState.Death:
				break;

			case LevelState.Complete:
				if(firstFrameInState)
				{
					Debug.Log("Completed the level.");
					firstFrameInState = false;
				}
				break;
		};

		if (Input.GetAxis("Action") == 0f)
		{
			pressed = false;
		}
	}
}
