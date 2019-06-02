using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MPMenu : MonoBehaviour {

	public MultiPlayerConnectionManager mp;

	public Button giris;
	public Button exit;
	public Button esBul;

	public Text textPlayer;

	public int playerCount = 2;


	public void IncreasePlayerCount () {
		ChangePlayerCount (1);

	}

	public void DecreasePlayerCount () {
		ChangePlayerCount (-1);
	}

	void  ChangePlayerCount (int amount){

		playerCount += amount;
		playerCount = Mathf.Clamp (playerCount, 1, 4);
		textPlayer.text = playerCount.ToString ();
		MultiPlayerConnectionManager.s.playerCount = playerCount;
	}

	// Use this for initialization
	void Start () {
		mp = MultiPlayerConnectionManager.s;
		giris.interactable = true;
		esBul.interactable = false;
		ChangePlayerCount (0);
		//exit.interactable = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void GirisYapildiMi (bool Girildi){
		if (Girildi) {
			giris.interactable = false;
			esBul.interactable = true;
			//exit.interactable = true;
		} else {
			giris.interactable = true;
			esBul.interactable = false;
			//exit.interactable = false;
		}
	}
}
