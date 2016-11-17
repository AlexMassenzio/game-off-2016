using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Very useful link: http://answers.unity3d.com/questions/11314/audio-or-music-to-continue-playing-between-scene-c.html

public class BGM : MonoBehaviour {

	private static BGM instance = null;
	public static BGM Instance
	{
		get { return instance; }
	}

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
}
