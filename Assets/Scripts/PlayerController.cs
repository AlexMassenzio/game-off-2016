using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	private Rigidbody2D rbody;
	private Vector2 lastPosition;

	// Use this for initialization
	void Start()
	{
		rbody = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		if (Input.GetAxis("Action") > 0f && transform.parent != null)
		{
			Vector2 detachedVelocity = ((Vector2)transform.position - lastPosition) * (1 / Time.fixedDeltaTime);
			transform.parent = null;
			rbody.isKinematic = false;
			rbody.velocity = detachedVelocity;
		}
		lastPosition = transform.position;
	}

	public void OnTriggerEnter2D(Collider2D collision)
	{
		rbody.velocity = new Vector2(0f, 0f);
		rbody.isKinematic = true;
		transform.parent = collision.gameObject.transform;
	}
}
