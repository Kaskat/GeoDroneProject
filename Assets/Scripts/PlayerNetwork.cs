using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;

public class PlayerNetwork : NetworkBehaviour
{
    [SerializeField]
    private GameObject _dronPrefab;

    private bool _localServer;
    private bool _localClient;

    private DroneController _dron;

    private int _numberFrame = 0;

    private void Awake()
    {
        


    }

    private void Start()
    {

        if (isServer && isLocalPlayer)
        {
            gameObject.name = "Server";
            _localServer = true;
        }
        else if (isServer && !isLocalPlayer)
        {
            gameObject.name = "Client";
        }
        else if (!isServer && isLocalPlayer)
        {
            gameObject.name = "Client";
            CmdCreateDron(gameObject);
            _localClient = true;
        }
        else
        {
            gameObject.name = "Server";
        }
    }

    private void Update()
    {
        if (!isServer || !isLocalPlayer) return;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        if (x != 0 || z != 0)
        {
            CmdMoveDron(x, z);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            CmdTakePhoto();
        }
    }

    [Command]
    private void CmdCreateDron(GameObject player)
    {
        var dron = Instantiate(_dronPrefab, Vector3.zero, Quaternion.identity);
        NetworkServer.SpawnWithClientAuthority(dron, player);
    }

    [Command]
    private void CmdMoveDron(float x, float z)
    {
        RpcMoveDron(x, z);
    }
    
    [ClientRpc]
    private void RpcMoveDron(float x, float z)
    {
        if (_localServer) return;
        
        if (_dron == null)
        {
            var dronObj = GameObject.FindGameObjectWithTag("Dron");

            if (dronObj == null) return;
            else _dron = dronObj.GetComponent<DroneController>();
        }

        _dron.MovementDrone(x, z);
    }

    [Command]
    private void CmdTakePhoto()
    {
        RpcTakePhoto();
    }

    [ClientRpc]
    private void RpcTakePhoto()
    {
        if (_localServer) return;

        var obj = GameObject.Find("Client");
        if (obj == null) return;

        if (_dron == null)
        {
            var dronObj = GameObject.FindGameObjectWithTag("Dron");

            if (dronObj == null) return;
            else _dron = dronObj.GetComponent<DroneController>();
        }

        obj.GetComponent<PlayerNetwork>().CmdSendPhoto(_dron.MakeScreeshot());
    }

    [Command]
    public void CmdSendPhoto(byte[] photo)
    {
        RpcSendPhoto(photo);
    }

    [ClientRpc]
    public void RpcSendPhoto(byte[] photo)
    {
        if (_localClient) return;

        string image = "frame_" + _numberFrame;
        _numberFrame++;

        Debug.Log(photo.Length);
        File.WriteAllBytes(SaveLocation() + image + ".png", photo);
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
}
