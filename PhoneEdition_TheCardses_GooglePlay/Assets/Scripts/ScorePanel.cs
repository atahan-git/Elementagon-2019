using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ScorePanel : MonoBehaviour {


	public int playerid = 0;
	public int scoreVal = 0;
	public string playerName = "NO PLAYER";

	public Text myName;
	public Text score;

	public Image myImage;
	public Color[] colors;
	// Use this for initialization
	/*void Awake () {
		if (DataHandler.s != null) {
			//UpdateName (1);
		} else {
			if (playerid == 2 || playerid == 3) {
				//Destroy (gameObject);
				myName.text = "NO PLAYER";
			}
		}
		myName.text = playerName;
		score.text = "0";
	}*/

	float bigSize = 65f;

	void Start () {

		myName.text = playerid.ToString ();

		//RectTransform panelParent = GameObject.Find ("LeftPanel").GetComponent<RectTransform> ();

		/*gameObject.GetComponent<RectTransform> ().parent = panelParent;
		gameObject.GetComponent<RectTransform> ().localScale = panelParent.localScale;*/

		UpdateId (playerid);
		//Invoke ("CheckIt", 0.5f);
	}

	static ScorePanel mainPlayer;
	public void SetValues (int id, string name, bool isMainPlayer) {
		print ("Score Panel Checked Values - " + playerid);
		playerName = name;
		UpdateId (id);

		if (!isMainPlayer) {
			if (mainPlayer != null) {
				mainPlayer.GetComponent<RectTransform> ().SetAsLastSibling ();
			}
			//GameObject.Find ("PowerUps").GetComponent<RectTransform>().SetAsFirstSibling();


		} else {

			/*gameObject.GetComponent<LayoutElement> ().minHeight = bigSize;*/
			gameObject.GetComponent<RectTransform> ().SetAsLastSibling ();
			//GameObject.Find ("PowerUps").GetComponent<RectTransform>().SetAsFirstSibling();
			gameObject.gameObject.name = "Score Panel Main Player";
			mainPlayer = this;
		}

		Invoke ("UpdateGfxAgain", 0.5f);
	}

	public void SetValues (int id, string name, bool isMainPlayer, Color myColor) {
		print ("Score Panel Checked Values - " + playerid);
		playerName = name;
		UpdateId (id);
		myImage.color = myColor;

		if (!isMainPlayer) {
			if (mainPlayer != null) {
				mainPlayer.GetComponent<RectTransform> ().SetAsLastSibling ();
			}
			//GameObject.Find ("PowerUps").GetComponent<RectTransform>().SetAsFirstSibling();


		} else {

			/*gameObject.GetComponent<LayoutElement> ().minHeight = bigSize;*/
			gameObject.GetComponent<RectTransform> ().SetAsLastSibling ();
			//GameObject.Find ("PowerUps").GetComponent<RectTransform>().SetAsFirstSibling();
			gameObject.gameObject.name = "Score Panel Main Player";
			mainPlayer = this;
		}

		Invoke ("UpdateGfxAgain", 0.5f);
	}

	void UpdateGfxAgain () {
		LayoutRebuilder.ForceRebuildLayoutImmediate (transform.parent.GetComponent<RectTransform> ());
	}

	// Update is called once per frame
	void Update () {
	
	}

	public float delay = 1.5f;

	public void UpdateScore (int value, bool isDelayed) {
		scoreVal = value;
		if (isDelayed) {
			StartCoroutine (UpdateGfx (scoreVal, delay));
		} else {
			StartCoroutine (UpdateGfx (scoreVal, 0));
		}
	}

	IEnumerator UpdateGfx (int scoreVal, float delayAm) {
		yield return new WaitForSeconds (delayAm);
		score.text = scoreVal.ToString ();
	}

	void UpdateId (int value) {
		playerid = value;
		myName.text = playerName;
		if (playerid >= 0) {
			myImage.color = colors [playerid];
			if (playerName == "Allies") {
				myImage.color = colors[4];
			} else if (playerName == "Enemies") {
				myImage.color = colors[5];
			}
			//ScoreBoardManager.s.scoreBoards [playerid] = gameObject;
		}
	}
		
}
