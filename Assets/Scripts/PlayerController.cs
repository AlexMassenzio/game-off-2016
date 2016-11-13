using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	private Rigidbody2D rbody;
	private Vector2 lastPosition;
	private bool playStart;

	private float gravScale = 1f;

	// Use this for initialization
	void Start()
	{
		rbody = GetComponent<Rigidbody2D>();
		rbody.gravityScale = 0f;
		playStart = false;
	}

	void FixedUpdate()
	{
		if (Input.GetAxis("Action") > 0f && !playStart)
		{
			rbody.gravityScale = gravScale;
		}

		if (Input.GetAxis("Action") > 0f && transform.parent != null)
		{
			Vector2 detachedVelocity = ((Vector2)transform.position - lastPosition) * (1 / Time.fixedDeltaTime);
			transform.parent = null;
			rbody.isKinematic = false;
			rbody.velocity = detachedVelocity;
			Camera.main.GetComponent<CameraFollow>().SetFocus(transform);
		}
		lastPosition = transform.position;
	}

	public void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.gameObject.tag == "Node")
		{
			StickToObject(collision.gameObject);
		}
	}

	private void StickToObject(GameObject o)
	{
		rbody.velocity = new Vector2(0f, 0f);
		rbody.isKinematic = true;
		transform.parent = o.gameObject.transform;
		Camera.main.GetComponent<CameraFollow>().SetFocus(o.transform);
	}
}
