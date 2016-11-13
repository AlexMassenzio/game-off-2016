using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	[SerializeField]
	private GameObject player;

	private Transform focus;

	// Use this for initialization
	void Start ()
	{
		focus = player.transform;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		transform.position += (focus.position - transform.position) * 0.1f;
		Vector3 temp = transform.position;
		temp.z = -10f;
		transform.position = temp;
	}
	
	public void SetFocus(Transform t)
	{
		focus = t;
	}
}
