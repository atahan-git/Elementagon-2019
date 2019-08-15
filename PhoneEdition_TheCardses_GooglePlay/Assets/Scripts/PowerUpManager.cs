using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour {

	public static PowerUpManager s;

	[HideInInspector]
	public bool canActivatePowerUp = true;

	public enum PUpTypes { equipment, potion};

	public PowerUpBase[] equipmentPUps;
	public PowerUpBase[] potionPUps;

	PowerUpBase activePUp;

	//for posion handling
	//public PowerUp_Poison pPoison;


	[Tooltip ("//--------CARD TYPES---------\n// 0 = any type\n// 1-7 = normal cards\n// 8-14 = dragons\n//---------------------------\n// 1 = Earth\n// 2 = Fire\n// 3 = Ice\n// 4 = Light\n// 5 = Nether\n// 6 = Poison\n// 7 = Shadow\n//---------------------------\n// 8 = Earth Dragon\n// 9 = Fire Dragon\n//10 = Ice Dragon\n//11 = Light Dragon\n//12 = Nether Dragon\n//13 = Poison Dragon\n//14 = Shadow Dragon\n//---------------------------")]
	public Color[] dragonColors = new Color[32];
	public GameObject genericIndicatorPrefab;
	public GameObject indicatorScoreboardPrefab;

	public Transform throwToCardStartPos;

	// Use this for initialization
	void Awake () {	   
		s = this;	
	}


	public void EnablePowerUp (PUpTypes type, int id, int power, float amount, Color effectColor) {
		DataLogger.LogMessage ("Activating pup: type:^"  + type.ToString() + " - id: " + id.ToString() + " - power: " + power.ToString() + " - amount: " + amount.ToString());
		if (canActivatePowerUp == false)
			return;

		if (type == PUpTypes.equipment) {
			if (id < equipmentPUps.Length) {
				if (equipmentPUps[id] != null) {
					equipmentPUps[id].Enable (power, amount, effectColor);
					activePUp = equipmentPUps[id];
				} else
					DataLogger.LogError ("Equipment pup with id " + id.ToString () + " is null!");
			} else
				DataLogger.LogError ("Equipment pup with id " + id.ToString () + " not enough Equipment pups! length:" + equipmentPUps.Length.ToString ());
		} else if (type == PUpTypes.potion) {
			if (id < potionPUps.Length) {
				if (potionPUps[id] != null) {
					CharacterStuffController.s.lastActivatedButton = false;
					potionPUps[id].Enable (power, amount, effectColor);
					activePUp = potionPUps[id];
				} else
					DataLogger.LogError ("Potion pup with id " + id.ToString () + " is null!");
			} else
				DataLogger.LogError ("Potion pup with id " + id.ToString () + " not enough potion pups! length:" + potionPUps.Length.ToString());
		} else {
			DataLogger.LogError ("Other pup types arent implemented/does not exist: " + type.ToString ());
		}
	}

	public void PowerUpDisabledCallback () {
		CharacterStuffController.s.PowerUpDisabledCallback ();
	}

	public void DisablePowerUps () {
		if(activePUp != null)
		activePUp.Disable ();
	}

	public delegate void Hook (IndividualCard card);
	public Hook activateHook;
	public void ActivateInvoke (IndividualCard card) {
		try{
			if(activateHook != null)
				activateHook.Invoke (card);
		} catch (System.Exception e) {
			DataLogger.LogError (this.name, e);
		}
	}

	public Hook selectHook;
	public void SelectInvoke (IndividualCard card) {
		try{
			if (selectHook != null)
				selectHook.Invoke (card);
		} catch (System.Exception e) {
			DataLogger.LogError (this.name, e);
		}
	}

	public delegate void Hook2 ();
	public Hook2 checkHook;
	public void CheckInvoke () {
		try{
			if (checkHook != null)
				checkHook.Invoke ();
		} catch (System.Exception e) {
			DataLogger.LogError (this.name, e);
		}
	}



	public enum ActionType {Enable, Activate, SelectCard, Disable}

	public void ReceiveEnemyPowerUpActions (int player, int x, int y, PUpTypes type, int id, int power, float amount, ActionType action) {
		IndividualCard card = null;
		try {
			if (x != -1 && y != -1)
				card = CardHandler.s.allCards[x, y];
		} catch {
			DataLogger.LogError ("ReceiveEnemyPowerUpActions couldnt find the card " + x.ToString() + "-" + y.ToString());
		}

		if (type == PUpTypes.equipment) {
			equipmentPUps[id].ReceiveAction (player, card,power,amount, action);
		} else {
			potionPUps[id].ReceiveAction (player, card, power, amount, action);
		}
	}

	//there exists only unselect, so unselect this card if we were selecting it wrongly
	public void ReceiveNetworkCorrection (IndividualCard myCard){
		foreach (PowerUpBase pup in equipmentPUps) {
			pup.ReceiveNetworkCorrection (myCard);
		}
	}

	public void SendPowerUpAction (int x, int y, PUpTypes pUpType, int id, int power, float amount, ActionType action) {
		DataHandler.s.SendPowerUpAction (x, y, pUpType, id, power, amount, action);
	}
}
