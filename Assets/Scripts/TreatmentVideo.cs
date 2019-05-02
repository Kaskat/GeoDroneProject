using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreatmentVideo : MonoBehaviour
{
    public Camera bakeCam;
    public RenderTexture ren;
    public bool enableShootEnvironment = false;

    public float cameraMatrixSize = 160f;
    public float GSD = .01f;
    [Range(0, 100)] public int overlayPercent = 60;

    [SerializeField] int numberFrame = 0;
    float shootDistant;
    string path;

    Vector2 previousPointShoot;

    void Start()
    {
        //bakeCam = GameObject.Find("RenderCamera").GetComponent<Camera>();
        Debug.Log(bakeCam);
        path = SaveLocation();
    }

    void FixedUpdate()
    {
        //if (Input.GetKeyDown(KeyCode.Mouse0))
        //{
        //    enableShootEnvironment = true;
        //    CreateImage();
        //}
        //else if (Input.GetKeyUp(KeyCode.Mouse0))
        //    enableShootEnvironment = false;

        //if (CheckToScreenshot())
        //    CreateImage();
    }

    bool CheckToScreenshot()
    {
        if (enableShootEnvironment && Vector2.Distance(GetPositionCamera(), previousPointShoot) >= shootDistant)
            return true;

        return false;
    }

    public byte[] CreateImage()
    {
        enableShootEnvironment = true;

        string image = "frame_" + numberFrame;
        numberFrame++;

        //path += image;

        bakeCam.targetTexture = ren;

        RenderTexture currentRT = RenderTexture.active;
        bakeCam.targetTexture.Release();

        RenderTexture.active = bakeCam.targetTexture;
        bakeCam.Render();

        Texture2D imgPng = new Texture2D(bakeCam.targetTexture.width, bakeCam.targetTexture.height, TextureFormat.ARGB32, false);
        imgPng.ReadPixels(new Rect(0, 0, bakeCam.targetTexture.width, bakeCam.targetTexture.height), 0, 0);
        imgPng.Apply();
        RenderTexture.active = currentRT;
        bakeCam.targetTexture = null;

        byte[] bytesPng = imgPng.EncodeToPNG();
        
        //File.WriteAllBytes(path + image + ".png", bytesPng);

        Debug.Log(path + " created!");
        previousPointShoot = GetPositionCamera();

        shootDistant = CalculateShootDistance();

        enableShootEnvironment = false;

        return bytesPng;
    }

    string SaveLocation()
    {
        string saveLocation = Application.streamingAssetsPath + "/Images/";

        if (!Directory.Exists(saveLocation))
        {
            Directory.CreateDirectory(saveLocation);
        }

        return saveLocation;
    }

    Vector2 GetPositionCamera()
    {
        return new Vector2(bakeCam.transform.position.x, bakeCam.transform.position.y);
    }

    float CalculateShootDistance()
    {
        return cameraMatrixSize * GSD * (100 - overlayPercent) / 100f;
    }
}
