using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DroneController : NetworkBehaviour
{
    public float moveSpeed = 6f;
    public float rotationSpeed = 5f;

    private TreatmentVideo _screeshot;

    private void Start()
    {
        _screeshot = GetComponent<TreatmentVideo>();
    }

    public void MovementDrone(float deltaX, float deltaZ)
    {
        deltaX *= moveSpeed;
        deltaZ *= moveSpeed;
        
        Vector3 movement = new Vector3(deltaX, 0, deltaZ);
        
        transform.position += movement * Time.deltaTime;
    }

    public void RotationDrone(float deltaY)
    {
        transform.Rotate(Vector3.up * deltaY * Time.deltaTime);
    }

    public byte[] MakeScreeshot()
    {
        return _screeshot.CreateImage();
    }
}
