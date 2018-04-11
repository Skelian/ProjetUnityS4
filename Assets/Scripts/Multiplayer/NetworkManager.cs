using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviour {
	
	NetworkClient myClient;
	private UnityEngine.Networking.NetworkManager nm;

    public InputField portTxtHost, portTxtJoin, hostIp;

    public enum Port
    {
        HOST,
        JOIN
    };

	// Use this for initialization
	void Start () {
		nm = GetComponent<UnityEngine.Networking.NetworkManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetupServer(){
		/*int port = UpdatePort ();
		NetworkServer.Listen (port);*/
		nm.StartServer ();

	}

	public void SetupClient(){
		/*int port = UpdatePort ();
		string ip = UpdateServerIp ();
		myClient = new NetworkClient ();
		myClient.RegisterHandler (MsgType.Connect, OnConnected);
		myClient.Connect ("127.0.0.1", 4444);*/
		nm.StartClient ();
	}

	public void SetupLocalClient(){
		/*myClient = ClientScene.ConnectLocalServer ();
		myClient.RegisterHandler (MsgType.Connect, OnConnected);*/
		nm.StartClient ();
	}

	public void Host(){
        nm.matchPort = UpdatePort(Port.HOST);
		nm.StartHost ();
        nm.StartClient();
	}

    public void Join()
    {
        nm.matchPort = UpdatePort(Port.JOIN);
        nm.serverBindAddress = UpdateServerIp();
        nm.StartClient();
    }

    public int UpdatePort (Port code){
        string port;
        switch (code)
        {
            case Port.HOST:
                port = portTxtHost.text;
                break;
            case Port.JOIN:
                port = portTxtJoin.text;
                break;
            default:
                port = "7777";
                break;
        }

        return int.Parse(port);
    }

	public string UpdateServerIp(){
		return hostIp.text;
	}

	public void OnConnected(NetworkMessage netMsg){
		Debug.Log ("Connected");
	}
}
