using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUpBase : MonoBehaviour {

	public int id;

	public enum PowerUpTypes {active, passive};
	public enum PeriodicActivateTypes { second, select, match };
	public enum AmountType {seconds,times};

	public PowerUpTypes myType;
	public PowerUpManager.PUpTypes pUpType;

	public bool isActive = false;
	[HideInInspector]
	public int power = -1;
	[HideInInspector]
	public float amount = -1;


	[Tooltip ("This determines which type of score \nwill be added when matching\n//--------CARD TYPES---------\n//-1 = don't change type\n// 0 = empty / already gotten\n// 1-7 = normal cards\n// 8-14 = dragons\n//---------------------------\n// 1 = Earth\n// 2 = Fire\n// 3 = Ice\n// 4 = Light\n// 5 = Nether\n// 6 = Poison\n// 7 = Shadow\n//---------------------------\n// 8 = Earth Dragon\n// 9 = Fire Dragon\n//10 = Ice Dragon\n//11 = Light Dragon\n//12 = Nether Dragon\n//13 = Poison Dragon\n//14 = Shadow Dragon\n//---------------------------")]
	public int elementalType = -1;

	[SerializeField]
	[Tooltip ("Spawns when power up is enabled and covers the screen")]
	private GameObject indicatorPrefab;
	[SerializeField]
	[Tooltip ("Spawns when enemy activates power up and shows on their scorepanel")]
	private GameObject indicatorScoreboardPrefab;
	GameObject indicator;

	[SerializeField]
	[Tooltip("Spawns over card and gets destroyed after a while")]
	private GameObject activatePrefab;
	protected GameObject activateEffect;
	[SerializeField]
	[Tooltip ("Spawns over card and gets destroyed when cards are matched")]
	private GameObject selectPrefab;

	List<IndividualCard> mem_Cards = new List<IndividualCard> ();

	//-----------------------------------------------------------------------------------------------Main Functions

	public virtual void Enable (int _elementalType, int _power, float _amount) {
		print ("Activating " + this.GetType().ToString());
		elementalType = _elementalType;
		power = _power;
		amount = _amount;
		isActive = true;

		SendAction (-1, -1, power, amount, PowerUpManager.ActionType.Enable);

		foreach (IndividualCard card in mem_Cards) {
			if (card != null)
				UnSelect (card);
		}

		mem_Cards.Clear ();

		if (myType == PowerUpTypes.active) {
			PowerUpManager.s.canActivatePowerUp = false;
		}
	}


	
	public virtual void Activate (IndividualCard myCard) {
		SendAction (myCard.x, myCard.y, power, amount, PowerUpManager.ActionType.Activate);

		if (activatePrefab != null) {
			activateEffect = Instantiate (activatePrefab, myCard.transform.position, Quaternion.identity);
			if (activateEffect.GetComponent<ElementalTypeSpriteColorChanger> () != null)
				activateEffect.GetComponent<ElementalTypeSpriteColorChanger> ().ChangeColor (elementalType);
		}
	}

	public virtual void Disable () {
		print ("Disabling " + this.GetType ().ToString ());
		isActive = false;
		HideIndicator ();

		SendAction (-1, -1, power, amount, PowerUpManager.ActionType.Disable);

		if (myType == PowerUpTypes.active) {
			PowerUpManager.s.canActivatePowerUp = true;
		}
		LocalPlayerController.s.canSelect = true;

		PowerUpManager.s.PowerUpDisabledCallback ();
	}



	//-----------------------------------------------------------------------------------------------Helper Functions

	protected List<IndividualCard> GetRandomizedSelectabeCardList () {
		return CardHandler.s.GetRandomizedSelectabeCardList();
	}

	protected List<IndividualCard> GetRandomizedOccupiedOrSelectabeCardList () {
		return CardHandler.s.GetRandomizedOccupiedOrSelectabeCardList ();
	}

	protected IndividualCard GetPoisonCard () {
		List<IndividualCard> posionCards = CardHandler.s.GetPosionCards ();
		if (posionCards.Count > 0)
			return posionCards[Random.Range (0, posionCards.Count)];
		else
			return null;
	}

	protected void Reveal (IndividualCard card, float time) {
		if (card == null)
			return;
		card.Reveal (time);
	}

	protected void Select (IndividualCard card) {
		if (card == null)
			return;

		if (card.isOccupied)
			card.KillOccupants ();

		if (card.isSelectable) {
			card.SelectCard (-1);
			SendAction (card.x, card.y, power, amount, PowerUpManager.ActionType.SelectCard);
			mem_Cards.Add (card);
			if (selectPrefab != null)
				card.selectedEffect = (GameObject)Instantiate (selectPrefab, card.transform.position, Quaternion.identity);
		}
	}

	protected void Select (IndividualCard card, bool isActivateEffectToo) {
		Select (card);

		if (isActivateEffectToo && activatePrefab != null) {
			activateEffect = Instantiate (activatePrefab, card.transform.position, Quaternion.identity);
			if (activateEffect.GetComponent<ElementalTypeSpriteColorChanger> () != null)
				activateEffect.GetComponent<ElementalTypeSpriteColorChanger> ().ChangeColor (elementalType);
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

	protected void UnSelectSelectedCards () {
		foreach (IndividualCard card in mem_Cards) {
			card.UnSelectCard ();
			DataHandler.s.SendPlayerAction (card.x, card.y, CardHandler.CardActions.UnSelect);
		}
		mem_Cards = new List<IndividualCard> ();
	}

	protected void UnSelectSelectedCards (float timer) {
		StartCoroutine (UnSelectSelectedCards (mem_Cards.ToArray (), timer));
		mem_Cards = new List<IndividualCard> ();
	}

	IEnumerator UnSelectSelectedCards (IndividualCard[] cards, float timer) {
		yield return new WaitForSeconds (timer);

		foreach (IndividualCard card in cards) {
			card.UnSelectCard ();
			DataHandler.s.SendPlayerAction (card.x, card.y, CardHandler.CardActions.UnSelect);
		}
	}

	protected void NetherReset (IndividualCard card) {
		//the card itself will send nether reset action across the network
		card.NetherReset ();
	}

	protected void PoisonSelectedCards () {
		//the card itself will send its poison status across the network
		foreach (IndividualCard card in mem_Cards) {
			card.PoisonCard ();
		}
	}

	protected void PoisonProtect (IndividualCard card) {
		DataLogger.LogMessage ("Poison Protect not implemented yet!");
	}

	protected void CheckCards () {
		CardChecker.s.CheckCards (mem_Cards.ToArray (),false);
		mem_Cards = new List<IndividualCard> ();
		LocalPlayerController.s.canSelect = true;
	}

	protected void CheckCards (float delay) {
		LocalPlayerController.s.canSelect = true;
		StartCoroutine (DelayedCheck(delay, mem_Cards.ToArray()));
		mem_Cards = new List<IndividualCard> ();
	}

	IEnumerator DelayedCheck (float delay, IndividualCard[] myCards) {
		yield return new WaitForSeconds (delay);
		CardChecker.s.CheckCards (myCards, false);
	}


	protected void ShowIndicator () {
		if (indicator == null) {

			GameObject toSpawn = indicatorPrefab;
			if (toSpawn == null)
				toSpawn = PowerUpManager.s.genericIndicatorPrefab;
		

			indicator = (GameObject)Instantiate (toSpawn, ScoreBoardManager.s.indicatorParent);
			indicator.transform.ResetTransformation ();

			if (indicator.GetComponent<ElementalTypeSpriteColorChanger> () != null)
				indicator.GetComponent<ElementalTypeSpriteColorChanger> ().ChangeColor (elementalType);
		}
	}

	protected void HideIndicator () {
		Invoke ("_HideIndicator",0.4f);
	}

	void _HideIndicator () {
		if (indicator != null) {
			indicator.GetComponent<DisableAndDestroy> ().Engage ();
			indicator = null;
		}
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

		switch (areaId+1) {
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

	protected GameObject networkActiveEffect;
	public virtual void NetworkedActivate (int player, IndividualCard card, int _power, float _amount) {
		networkActiveEffect = Instantiate (activatePrefab, card.transform.position, Quaternion.identity);
	}

	//-----------------------------------------------------------------------------------------------Networking

	GameObject[] networkedIndicators = new GameObject[4];
	public void ReceiveAction (int player, IndividualCard card, int power, float amount, PowerUpManager.ActionType action) {
		try {
			switch (action) {
			case PowerUpManager.ActionType.Enable:
				if (indicatorScoreboardPrefab != null) {
					networkedIndicators[player] = (GameObject)Instantiate (indicatorScoreboardPrefab, ScoreBoardManager.s.scoreBoards[player].transform);
					networkedIndicators[player].transform.ResetTransformation ();
				}
				break;
			case PowerUpManager.ActionType.Activate:
				NetworkedActivate (player, card, power, amount);
				break;
			case PowerUpManager.ActionType.SelectCard:
				if (card.isSelectable) {
					card.SelectCard (-1);
				} else {
					DataHandler.s.NetworkCorrection (card);
				}
				break;
			case PowerUpManager.ActionType.Disable:
				if (networkedIndicators[player] != null)
					networkedIndicators[player].GetComponent<DisableAndDestroy> ().Engage ();
				networkedIndicators[player] = null;
				break;
			default:
				DataLogger.LogError ("Unrecognized power up action PUF");
				break;
			}
		} catch (System.Exception e) {
			DataLogger.LogError (this.name, e);
		}

	}

	void SendAction (int x, int y, int power,float amount, PowerUpManager.ActionType action) {
		PowerUpManager.s.SendPowerUpAction (x, y, pUpType, id, power, amount, action);
	}

	//there exists only unselect, so unselect this card if we were selecting it wrongly
	public void ReceiveNetworkCorrection (IndividualCard myCard) {
		if (mem_Cards.Remove (myCard))
			myCard.DestroySelectedEfect ();
	}
}










//-----------------------------------------------------------------------------------------------Archetypes

public abstract class PowerUp_Active_Instant : PowerUpBase, IActivatable {
	public override void Enable (int _elementalType, int _power, float _amount) {
		base.Enable (_elementalType, _power, _amount);

		ShowIndicator ();

		CharacterStuffController.s.SetActiveButtonState (_amount, -1, false, true);
	}
}

public abstract class PowerUp_Active_Select : PowerUpBase, IActivatable, ICardSelectable, IIndicator {
	public bool isHooked = false;

	public override void Enable (int _elementalType, int _power, float _amount) {
		base.Enable (_elementalType, _power, _amount);

		ShowIndicator ();

		CharacterStuffController.s.SetActiveButtonState (_amount, _amount, false, true);
	}

	public override void Activate (IndividualCard myCard) {
		base.Activate (myCard);
		CharacterStuffController.s.SetActiveButtonState (-5, amount, false, true);
	}

	public override void Disable () {
		base.Disable ();

		if (isHooked) {
			PowerUpManager.s.activateHook -= Activate;
			isHooked = false;
		}
		LocalPlayerController.s.PowerUpMode (false);

		CharacterStuffController.s.SetActiveButtonState (-1, -1, false, false);
	}

	public void HookSelf (bool isOverride) {
		LocalPlayerController.s.PowerUpMode (isOverride);
		if (!isHooked) {
			PowerUpManager.s.activateHook += Activate;
			isHooked = true;
		}
	}
}

public abstract class PowerUp_Active_Effect : PowerUpBase, IActivatable, IIndicator, IPeriodic {
	public override void Enable (int _elementalType, int _power, float _amount) {
		base.Enable (_elementalType, _power, _amount);
	}

	public void HookSelf (PeriodicActivateTypes type, float amount) {
		throw new System.NotImplementedException ();
	}

	public void PeriodicActivate (IndividualCard card) {
		throw new System.NotImplementedException ();
	}
}

public abstract class PowerUp_Passive_Always : PowerUpBase, IPassive {

	public virtual void Enable (int _elementalType, int _power) {
		base.Enable (_elementalType, _power, -1);
	}
}


public abstract class PowerUp_Passive_Periodic : PowerUpBase, IPassive, IPeriodic {

	public virtual void Enable (int _elementalType, int _power) {
		base.Enable (_elementalType, _power, -1);
	}

	public void HookSelf (PeriodicActivateTypes type, float amount) {
		throw new System.NotImplementedException ();
	}

	public void PeriodicActivate (IndividualCard card) {
		throw new System.NotImplementedException ();
	}
}



public interface ICardSelectable {

	//if override, the player wont select the cards normally, instead everything will be moved here
	void HookSelf (bool _isOverride);

	void Activate (IndividualCard _card);
}

public interface IActivatable {

	void Enable (int _elementalType, int _power, float _amount);
}

public interface IIndicator {

}

public interface IPassive {

	void Enable (int _elementalType, int _power);
}

public interface IPeriodic {

	void HookSelf (PowerUpBase.PeriodicActivateTypes _type, float _amount);

	void PeriodicActivate (IndividualCard _card);
}