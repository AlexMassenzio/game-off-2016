using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeController : MonoBehaviour
{

	[SerializeField]
	private float rotationSpeed;

	// Update is called once per frame
	void FixedUpdate()
	{
		transform.Rotate(Vector3.forward * rotationSpeed);
	}
}
