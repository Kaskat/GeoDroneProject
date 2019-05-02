using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour 
{
	public float speedFollowCamera = 5f;

	Transform droneTransform;
	Transform camTransform;
	Vector3 offset;

	void Start()
	{
        droneTransform = transform;
        camTransform = GameObject.Find("DroneCamera").transform;
		offset = camTransform.position - droneTransform.position;
	}

	void LateUpdate()
	{
        camTransform.position = droneTransform.position + offset;
	}
}
