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
	private float timer;

	//General Vars
	[SerializeField]
	private GameObject player, goal, completeText, deadText;

	//Fadeout/Fadein Vars
	[SerializeField]
	private Image whiteout;
	private const float FADE_TIME_SCALE = 0.5f;

	//Intro Vars
	private const float WIDESCREEN_HEIGHT = 83f;
	private float textStartPos;
	private bool introComplete;
	[SerializeField]
	private GameObject WidescreenBottom, WidescreenTop, levelText;

	// Use this for initialization
	void Start () {
		currentState = LevelState.Preview;
		previewTrans = new GameObject();
		firstFrameInState = true;
		pressed = false;
		timer = 0;
		introComplete = false;
		textStartPos = levelText.transform.position.x;
		levelText.GetComponent<Text>().text = "Level " + (SceneManager.GetActiveScene().buildIndex);
		Debug.Log(levelText.GetComponent<Text>().text);
	}
	
	// Update is called once per frame
	void Update ()
	{
		timer += Time.deltaTime;

		switch (currentState) {
			case LevelState.Preview:
				if (firstFrameInState)
				{
					timer = 0;

					previewTrans.transform.position = (Vector2)((goal.transform.position - player.transform.position) * 0.5f + player.transform.position);
					Camera.main.GetComponent<CameraFollow> ().SetFocus (previewTrans.transform);
					LeanTween.moveY(WidescreenTop, WidescreenTop.transform.position.y - WIDESCREEN_HEIGHT, 0.75f).setEase(LeanTweenType.easeOutCubic).delay = 0.5f;
					LeanTween.moveY(WidescreenBottom, WidescreenBottom.transform.position.y + WIDESCREEN_HEIGHT, 0.75f).setEase(LeanTweenType.easeOutCubic).delay = 0.5f;
					LeanTween.moveX(levelText, levelText.transform.parent.gameObject.transform.position.x, 0.5f).setEase(LeanTweenType.easeOutBack).delay = 1f;

					firstFrameInState = false;
				}

				if (Input.GetAxis("Action") > 0f)
				{
					if (!pressed)
					{
						if (!introComplete)
						{
							StartCoroutine(IntroFinish(3));
						}
						else
						{
							currentState = LevelState.Playing;
							player.GetComponent<Rigidbody2D>().gravityScale = 1;
							firstFrameInState = true;
						}
					}
					pressed = true;
				}

				if(timer > 3f && !introComplete)
				{
					StartCoroutine(IntroFinish(1));
				}
				break;

			case LevelState.Playing:
				if(firstFrameInState)
				{
					Camera.main.GetComponent<CameraFollow>().SetFocus(player.transform);
					firstFrameInState = false;
				}

				if(player.GetComponent<PlayerController>().IsDead())
				{
					currentState = LevelState.Death;
					firstFrameInState = true;
				}

				if(goal.GetComponent<GoalController>().IsGoalReached())
				{
					currentState = LevelState.Complete;
					firstFrameInState = true;
				}
				break;

			case LevelState.Death:
				if (firstFrameInState)
				{
					Debug.Log("Lost the level");
					firstFrameInState = false;
					LeanTween.scale(deadText, new Vector3(1, 1, 1), 0.75f).setEase(LeanTweenType.easeOutBack);
				}

				if (Input.GetAxis("Action") > 0f)
				{
					if (!pressed)
					{
						SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
					}
					pressed = true;
				}
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
					}
					pressed = true;
				}
				break;
		};

		if (Input.GetAxis("Action") == 0f)
		{
			pressed = false;
		}
	}

	IEnumerator IntroFinish(float timeScale)
	{
		introComplete = true;
		while(LeanTween.isTweening(levelText))
		{
			yield return null;
		}
		LeanTween.moveY(WidescreenTop, WidescreenTop.transform.position.y + WIDESCREEN_HEIGHT, 0.75f * (1/timeScale)).setEase(LeanTweenType.easeOutCubic);
		LeanTween.moveY(WidescreenBottom, WidescreenBottom.transform.position.y - WIDESCREEN_HEIGHT, 0.75f * (1 / timeScale)).setEase(LeanTweenType.easeOutCubic);
		float textStartPos = levelText.transform.position.x;
		LeanTween.moveX(levelText, levelText.transform.parent.gameObject.transform.position.x + textStartPos * 2, 0.5f * (1 / timeScale)).setEase(LeanTweenType.easeInBack);
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
