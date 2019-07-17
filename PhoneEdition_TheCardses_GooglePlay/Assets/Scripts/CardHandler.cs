using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHandler : MonoBehaviour {

	public static CardHandler s;


	[HideInInspector]
	public Vector3[,] grid = new Vector3[4, 4];
	[HideInInspector]
	public IndividualCard[,] allCards = new IndividualCard[4, 4];

	public int xSize {
		get { return allCards.GetLength (0); }
	}

	public int ySize {
		get { return allCards.GetLength (1); }
	}

	public bool debugDraw = false;
	public GridSettings debugSettings;

	void Awake(){
		debugDraw = false;
		s = this;

		if (GS.a == null)
			FindObjectOfType<GS> ().Awake ();

		if (GS.a.autoSetUpBoard)
			SetUpGrid (GS.a.gridSettings);
		//print (DataHandler.s);
		//print (DataHandler.s.myPlayerinteger);


	}
	

	public bool receivedCardTypes = false;

	// Use this for initialization
	void Start () {
		
		//DataLogger.LogMessage("card grid done");
		if (DataHandler.s.myPlayerInteger == 0 && GS.a.autoSetUpBoard) {
			//DataLogger.LogMessage("initializing starting cards");
			Invoke ("RandomizeAllCards", 0.5f);
			//DataLogger.LogMessage("Card intialized succesfully");
		} else {
			Invoke ("RequestCardTypes",1f);
		}
		   
	}

	void RequestCardTypes () {
		if (!receivedCardTypes) {
			DataHandler.s.SendRequest (DataHandler.RequestTypes.CardTypeRequest);
			Invoke ("RequestCardTypes", 1f);
		}
	}

	void Update (){
		if(debugDraw){
			SetUpGrid (debugSettings);
		}
	}



	//--------------------------------------------------------Initialization Functions

	public void SetUpGrid (GridSettings setting){

		if (setting == null) {
			DataLogger.LogError ("Trying to set up card grid with null settings!");
		}

		//first clean old cards if they exist
		DeleteCards ();


		if (GS.a == null)
			FindObjectOfType<GS> ().Awake ();
		//give us a better centered position
		Vector3 center = new Vector3 (transform.position.x - setting.gridScaleX * ((float)setting.gridSizeX / 2f + setting.centerOffsetX), transform.position.y - setting.gridScaleY * ((float)setting.gridSizeY / 2f + setting.centerOffsetY), transform.position.z);

		//set up arrays according to the sizes that are given to us
		grid = new Vector3[setting.gridSizeX, setting.gridSizeY];
		allCards = new IndividualCard[setting.gridSizeX, setting.gridSizeY];

		//set up grid positions
		for (int i = 0; i < setting.gridSizeX; i++) {
			for (int m = 0; m < setting.gridSizeY; m++) {

				grid [i, m] = new Vector3 ();
				grid [i, m] = center + new Vector3 (i * setting.gridScaleX, m * setting.gridScaleY, 0);

			}
		}

		//instantiate cards at correct positions
		for (int i = 0; i < setting.gridSizeX; i++) {
			for (int m = 0; m < setting.gridSizeY; m++) {

				IndividualCard myCard = Instantiate (GS.a.gfxs.card, grid [i, m], Quaternion.identity).GetComponent<IndividualCard>();
				myCard.transform.parent = transform;
				myCard.transform.position = grid [i, m];
				myCard.transform.localScale = new Vector3 (setting.scaleMultiplier, setting.scaleMultiplier, setting.scaleMultiplier);
				allCards [i, m] = myCard;
				myCard.x = i;
				myCard.y = m;
				myCard.name = "Card " + i.ToString () + "-" + m.ToString ();

			}
		}
	}

	public void RandomizeAllCards(){
		foreach (IndividualCard card in allCards) {
			card.RandomizeCardType ();
		}
	}

	public void SendCardTypesAgain (int targetPlayer) {
		foreach (IndividualCard card in allCards) {
			DataHandler.s.SendCardType (targetPlayer, card.x, card.y, card.cBase.dynamicCardID);
		}
	}

	public void DeleteCards (){
		for (int i = 0; i < allCards.GetLength(0); i++) {
			for (int m = 0; m < allCards.GetLength(1); m++) {
				if (allCards[i,m] != null) {
					Destroy (allCards [i, m].gameObject);
					allCards [i, m] = null;
				}
			}
		}
	}

	//--------------------------------------------------------Helper Functions
	public enum CardActions {Select, UnSelect, Match}

	/*public void AccessCard (int x, int y, CardActions action){
		   
		switch (action) {
		case CardActions.Select:
			allCards [x, y].SelectCard ();
			break;
		case CardActions.UnSelect:
			allCards [x, y].UnSelectCard ();
			break;
		case CardActions.Match:
			allCards [x, y].MatchCard ();
			break;
		case CardActions.ReOpen:
			allCards [x, y].ReOpenCard ();
			break;
		default:

			break;
		}
		   
		//DataHandler.s.SendCardAction (x, y, action);
	}*/

	public void ReceiveCardType (int x, int y, int type) {
		receivedCardTypes = true;
		UpdateCardType (x,y,type);
	}

	public void UpdateCardType (int x, int y, int type){
		allCards [x, y].UpdateCardType (type);
	}

	public List<IndividualCard> GetAllCards () {
		List<IndividualCard> myList = new List<IndividualCard> ();
		foreach (IndividualCard myCard in allCards) {
			myList.Add (myCard);
		}

		return myList;
	}

	public List<IndividualCard> GetOccupiedOrSelectableCards () {
		List<IndividualCard> myList = new List<IndividualCard> ();
		foreach (IndividualCard myCard in allCards) {
			if (myCard.isSelectable || myCard.isOccupied)
				myList.Add (myCard);
		}

		return myList;
	}

	public List<IndividualCard> GetSelectableCards () {
		List<IndividualCard> myList = new List<IndividualCard> ();
		foreach (IndividualCard myCard in allCards) {
			if (myCard.isSelectable)
				myList.Add (myCard);
		}

		return myList;
	}

	public List<IndividualCard> GetPosionCards () {
		List<IndividualCard> myList = new List<IndividualCard> ();
		foreach (IndividualCard myCard in allCards) {
			if (myCard.isPoison)
				myList.Add (myCard);
		}

		return myList;
	}

	public List<IndividualCard> GetRandomizedSelectabeCardList () {
		List<IndividualCard> selectableCards = GetSelectableCards ();
		RandFuncs.Shuffle (selectableCards);
		return selectableCards;
	}

	public List<IndividualCard> GetRandomizedOccupiedOrSelectabeCardList () {
		List<IndividualCard> selectableCards = GetOccupiedOrSelectableCards ();
		RandFuncs.Shuffle (selectableCards);
		return selectableCards;
	}
}
