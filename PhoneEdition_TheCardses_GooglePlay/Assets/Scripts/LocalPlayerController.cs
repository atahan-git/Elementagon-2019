using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPlayerController : MonoBehaviour {

	public static LocalPlayerController s;
	public static bool isActive = false;


	CardHandler cardHand;
	PowerUpManager powerUp;


	[HideInInspector]
	public bool canSelect = false;
	[HideInInspector]
	public bool isPowerUp = false;

	GameObject myEffect;
	List<IndividualCard> mem_Cards = new List<IndividualCard>();

	public int handSize = 2;

	void Start () {
		s = this;

		switch (DataHandler.s.toChar(DataHandler.s.myPlayerInteger)) {
		case DataHandler.p_blue:
			myEffect = GS.a.gfxs.selectEffects[1];
			break;
		case DataHandler.p_red:
			myEffect = GS.a.gfxs.selectEffects[2];
			break;
		case DataHandler.p_green:
			myEffect = GS.a.gfxs.selectEffects[3];
			break;
		case DataHandler.p_yellow:
			myEffect = GS.a.gfxs.selectEffects[4];
			break;
		default:
			myEffect = GS.a.gfxs.selectEffects[1];
			break;
		}

		canSelect = true;
	}


	//--------------------------------------------------------------------------------------------------------slide mode stuff

	/*
	 * 			  1
	 * 			  |
	 * 		 0---   ---2
	 * 			  |
	 * 			  3
	 * 
	 */

	//---Enemy Player Handler handles this movement stuff

	//--------------------------------------------------------------------------------------------------------slide mode stuff


	void Update () {
		if (!isActive) {
			return;
		}


		if (Input.GetMouseButtonUp (0) && canSelect && !CardsScrollController.s.isScrolling && Input.mousePosition.y > Screen.height / 4.36f) {
			RaycastSelect ();
		}
	}

	void RaycastSelect (){
		RaycastHit hit = new RaycastHit ();

		if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit)) {

			//print (myHet.collider.transform.parent.gameObject.name);

			IndividualCard myCardS = hit.collider.gameObject.GetComponentInParent<IndividualCard> ();

			SelectionCheck (myCardS);
		}
	}

	public void SelectionCheck (IndividualCard myCardS){
		if (!isActive)
			return;
		
		if (canSelect) {
			if (myCardS != null) {
				if (myCardS.isSelectable) {
					canSelect = false;

					if (!isPowerUp) {
						SelectCard (myCardS);
					} else {
						PowerUpSelect (myCardS);
					}
				}
			}
		} 
	}


	public void SelectCard (IndividualCard myCardS) {
		canSelect = false;
		if (myCardS.cBase.isPoison && !canSelect) {
			CardChecker.s.UnSelectCards (mem_Cards);
			CardChecker.s.EmptyArray (mem_Cards);

			canSelect = true;
			PoisonMaster.s.ChoosePoisonCard (DataHandler.s.myPlayerInteger, myCardS, "local player");
			return;
		}

		/*if (activeTimer != null)
			StopCoroutine (activeTimer);*/

		PowerUpManager.s.SelectInvoke (myCardS);
		myCardS.selectedEffect = (GameObject)Instantiate (myEffect, myCardS.transform.position, Quaternion.identity);
		myCardS.selectEffectID = DataHandler.s.myPlayerInteger;
		myCardS.SelectCard (DataHandler.s.myPlayerInteger);
		DataHandler.s.SendPlayerAction (myCardS.x, myCardS.y, CardHandler.CardActions.Select);
		mem_Cards.Add (myCardS);
		canSelect = true;
		/*activeTimer = DeselectTimer (myCardS);
		StartCoroutine (activeTimer);*/

		if (mem_Cards.Count >= handSize) {
			canSelect = false;
			Invoke ("CheckCards", GS.a.playerSelectionSettings.checkSpeedPlayer);
		}
	}

	void CheckCards () {
		PowerUpManager.s.CheckInvoke ();

		canSelect = true;
		CardChecker.s.CheckCards (mem_Cards, true);
	}

	public void PowerUpMode (bool state){
		isPowerUp = state;

		CardChecker.s.UnSelectCards (mem_Cards);
		CardChecker.s.EmptyArray (mem_Cards);
	}

	void PowerUpSelect(IndividualCard myCardS){
		canSelect = true;
		PowerUpManager.s.ActivateInvoke (myCardS);
	}

	public void SetHandSize (int size) {
		handSize = size;
	}

	/*IEnumerator activeTimer = null;
	IEnumerator DeselectTimer (IndividualCard myCard){
		//print (GS.a.playerSelectionSettings.deselectSpeedPlayer);
		if (GS.a.playerSelectionSettings.deselectSpeedPlayer > 0) {
			yield return new WaitForSeconds (GS.a.playerSelectionSettings.deselectSpeedPlayer);

			myCard.UnSelectCard ();
			DataHandler.s.SendPlayerAction (myCard.x, myCard.y, CardHandler.CardActions.UnSelect);
			ReceiveNetworkCorrection (myCard);
		} else {
			yield return null;
		}
	}*/

	public void DeselectAll (){
		try {
			CancelInvoke ("CheckCards");
			CardChecker.s.UnSelectCards (mem_Cards);
			CardChecker.s.EmptyArray (mem_Cards);
			canSelect = true;
		} catch (System.Exception e) {
			DataLogger.LogError (this.name, e);
		}
	}

	//-----------------------------------------------------------------------------------------------Networking
	//there exists only unselect, so unselect this card if we were selecting it wrongly
	public void ReceiveNetworkCorrection (IndividualCard myCard){

		myCard.DestroySelectedEfect ();
		CancelInvoke ("CheckCards");
		canSelect = true;

		mem_Cards.Remove (myCard);

		/*if (mem_Cards [0] == myCard) {
			mem_Cards [0] = null;
			myCard.DestroySelectedEfect ();

			if(activeTimer != null)
				StopCoroutine (activeTimer);

			if (mem_Cards [1] != null) {
				mem_Cards [0] = mem_Cards [1];
				mem_Cards [1] = null;
				CancelInvoke ("CheckCards");
				canSelect = true;

				activeTimer = DeselectTimer (mem_Cards [0]);
				StartCoroutine (activeTimer);
			} else {
			}
		}*/
	}

	public IndividualCard[] GiveMem () {
		return mem_Cards.ToArray ();
	}
}
