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

	[HideInInspector]
	public bool isFrozen = false;
	public bool isKillable = true;

	//----------------------------------values
	float timeRandomPercent = 0.2f;
	float checkTime = 0.5f;
	protected float selectWaitTime = 0.2f;
	Vector3 cardOffset = new Vector3 (0, 0, -0.2f);
	Vector3 startingPositionOffset = new Vector3 (-2f, 0, 0);
	//--------------------------------


	public float movementSpeed = 0.2f;
	[Tooltip("What percent of selections should match")]
	public float AIStrength = 0.5f;
	public bool poisonImmune = false;


	public int power = 1;

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

	public IndividualCard myCurrentOccupation;
	bool isSpawned = false;

	public enum UnKillableTypes { Stun, Invulnerable }
	public UnKillableTypes myUnkillableType = UnKillableTypes.Stun;
	public GameObject unkillableEffect;
	public bool isStunned = false;
	public float stunTime = 5f;
	//-----------------------------------------------------------------------------------------------Main Functions
	public virtual void Spawn () {
		Spawn (Random.Range (0, CardHandler.s.allCards.GetLength (1)));
	}

	public virtual void Spawn (int y) {
		if (!isSpawned) {
			GameObjectiveFinishChecker.s.ActiveNPCS.Add (this);
			isSpawned = true;
		}
		StartCoroutine (MoveToStartPos(y));
	}

	IEnumerator MoveToStartPos (int y) {
		int x = 0;
		IndividualCard myCard = CardHandler.s.allCards[x, y];
		int trials = 0;
		while (myCard.myOccupants.Count > (Mathf.FloorToInt(trials/CardHandler.s.allCards.Length) + 1)) {
			x++;
			if (x > CardHandler.s.allCards.GetLength (0)) {
				x = 0;
				y++;
				if (y > CardHandler.s.allCards.GetLength (1)) {
					y = 0;
				}
			}
			myCard = CardHandler.s.allCards[x, y];
			trials++;
		}

		myCard.myOccupants.Add(this);

		transform.position = CardHandler.s.allCards[0, y].transform.position + cardOffset +cardOffset + cardOffset+ startingPositionOffset;
		Vector3 endPos = myCard.transform.position + cardOffset;

		while (Vector3.Distance (transform.position, endPos) > 0.01f) {

			transform.position = Vector3.Lerp (transform.position, endPos, Time.deltaTime * 10f);

			yield return null;
		}
		transform.position = endPos;

		myCurrentOccupation = myCard;

		StartCoroutine (MainLoop());
	}

	private void Update () {
		if (GameObjectiveFinishChecker.s.isFinished)
			StopCoroutine ("MainLoop");
	}

	public abstract IEnumerator MainLoop ();

	public virtual void Die () {
		if (!isKillable) {
			Instantiate (unkillableEffect, transform.position, unkillableEffect.transform.rotation);
			if (myUnkillableType == UnKillableTypes.Stun) {
				isStunned = true;
				Invoke ("UnStun", stunTime);
			}
			return;
		}

		Instantiate (diePrefab, transform.position, transform.rotation);
		if (isSpawned) {
			GameObjectiveFinishChecker.s.ActiveNPCS.Remove (this);
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

		while (Vector3.Distance(transform.position, endPos) > 0.01f) {
			if(!isStunned)
				transform.position = Vector3.Lerp (transform.position, endPos, Time.deltaTime * 20f);

			yield return null;
		}

		transform.position = endPos;

		from.myOccupants.Remove(this);
		to.myOccupants.Add (this);
		myCurrentOccupation = to;
		yield return null;
	}

	protected bool CanTargetSpot (IndividualCard myCard) {
		if (myCard.isTargeted == false && !myCard.cBase.isItem && myCard.cBase.elementType <= 7)
			return true;
		else
			return false;
	}

	protected float RandomTimeMultiplier () {
		return Random.Range (1f - timeRandomPercent, 1f + timeRandomPercent);
	}

	protected List<IndividualCard> GetRandomizedMoveableCardList () {
		List < IndividualCard > l = CardHandler.s.GetRandomizedSelectabeCardList ().FindAll (s => CanTargetSpot (s));

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

	protected void Select (IndividualCard card) {
		if (card == null)
			return;
		card.isTargeted = false;
		if (card.isSelectable) {
			card.SelectCard (DataHandler.NPCInteger);
			//SendAction (card.x, card.y, power, amount, PowerUpManager.ActionType.SelectCard);
			mem_Cards.Add (card);
			if (selectPrefab != null)
				card.selectedEffect = (GameObject)Instantiate (selectPrefab.gameObject, card.transform.position, Quaternion.identity);
		}
	}

	protected void Select (IndividualCard card, bool isActivateEffectToo) {
		Select (card);

		if (isActivateEffectToo && activatePrefab != null) {
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

}
