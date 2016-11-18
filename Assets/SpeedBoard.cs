using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoard : MonoBehaviour
{
	private float timer;

	public void OnCollisionEnter2D(Collision2D collision)
	{
		timer = 0f;
	}

	public void OnCollisionStay2D(Collision2D collision)
	{
		timer += Time.deltaTime;
		if (collision.gameObject.tag == "Player" && timer > 0.1f)
		{
			Debug.Log("Got it");
			collision.gameObject.GetComponent<Rigidbody2D>().AddForce(collision.gameObject.GetComponent<Rigidbody2D>().velocity.normalized * 250f);
			timer -= 0.1f;
		}
	}

	public void OnCollisionExit2D(Collision2D collision)
	{

	}
}
