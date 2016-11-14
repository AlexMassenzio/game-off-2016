using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelController : MonoBehaviour {

	private enum LevelState {Preview, Playing, Death, Complete};
	private LevelState currentState;
	private GameObject previewTrans;
	private bool firstFrameInState;
	private bool pressed;

	//General Vars
	[SerializeField]
	private GameObject player, goal, completeText;

	//Fadeout/Fadein Vars
	[SerializeField]
	private Image whiteout;
	private const float FADE_TIME_SCALE = 0.5f;

	//Intro Vars
	[SerializeField]
	private GameObject WidescreenBottom, WidescreenTop;


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
					LeanTween.moveY(WidescreenTop, 227.5f, 0.75f).setEase(LeanTweenType.easeOutCubic).delay = 2.5f;
					LeanTween.moveY(WidescreenBottom, -227.5f, 0.75f).setEase(LeanTweenType.easeOutCubic).delay = 2.5f;
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
					LeanTween.scale(completeText, new Vector3(1,1,1), 0.75f).setEase(LeanTweenType.easeOutBack);
				}

				if (Input.GetAxis("Action") > 0f)
				{
					if (!pressed)
					{
						StartCoroutine(Fadeout());
						pressed = true;
					}
				}
				break;
		};

		if (Input.GetAxis("Action") == 0f)
		{
			pressed = false;
		}
	}

	IEnumerator Fadeout ()
	{
		float progress = 0;
		Color tempColor;
		while(progress < 0.99)
		{
			tempColor = whiteout.color;
			tempColor.a = LeanTween.linear(0f, 1f, progress);
			whiteout.color = tempColor;
			progress += Time.deltaTime * FADE_TIME_SCALE;
			yield return null;
		}

		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}
}
