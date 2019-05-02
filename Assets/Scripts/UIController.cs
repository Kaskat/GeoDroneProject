using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class UIController : NetworkBehaviour
{
    public static UIController instance;

    public int verticalInput;
    public int horizontalInput;

    public PlayerNetwork player;

    private void Start()
    {
        instance = this;
    }

    

    public void MoveHorizontal(int value)
    {
        horizontalInput = value;
    }

    public void MoveVertical(int value)
    {
        verticalInput = value;
    }

    public void MakePhoto()
    {
        
    }
}
