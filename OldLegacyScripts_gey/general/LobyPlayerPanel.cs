using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LobyPlayerPanel : MonoBehaviour {


	[HideInInspector]
	public LobyPlayerController myPlayer;

	public int playerid = -1;
	public bool playerState = false;
	public bool isLocalPlayer = false;


	public Text myName;
	public Text myState;
	public GameObject myButton;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void UpdateValues () {
		myName.text = "Player " + playerid;
		if (playerState) {
			myState.text = "Ready";
			myState.color = Color.green;
		} else {
			myState.text = "Not Ready";
			myState.color = Color.red;
		}

		myButton.SetActive (isLocalPlayer);
	}

	public void ChangeState () {
		myPlayer.ChangeReadyState ();
	}
}
