﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
	private Rigidbody2D rbody;
	private Vector2 lastPosition;

	private bool pressed;
	private bool dead;

		private KeyCode[] numKeys = {
		 KeyCode.Alpha1,
		 KeyCode.Alpha2,
		 KeyCode.Alpha3,
		 KeyCode.Alpha4,
		 KeyCode.Alpha5,
		 KeyCode.Alpha6,
		 KeyCode.Alpha7,
		 KeyCode.Alpha8,
		 KeyCode.Alpha9,
	 };

	[SerializeField]
	private AudioClip stick, unstick;

	// Use this for initialization
	void Start()
	{
		rbody = GetComponent<Rigidbody2D>();
		rbody.gravityScale = 0f;
		dead = false;
		pressed = false;
	}

	void Update()
	{
		for (int i = 0; i < numKeys.Length; i++)
		{
			if (Input.GetKeyDown(numKeys[i]))
			{
				int numberPressed = i + 1;
				SceneManager.LoadScene(numberPressed);
			}
		}

		if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
		}

		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
		}

	}

	void FixedUpdate()
	{

		if (Input.GetAxis("Action") > 0f)
		{
			if (!pressed && transform.parent != null)
			{
				Debug.Log("Releasing");
				Vector2 detachedVelocity = ((Vector2)transform.position - lastPosition) * (1 / Time.fixedDeltaTime);
				transform.parent = null;
				rbody.isKinematic = false;
				rbody.velocity = detachedVelocity;
				Camera.main.GetComponent<CameraFollow>().SetFocus(transform);
				GetComponent<AudioSource>().clip = unstick;
				GetComponent<AudioSource>().Play();
			}
			pressed = true;
		}
		lastPosition = transform.position;

		if (Input.GetAxis("Action") == 0f)
		{
			pressed = false;
		}
	}

	public void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.gameObject.tag == "Node")
		{
			StickToObject(collision.gameObject);
		}
		if(collision.gameObject.tag == "Kill")
		{
			dead = true;
		}
	}

	public bool IsDead()
	{
		return dead;
	}

	private void StickToObject(GameObject o)
	{
		rbody.velocity = new Vector2(0f, 0f);
		rbody.isKinematic = true;
		transform.parent = o.gameObject.transform;
		Camera.main.GetComponent<CameraFollow>().SetFocus(o.transform);
		GetComponent<AudioSource>().clip = stick;
		GetComponent<AudioSource>().Play();
	}
}
