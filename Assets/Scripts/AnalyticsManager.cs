using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

public class AnalyticsManager : MonoBehaviour {

	private static AnalyticsManager instance = null;
	public static AnalyticsManager Instance
	{
		get { return instance; }
	}

	private int levelNumber;

	void Awake()
	{
		if (instance != null && instance != this)
		{
			Destroy(this.gameObject);
			return;
		}
		else
		{
			instance = this;
		}

		DontDestroyOnLoad(this.gameObject);
	}

	void Start()
	{
		levelNumber = SceneManager.GetActiveScene().buildIndex;
		ReportLevelStart(levelNumber);
	}

	// Update is called once per frame
	void Update () {
		if(levelNumber != SceneManager.GetActiveScene().buildIndex)
		{
			levelNumber = SceneManager.GetActiveScene().buildIndex;
			ReportLevelStart(levelNumber);
		}
	}

	private void ReportLevelStart(int l)
	{
		Debug.Log(Analytics.CustomEvent("levelStart", new Dictionary<string, object>
		{
			{"levelNumber", l}
		}));
		Debug.Log("Sent Analytics data: Entered Level: " + levelNumber);
	}
}
