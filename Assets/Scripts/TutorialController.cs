using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class TutorialController : MonoBehaviour {

	private enum TutorialState {Intro, Stuck, Unstuck};
	private TutorialState currentState;
	private float timer;

	//Update Vars
	[SerializeField]
	private GameObject player;
	private bool escapable = true;

	//Fadeout Vars
	private float fadeoutTimeScale = 0.5f;

	[SerializeField]
	private Text tutText;
	[SerializeField]
	private Image whiteout;

	// Use this for initialization
	void Start () {
		currentState = TutorialState.Intro;
		timer = 0f;
		Input.simulateMouseWithTouches = true;
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;

		if (Input.GetKeyDown(KeyCode.Escape) && escapable)
		{
			escapable = false;
			StartCoroutine(Fadeout());
		}

		switch (currentState)
		{
			case TutorialState.Intro:
				if (timer > 1f && timer < 3f)
				{
					tutText.text = "Welcome to Tangent!";
				}
				else if(timer >= 3f && timer < 6f)
				{
					tutText.text = "Press space to release your ball.";
				}

				if (Input.GetButtonDown("Action") || Input.GetMouseButtonDown(0))
				{
					player.GetComponent<Rigidbody2D>().gravityScale = 1;
					currentState++;
					timer = 0f;
				}
				break;

			case TutorialState.Stuck:
				if (timer > 1.5f && timer < 4f)
				{
					tutText.text = "Good Job!";
				}
				else if (timer >= 4f && timer < 7f)
				{
					tutText.text = "As you can see, landing on a wheel sticks you to it.";
				}
				else if (timer >= 7f)
				{
					tutText.text = "Press space to un-stick yourself.";
				}

				if (Input.GetButtonDown("Action"))
				{
						currentState++;
						timer = 0f;
				}
				break;

			case TutorialState.Unstuck:
				if (timer > 1.5f && timer < 4f)
				{
					tutText.text = "You have completed the tutorial! Press space";
					Camera.main.GetComponent<CameraFollow>().SetFocus(tutText.transform);
				}

				if (Input.GetAxis("Action") > 0f && timer > 1.5f && escapable)
				{
					escapable = false;
					StartCoroutine(Fadeout());
				}
					break;
		};
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
			progress += Time.deltaTime * fadeoutTimeScale;
			yield return null;
		}

		SceneManager.LoadScene(1);
	}
}
