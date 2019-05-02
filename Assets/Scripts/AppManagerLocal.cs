using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppManagerLocal : MonoBehaviour
{
    public static AppManagerLocal instance;

    public GameObject serverMenu;
    public GameObject localCamera;

    private void Start()
    {
        instance = this;
    }

    public void ActivateServer()
    {
        localCamera.SetActive(false);
    }

    public void ActivateClient()
    {
        //serverMenu.SetActive(false);
    }
}
