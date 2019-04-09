using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class LobyPlayerController : NetworkBehaviour {

	public GameObject playerPanel;
	LobyPlayerPanel panelScript;
	NetworkLobbyPlayer manager;

	[SyncVar]
	public bool isReady;

	// Use this for initialization
	void Start () {
		manager = GetComponent<NetworkLobbyPlayer> ();

		GameObject panelParrent = GameObject.Find ("PanelParent");
		playerPanel = (GameObject)Instantiate (playerPanel, panelParrent.transform);
		playerPanel.transform.localScale = new Vector3 (0.8f, 0.8f, 0.8f);

		panelScript= playerPanel.GetComponent<LobyPlayerPanel> ();

		panelScript.playerid = manager.slot;
		panelScript.playerState = manager.readyToBegin;
		panelScript.myPlayer = this;

		if (isLocalPlayer)
			panelScript.isLocalPlayer = true;
		else 
			panelScript.isLocalPlayer = false;

	}
	
	// Update is called once per frame
	void Update () {
		if (SceneManager.GetActiveScene ().buildIndex == 0) {
			panelScript.playerid = manager.slot;
			panelScript.playerState = isReady;
			panelScript.UpdateValues ();
			manager.readyToBegin = isReady;
		}
	}

	public void ChangeReadyState() {
		CmdChangeReadyState ();
	}

	[Command]
	public void CmdChangeReadyState () {
		if (isReady) {
			isReady = false;
		} else {
			isReady = true;
		}

		Update ();
		panelScript.UpdateValues ();
		GameObject.Find ("NetWorkManaGer").GetComponent<NetworkLobbyManager>().CheckReadyToBegin();
	}
}
