using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NPCBase : MonoBehaviour {

	public new string name = "Default NPC Name";
	public Color myColor = Color.red;

	bool _isActive = false;
	public bool isActive {
		get {
			return _isActive;
		}
	}


	//----------------------------------values
	float timeRandomPercent = 0.2f;
	float checkTime = 0.5f;
	protected float selectWaitTime = 0.2f;
	Vector3 cardOffset = new Vector3 (0, 0, -0.2f);
	Vector3 startingPositionOffset = new Vector3 (-2f, 0, 0);
	//--------------------------------

	[Tooltip ("The time between moving each card, smaller = faster")]
	public float movementSpeed = 0.2f;
	[Tooltip ("What percent of selections should match")]
	public float AIStrength = 0.5f;
	public bool poisonImmune = false;

	[HideInInspector]
	public bool isFrozen = false;

	public enum NPCDeathTypes { Killable, Stun, Invulnerable }
	[Space]
	public NPCDeathTypes myDeathType = NPCDeathTypes.Killable;
	public GameObject unkillableEffect;
	[Space]
	public bool isStunned = false;
	public float stunTime = 5f;

	[Space]
	[SerializeField]
	[Tooltip ("Spawns over card and gets destroyed after a while")]
	private GameObject activatePrefab;
	protected GameObject activateEffect;
	[SerializeField]
	[Tooltip ("Spawns over card and gets destroyed when cards are matched")]
	private SelectEffect selectPrefab;

	[SerializeField]
	private GameObject diePrefab;

	List<IndividualCard> mem_Cards = new List<IndividualCard> ();
	protected int SelectedCardCount { get { return mem_Cards.Count; } }

	[Space]
	public IndividualCard myCurrentOccupation;
	bool isSpawned = false;

	[Space]
	public GameObject selectionDeniedEffect;
	public GameObject selectionTooManyDeniedEffect;
	public GameObject selectionHintEffect;
	//-----------------------------------------------------------------------------------------------Main Functions

	public virtual void Spawn (IndividualCard myCard) {
		if (!isSpawned) {
			isSpawned = true;
		}

		myCard.myOccupants.Add (this);
		StopAllCoroutines ();
		StartCoroutine (MoveToStartPos (myCard));
	}

	IEnumerator MoveToStartPos (IndividualCard myCard) {
		//come back to this sometime to comment this section please!

		transform.position = myCard.transform.position + cardOffset + cardOffset + cardOffset + startingPositionOffset;
		Vector3 endPos = myCard.transform.position + cardOffset;

		while (Vector3.Distance (transform.position, endPos) > 0.01f) {

			transform.position = Vector3.Lerp (transform.position, endPos, Time.deltaTime * 10f);

			yield return null;
		}
		transform.position = endPos;

		myCurrentOccupation = myCard;

		if (DataHandler.s.myPlayerInteger == 0)
			StartCoroutine (MainLoop ());
	}

	private void Update () {
		if (GameObjectiveMaster.s.isFinished)
			StopCoroutine ("MainLoop");
	}

	public abstract IEnumerator MainLoop ();

	public virtual void Die (bool isForced) {
		SendNPCAction (-1, -1, NPCManager.ActionType.Die, isForced ? 1 : 0);
		//if its not forced and we are not killable > dont die but get stunned or sth
		if (myDeathType != NPCDeathTypes.Killable && !isForced) {
			if (unkillableEffect != null)
				Instantiate (unkillableEffect, transform.position, unkillableEffect.transform.rotation);
			if (myDeathType == NPCDeathTypes.Stun) {
				isStunned = true;
				Invoke ("UnStun", stunTime);
			}
			return;
		}

		if (diePrefab != null)
			Instantiate (diePrefab, transform.position, transform.rotation);
		if (isSpawned) {
			NPCManager.s.ActiveNPCS.Remove (this);
			isSpawned = false;
		}

		StopAllCoroutines ();
		CancelInvoke ();
		CardChecker.s.CheckCards (DataHandler.NPCInteger, mem_Cards, true);
		Destroy (gameObject);
	}

	void UnStun () {
		isStunned = false;
	}

	//-----------------------------------------------------------------------------------------------Helper Functions

	protected bool ShouldIDecideCorrectly () {
		return Random.value < AIStrength;
	}

	protected IEnumerator MoveToPosition (IndividualCard to) {
		if (to == null) {
			yield return null;
		} else {
			if (DataHandler.s.myPlayerInteger == 0)
				SendNPCAction (to.x, to.y, NPCManager.ActionType.GoToPos, -1);

			if (selectionHintEffect != null)
				Instantiate (selectionHintEffect, to.transform.position, selectionHintEffect.transform.rotation);
			yield return MoveToPositionCoroutine (to);
		}
	}

	IEnumerator MoveToPositionCoroutine (IndividualCard to) {
		if (to == null) {
			yield return new WaitForSeconds (1f);
		}

		to.isTargeted = true;

		int distanceX = to.x - myCurrentOccupation.x;
		int distanceY = to.y - myCurrentOccupation.y;
		int distance = Mathf.Abs (distanceX) + Mathf.Abs (distanceY);

		while (distance > 0) {
			int xOff = 0;
			int yOff = 0;
			if (distanceY > 0) {
				yOff = 1;
			} else if (distanceY < 0) {
				yOff = -1;
			}
			if (distanceX > 0) {
				xOff = 1;
			} else if (distanceX < 0) {
				xOff = -1;
			}

			if (Mathf.Abs (xOff) + Mathf.Abs (yOff) == 2) {
				if (Random.value > 0.5f) {
					xOff = 0;
				} else {
					yOff = 0;
				}
			}

			if (Mathf.Abs (xOff) + Mathf.Abs (yOff) == 0) {
				break;
			}
			IndividualCard targetCard = CardHandler.s.allCards[myCurrentOccupation.x + xOff, myCurrentOccupation.y + yOff];


			yield return MoveOneSpot (myCurrentOccupation, targetCard);
			distanceX = to.x - myCurrentOccupation.x;
			distanceY = to.y - myCurrentOccupation.y;
			distance = Mathf.Abs (distanceX) + Mathf.Abs (distanceY);


			yield return new WaitForSeconds (movementSpeed);
		}

		//moving finsihed
		yield return new WaitForSeconds (selectWaitTime);
	}

	IEnumerator MoveOneSpot (IndividualCard from, IndividualCard to) {

		//Vector3 startPos = from.transform.position + cardOffset;
		Vector3 endPos = to.transform.position + cardOffset;

		while (Vector3.Distance (transform.position, endPos) > 0.01f) {
			if (!isStunned)
				transform.position = Vector3.Lerp (transform.position, endPos, Time.deltaTime * 20f);

			yield return null;
		}

		transform.position = endPos;

		from.myOccupants.Remove (this);
		to.myOccupants.Add (this);
		myCurrentOccupation = to;
		yield return null;
	}

	protected bool CanTargetSpot (IndividualCard myCard) {
		if (myCard.isTargeted == false && !myCard.cBase.isItem && myCard.cBase.isAITargetable)
			return true;
		else
			return false;
	}

	protected float RandomTimeMultiplier () {
		return Random.Range (1f - timeRandomPercent, 1f + timeRandomPercent);
	}

	protected List<IndividualCard> GetAllCards () {
		return CardHandler.s.GetAllCards ();
	}

	protected List<IndividualCard> GetRandomizedMoveableCardList () {
		List<IndividualCard> l = CardHandler.s.GetRandomizedSelectabeCardList ().FindAll (s => CanTargetSpot (s));

		if (l.Count == 0)
			l.Add (null);
		return l;
	}

	protected IndividualCard GetPoisonCard () {
		List<IndividualCard> posionCards = CardHandler.s.GetPosionCards ();
		if (posionCards.Count > 0)
			return posionCards[Random.Range (0, posionCards.Count)];
		else
			return null;
	}

	int denyCount = 0;
	protected bool Select (IndividualCard card) {
		if (card == null)
			return false;

		if (DataHandler.s.myPlayerInteger == 0)
			SendNPCAction (card.x, card.y, NPCManager.ActionType.SelectCard, -1);

		card.isTargeted = false;
		if (card.isSelectable) {
			card.SelectCard (DataHandler.NPCInteger);
			//SendAction (card.x, card.y, power, amount, PowerUpManager.ActionType.SelectCard);
			mem_Cards.Add (card);
			if (selectPrefab != null)
				card.selectedEffect = Instantiate (selectPrefab.gameObject, card.transform.position, Quaternion.identity);

			denyCount = 0;
			return true;
		} else {
			return false;
		}
	}

	protected void Denied () {
		if (selectionDeniedEffect != null)
			Instantiate (selectionDeniedEffect, transform.position, selectionDeniedEffect.transform.rotation);
		denyCount++;

		if (denyCount == 3 || (denyCount > 3 && Random.value > 0.5f)) {
			Invoke ("TooManyDeniesEffect", 1f);
		}
	}

	void TooManyDeniesEffect () {
		if (selectionTooManyDeniedEffect != null)
			Instantiate (selectionTooManyDeniedEffect, transform.position, selectionTooManyDeniedEffect.transform.rotation);
	}

	protected void Activate (IndividualCard card) {

		if (DataHandler.s.myPlayerInteger == 0)
			SendNPCAction (card.x, card.y, NPCManager.ActionType.Activate, -1);

		if (activatePrefab != null) {
			activateEffect = Instantiate (activatePrefab, card.transform.position, Quaternion.identity);
			/*if (activateEffect.GetComponent<ElementalTypeSpriteColorChanger> () != null)
				activateEffect.GetComponent<ElementalTypeSpriteColorChanger> ().ChangeColor (elementalType);*/
		}
	}

	/// <summary>
	/// Dont use this method unless necessary, CheckCards will automatically unselect for you
	/// </summary>
	/// <param name="card"></param>
	protected void UnSelect (IndividualCard card) {
		card.UnSelectCard ();
		DataHandler.s.SendPlayerAction (card.x, card.y, CardHandler.CardActions.UnSelect);
	}

	protected void CheckCards () {
		CheckCards (checkTime);
	}

	protected void CheckCards (float delay) {
		StartCoroutine (DelayedCheck (delay, mem_Cards));
		mem_Cards = new List<IndividualCard> ();
	}

	IEnumerator DelayedCheck (float delay, List<IndividualCard> _mem_Cards) {
		yield return new WaitForSeconds (delay);
		CardChecker.s.CheckCards (DataHandler.NPCInteger, _mem_Cards, false);
	}



	//Line Sequence
	// 7 5 3 1 2 4 6
	protected IndividualCard GetLineSequence (IndividualCard start, int lineId, int power) {

		int xLength = CardHandler.s.allCards.GetLength (0);
		int offset = Mathf.FloorToInt (power / 2f);



		if (xLength > start.x + offset - lineId && start.x + offset - lineId >= 0)
			return CardHandler.s.allCards[start.x + offset - lineId, start.y];
		else
			return null;
	}


	//Area Sequence
	/*
	 *   6  4  8
	 *   3  1  2
	 *   9  5  7
	 */
	protected IndividualCard GetAreaSequence (IndividualCard start, int areaId, int power) {

		int xLength = CardHandler.s.allCards.GetLength (0);
		int yLength = CardHandler.s.allCards.GetLength (1);
		int xOffset = 0;
		int yOffset = 0;

		switch (areaId + 1) {
		case 1:
			//do nothing as this is the middle
			break;
		case 2:
			xOffset = 1;
			break;
		case 3:
			xOffset = -1;
			break;
		case 4:
			yOffset = 1;
			break;
		case 5:
			yOffset = -1;
			break;
		case 6:
			xOffset = -1;
			yOffset = 1;
			break;
		case 7:
			xOffset = 1;
			yOffset = -1;
			break;
		case 8:
			xOffset = 1;
			yOffset = 1;
			break;
		case 9:
			xOffset = -1;
			yOffset = -1;
			break;
		}


		if ((xLength > start.x + xOffset && start.x + xOffset >= 0)
			&& (yLength > start.y + yOffset && start.y + yOffset >= 0)) {

			return CardHandler.s.allCards[start.x + xOffset, start.y + yOffset];
		} else {
			return null;
		}
	}

	//-----------------------------------------------------------------------------------------------Networking

	public void ReceiveNPCAction (IndividualCard card, NPCManager.ActionType action, int data) {
		try {
			switch (action) {
			case NPCManager.ActionType.Die:
				Die (data == 1);
				break;
			case NPCManager.ActionType.SelectCard:
				Select (card);
				break;
			case NPCManager.ActionType.Activate:
				Activate (card);
				break;
			case NPCManager.ActionType.GoToPos:
				StartCoroutine (MoveToPosition (card));
				break;
			default:
				DataLogger.LogError ("Unrecognized power up action PUF");
				break;
			}
		} catch (System.Exception e) {
			DataLogger.LogError (this.name, e);
		}
	}

	public void ReceiveNetworkCorrection (IndividualCard myCard) {
		if (mem_Cards.Remove (myCard))
			myCard.DestroySelectedEfect ();
	}

	public void SendNPCAction (int x, int y, NPCManager.ActionType action, int data) {
		NPCManager.s.SendNPCAction (x, y, this, action, data);
	}
}
