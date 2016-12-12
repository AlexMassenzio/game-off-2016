using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelController : MonoBehaviour {

	//State machine variables
	public enum LevelState {Preview, Playing, Death, Complete};
	private LevelState currentState;
	private bool firstFrameInState;
	public LevelState CurrentState
	{
		get { return currentState; }
		set
		{
			currentState = value;
			firstFrameInState = true;
		}
	}


	//General variables
	[SerializeField]
	private GameObject guiPrefab;
	private GameObject myGUI;

	[SerializeField]
	private GameObject player, goal;
	private GameObject	completeText,
						deadText,
						WidescreenTop,
						WidescreenBottom,
						levelText,
						mobileInputGUI;

	//Fadeout/Fadein variables
	private Image whiteout;
	private const float FADE_TIME_SCALE = 0.5f;

	//Intro variables
	private const float WIDESCREEN_HEIGHT = 83f;
	private float textStartPos;
	private float textEndPos;
	private bool introComplete;

	//Misc variables
	private GameObject previewTrans;
	private float timer;

	void Start () {
		CurrentState = LevelState.Preview;
		previewTrans = new GameObject();
		timer = 0;

		//Instantiate GUI and set GUI variables for ease of access
		myGUI = Instantiate(guiPrefab);
		completeText = myGUI.transform.GetChild(0).gameObject;
		deadText = myGUI.transform.GetChild(1).gameObject;
		whiteout = myGUI.transform.GetChild(2).gameObject.GetComponent<Image>();
		WidescreenTop = myGUI.transform.GetChild(3).gameObject;
		WidescreenBottom = myGUI.transform.GetChild(4).gameObject;
		levelText = myGUI.transform.GetChild(5).gameObject;
		mobileInputGUI = myGUI.transform.GetChild(6).gameObject;

		introComplete = false;
		textStartPos = (Screen.width + levelText.transform.parent.transform.position.x) * -1;
		textEndPos = (Screen.width + levelText.transform.parent.transform.position.x);
		levelText.transform.position = new Vector2(textStartPos, levelText.transform.position.y);
		levelText.GetComponent<Text>().text = "Level " + (SceneManager.GetActiveScene().buildIndex);

		Debug.Log(levelText.GetComponent<Text>().text);
	}
	
	void Update ()
	{
		timer += Time.deltaTime;

		//Debug
		//Debug.Log(player.GetComponent<Rigidbody2D>().angularVelocity.ToString());

		switch (CurrentState) {
			case LevelState.Preview:
				if (firstFrameInState)
				{
					timer = 0;

					previewTrans.transform.position = (Vector2)((goal.transform.position - player.transform.position) * 0.5f + player.transform.position);

					Camera.main.GetComponent<CameraFollow> ().SetFocus (previewTrans.transform);
					Camera.main.GetComponent<CameraFollow>().ZoomToWidth(Vector2.Distance(player.transform.position, goal.transform.position));

					LeanTween.moveY(WidescreenTop, WidescreenTop.transform.position.y - WIDESCREEN_HEIGHT, 0.75f).setEase(LeanTweenType.easeOutCubic).delay = 0.5f;
					LeanTween.moveY(WidescreenBottom, WidescreenBottom.transform.position.y + WIDESCREEN_HEIGHT, 0.75f).setEase(LeanTweenType.easeOutCubic).delay = 0.5f;
					LeanTween.moveX(levelText, levelText.transform.parent.gameObject.transform.position.x, 0.5f).setEase(LeanTweenType.easeOutBack).delay = 1f;

					firstFrameInState = false;
				}

				if (Input.GetButtonDown("Action"))
				{
						if(introComplete || levelText.transform.position.x == textStartPos)
						{
							CurrentState = LevelState.Playing;
							player.GetComponent<Rigidbody2D>().gravityScale = 1;
						}
						if (!introComplete)
						{
							IntroFinish(3, true);
						}
				}

				if(timer > 3f && !introComplete)
				{
					IntroFinish(1, false);
				}
				break;

			case LevelState.Playing:
				if(firstFrameInState)
				{
					Camera.main.GetComponent<CameraFollow>().SetFocus(player.transform);
					Camera.main.GetComponent<CameraFollow>().ResetZoom();
					firstFrameInState = false;
				}

				if(player.GetComponent<PlayerController>().IsDead())
				{
					CurrentState = LevelState.Death;
				}

				if(goal.GetComponent<GoalController>().IsGoalReached())
				{
					CurrentState = LevelState.Complete;
				}
				break;

			case LevelState.Death:
				if (firstFrameInState)
				{
					Debug.Log("Lost the level");
					firstFrameInState = false;
					LeanTween.scale(deadText, new Vector3(1, 1, 1), 0.75f).setEase(LeanTweenType.easeOutBack);
				}

				if (Input.GetButtonDown("Action"))
				{
					SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
				}
				break;

			case LevelState.Complete:
				if(firstFrameInState)
				{
					Debug.Log("Completed the level.");
					firstFrameInState = false;
					LeanTween.scale(completeText, new Vector3(1,1,1), 0.75f).setEase(LeanTweenType.easeOutBack);
				}

				if (Input.GetButtonDown("Action"))
				{
					StartCoroutine(Fadeout());
				}
				break;
		};
	}

	private void IntroFinish (float timeScale, bool cancelEarly)
	{
		introComplete = true;
		if(LeanTween.isTweening(levelText))
			LeanTween.cancelAll ();
		LeanTween.moveY (WidescreenTop, WidescreenTop.transform.position.y + WIDESCREEN_HEIGHT, 0.75f * (1 / timeScale)).setEase (LeanTweenType.easeOutCubic);
		LeanTween.moveY (WidescreenBottom, WidescreenBottom.transform.position.y - WIDESCREEN_HEIGHT, 0.75f * (1 / timeScale)).setEase (LeanTweenType.easeOutCubic);
		if (cancelEarly)
		{
			Debug.Log("Ended Early!");
			LeanTween.moveX (levelText, textStartPos, 0.5f * (1 / timeScale)).setEase (LeanTweenType.easeInBack);
		}
		else
		{
			Debug.Log("Ended On Time!");
			LeanTween.moveX (levelText, textEndPos, 0.5f * (1 / timeScale)).setEase (LeanTweenType.easeInBack);
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

		int index = SceneManager.GetActiveScene().buildIndex + 1;
		if (index >= SceneManager.sceneCountInBuildSettings)
			index = SceneManager.sceneCountInBuildSettings - 1;
		SceneManager.LoadScene(index);
	}
}
