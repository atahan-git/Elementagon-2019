using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class DataHandler : MonoBehaviour {

	public static DataHandler s;

	[HideInInspector]
	char myPlayerIdentifier = 'B';
	[HideInInspector]
	public int myPlayerInteger = 0;

	public const int NPCInteger = 3;

	public const char p_blue = 'B';
	public const char p_red = 'R';
	public const char p_green = 'G';
	public const char p_yellow = 'Y';
	public const char p_NPC = 'X';
	public const char p_TeamA = 'A';
	public const char p_TeamE = 'E';


	const char a_player = 'A';
	const char a_cardtype = 'C';
	const char a_power = 'P';
	const char a_npc = 'N';
	const char a_score = 'S';
	const char a_def = 'D';
	const char a_netcorrect = 'F'; //f for fix
	const char a_movplayer = 'M';
	const char a_cardmatcheffect = 'E';
	const char a_objective = 'O';
	const char a_request = 'V';
	const char a_dialogEnd = 'X';

	DataLogger logText;

	// Use this for initialization
	void Awake () {
		try {
			if (s != null && s != this) {
				Destroy (this.gameObject);
				return;
			} else {
				s = this;
			}

			logText = GameObject.FindGameObjectWithTag ("LogText").GetComponent<DataLogger> ();
			//DataLogger.text = "getting started";

			GetPlayerIdentity ();
			//DataLogger.text = "identity received " + myPlayerIdentifier.ToString();
			GoogleAPI.s.myReceiver = ReceiveData;
		} catch (Exception e) {
			DataLogger.LogError (this.name, e);
		}
	}

	void Update () {
		try {
			if (logText == null) {
				logText = DataLogger.s;
			}
		} catch { }
		//DataLogger.LogMessage(GetSelf ().ParticipantId.ToString();
	}

	void GetPlayerIdentity () {
		try {
			if (GoogleAPI.s == null) {
				GoogleAPI.s = GameObject.FindObjectOfType<GoogleAPI> ();
			}

			if (!GoogleAPI.s.gameInProgress)
				return;

			try {
				myPlayerInteger = (int)GoogleAPI.s.participants.IndexOf (GoogleAPI.s.GetSelf ());
			} catch {
				myPlayerInteger = 0;
			}
			try {
				DataLogger.LogMessage (myPlayerInteger.ToString ());
			} catch {
			}

			switch (myPlayerInteger) {
			case 0:
				myPlayerIdentifier = p_blue;
				break;
			case 1:
				myPlayerIdentifier = p_red;
				break;
			case 2:
				myPlayerIdentifier = p_green;
				break;
			case 3:
				myPlayerIdentifier = p_yellow;
				break;
			}
		} catch (Exception e) {
			DataLogger.LogError (this.name, e);
		}
	}


	public char toChar (int integer) {
		switch (integer) {
		case 0:
			return p_blue;
		case 1:
			return p_red;
		case 2:
			return p_green;
		case 3:
			if (!GS.a.isNPCEnabled)
				return p_yellow;
			else
				return p_NPC;
		case 4:
			return p_TeamA;
		case 5:
			return p_TeamE;
		}

		return p_blue;
	}

	public int toInt (char character) {
		switch (character) {
		case p_blue:
			return 0;
		case p_red:
			return 1;
		case p_green:
			return 2;
		case p_yellow:
		case p_NPC:
			return 3;
		case p_TeamA:
			return 4;
		case p_TeamE:
			return 5;
		}

		return 0;
	}




	//----------------------------------------------------------Send Data Commands




	const char player_select = 'S'; //CardHandler.CardActions.Select;
	const char player_unselect = 'U';   //CardHandler.CardActions.UnSelect;
	const char player_match = 'M';  //CardHandler.CardActions.Match;
	public void SendPlayerAction (int x, int y, CardHandler.CardActions action, int x2, int y2) {

		List<byte> toSend = new List<byte> ();

		toSend.AddRange (System.BitConverter.GetBytes ((short)x));
		toSend.AddRange (System.BitConverter.GetBytes ((short)y));

		switch (action) {
		case CardHandler.CardActions.Select:
			toSend.Add ((byte)player_select);
			break;
		case CardHandler.CardActions.UnSelect:
			toSend.Add ((byte)player_unselect);
			break;
		case CardHandler.CardActions.Match:
			toSend.Add ((byte)player_match);
			break;
		default:
			toSend.Add ((byte)a_def);
			break;
		}

		if (x2 != -1) {
			toSend.AddRange (System.BitConverter.GetBytes ((short)x2));
			toSend.AddRange (System.BitConverter.GetBytes ((short)y2));
		}

		SendData (a_player, toSend.ToArray ());
	}
	public void SendPlayerAction (int x, int y, CardHandler.CardActions action) { SendPlayerAction (x,y,action,-1,-1); }


	public void SendCardType (int x, int y, int type) {

		try {
			//DataLogger.LogMessage("Sending card type initiate");

			List<byte> toSend = new List<byte> ();
			toSend.AddRange (System.BitConverter.GetBytes ((short)x));
			toSend.AddRange (System.BitConverter.GetBytes ((short)y));
			toSend.AddRange (System.BitConverter.GetBytes ((short)type));


			//DataLogger.LogMessage("Sending card type list made");
			SendData (a_cardtype, toSend.ToArray ());
		} catch (Exception e) {
			DataLogger.LogMessage (e.Message + " - " + e.StackTrace);
		}
	}


	public void SendCardType (int targetPlayer,int x, int y, int type) {

		try {
			//DataLogger.LogMessage("Sending card type initiate");

			List<byte> toSend = new List<byte> ();
			toSend.AddRange (System.BitConverter.GetBytes ((short)x));
			toSend.AddRange (System.BitConverter.GetBytes ((short)y));
			toSend.AddRange (System.BitConverter.GetBytes ((short)type));


			//DataLogger.LogMessage("Sending card type list made");
			SendData (targetPlayer, a_cardtype, toSend.ToArray ());
		} catch (Exception e) {
			DataLogger.LogMessage (e.Message + " - " + e.StackTrace);
		}
	}


	const char power_equipment = 'E';  //PowerUpManager.PUpTypes.equipment;
	const char power_potion = 'P';  //PowerUpManager.PUpTypes.potion;

	const char power_activate = 'A';    //PowerUpManager.ActionType.Activate;
	const char power_disable = 'D'; //PowerUpManager.ActionType.Disable;
	const char power_enable = 'Y';  //PowerUpManager.ActionType.Enable;
	const char power_select = 'T';  //PowerUpManager.ActionType.SelectCard;
	public void SendPowerUpAction (int x, int y, PowerUpManager.PUpTypes type, int id, int power, float amount, PowerUpManager.ActionType action) {

		List<byte> toSend = new List<byte> ();

		toSend.AddRange (System.BitConverter.GetBytes ((short)x));
		toSend.AddRange (System.BitConverter.GetBytes ((short)y));

		switch (type) {
		case PowerUpManager.PUpTypes.equipment:
			toSend.Add ((byte)power_equipment);
			break;
		case PowerUpManager.PUpTypes.potion:
			toSend.Add ((byte)power_potion);
			break;
		default:
			DataLogger.LogError ("Unknown power up type: " + type.ToString () + " can't send multiplayer data");
			return;
		}

		toSend.AddRange (System.BitConverter.GetBytes ((short)id));
		toSend.AddRange (System.BitConverter.GetBytes ((short)power));
		toSend.AddRange (System.BitConverter.GetBytes (amount));

		switch (action) {
		case PowerUpManager.ActionType.Activate:
			toSend.Add ((byte)power_activate);
			break;
		case PowerUpManager.ActionType.Disable:
			toSend.Add ((byte)power_disable);
			break;
		case PowerUpManager.ActionType.Enable:
			toSend.Add ((byte)power_enable);
			break;
		case PowerUpManager.ActionType.SelectCard:
			toSend.Add ((byte)power_select);
			break;
		default:
			DataLogger.LogError ("Unknown action type: " + action.ToString () + " can't send multiplayer data");
			return;
		}



		SendData (a_power, toSend.ToArray ());
	}


	const char npc_spawn = 'S';    //NPCManager.ActionType.Spawn;
	const char npc_goto = 'G'; //NPCManager.ActionType.GoToPos;
	const char npc_select = 'C';  //NPCManager.ActionType.SelectCard;
	const char npc_activate = 'A';  //NPCManager.ActionType.Activate;
	const char npc_die = 'D';  //NPCManager.ActionType.Die;
	public void SendNPCAction (int x, int y, int index, NPCManager.ActionType action, int data) {

		List<byte> toSend = new List<byte> ();

		toSend.AddRange (System.BitConverter.GetBytes ((short)x));
		toSend.AddRange (System.BitConverter.GetBytes ((short)y));

		toSend.AddRange (System.BitConverter.GetBytes ((short)index));
		toSend.AddRange (System.BitConverter.GetBytes ((short)data));

		switch (action) {
		case NPCManager.ActionType.Spawn:
			toSend.Add ((byte)npc_spawn);
			break;
		case NPCManager.ActionType.GoToPos:
			toSend.Add ((byte)npc_goto);
			break;
		case NPCManager.ActionType.SelectCard:
			toSend.Add ((byte)npc_select);
			break;
		case NPCManager.ActionType.Activate:
			toSend.Add ((byte)npc_activate);
			break;
		case NPCManager.ActionType.Die:
			toSend.Add ((byte)npc_die);
			break;
		default:
			DataLogger.LogError ("Unknown action type: " + action.ToString () + " can't send multiplayer data");
			return;
		}



		SendData (a_npc, toSend.ToArray ());
	}


	public void SendScore (char player, int scoreType, int totalScore, bool isDelayed) {
		try {
			List<byte> toSend = new List<byte> ();
			toSend.Add ((byte)player);
			toSend.AddRange (System.BitConverter.GetBytes ((short)scoreType));
			toSend.AddRange (System.BitConverter.GetBytes ((short)totalScore));
			byte boolByte = isDelayed ? (byte)0x01 : (byte)0x00;
			toSend.Add ((byte)boolByte);

			SendData (a_score, toSend.ToArray ());
		} catch (Exception e) {
			DataLogger.LogError (this.name, e);
		}
	}


	public enum cardStates { open, close, matched };
	const char cs_open = 'O';
	const char cs_close = 'C';
	const char cs_match = 'M';
	//In case there is a conflict in the blue's game state, a network correction for the conflicting card will be send to everyone.
	//this new state will be enforced onto other game systems
	public void NetworkCorrection (IndividualCard myCard) {

		//only player 0 can make corrections to evade further conflicts
		if (DataHandler.s.myPlayerInteger != 0) {
			return;
		}

		int x = (int)myCard.x;
		int y = (int)myCard.y;
		int type = (int)myCard.cBase.dynamicCardID;
		int selectEffectID = (int)myCard.selectEffectID;
		cardStates state = myCard.cardState;

		try {

			List<byte> toSend = new List<byte> ();
			toSend.AddRange (System.BitConverter.GetBytes ((short)x));
			toSend.AddRange (System.BitConverter.GetBytes ((short)y));
			toSend.AddRange (System.BitConverter.GetBytes ((short)type));
			toSend.AddRange (System.BitConverter.GetBytes ((short)selectEffectID));

			switch (state) {
			case cardStates.open:
				toSend.Add ((byte)cs_open);
				break;
			case cardStates.close:
				toSend.Add ((byte)cs_close);
				break;
			case cardStates.matched:
				toSend.Add ((byte)cs_match);
				break;
			default:
				toSend.Add ((byte)a_def);
				break;
			}


			SendData (a_netcorrect, toSend.ToArray ());
		} catch (Exception e) {
			DataLogger.LogError (this.name, e);
		}
	}


	public void SendMovementAction (char player, int x, int y) {

		try {
			List<byte> toSend = new List<byte> ();
			toSend.Add ((byte)player);
			toSend.AddRange (System.BitConverter.GetBytes ((short)x));
			toSend.AddRange (System.BitConverter.GetBytes ((short)y));

			SendData (a_movplayer, toSend.ToArray ());
		} catch (Exception e) {
			DataLogger.LogError (this.name, e);
		}
	}


	/*public void SendCardMatchEffectAction (int myPlayer, int x1, int y1, int x2, int y2) {
		char player = toChar (myPlayer);

		try {
			List<byte> toSend = new List<byte> ();
			toSend.Add ((byte)player);
			toSend.AddRange (System.BitConverter.GetBytes ((short)x1));
			toSend.AddRange (System.BitConverter.GetBytes ((short)y1));
			toSend.AddRange (System.BitConverter.GetBytes ((short)x2));
			toSend.AddRange (System.BitConverter.GetBytes ((short)y2));

			SendData (a_cardmatcheffect, toSend.ToArray ());
		} catch (Exception e) {
			DataLogger.LogError (this.name, e);
		}
	}*/


	public void SendObjectiveAction (bool isOn, int cardType, int objectiveType) {
		try {
			List<byte> toSend = new List<byte> ();
			byte boolByte = isOn ? (byte)0x01 : (byte)0x00;
			toSend.Add ((byte)boolByte);
			toSend.AddRange (System.BitConverter.GetBytes ((short)cardType));
			toSend.AddRange (System.BitConverter.GetBytes ((short)objectiveType));

			SendData (a_objective, toSend.ToArray ());
		} catch (Exception e) {
			DataLogger.LogError (this.name, e);
		}
	}


	public enum RequestTypes{CardTypeRequest}
	const char v_cardTypeRequest = 'R';
	public void SendRequest (RequestTypes reqType) {

		char sendChar = v_cardTypeRequest;

		/*switch (variant) {
		case 0:
			sendChar = v_tap;
			break;
		case 1:
			sendChar = v_slide;
			break;
		case -1:
			sendChar = v_cardTypeRequest;
			break;
		default:
			sendChar = v_tap;
			break;
		}*/

		try {
			List<byte> toSend = new List<byte> ();
			toSend.Add ((byte)sendChar);

			SendData (a_request, toSend.ToArray ());
		} catch (Exception e) {
			DataLogger.LogError (this.name, e);
		}
	}

	public void SendDialogEnd () {
		try {
			SendData (a_dialogEnd, new byte[0]);
		} catch (Exception e) {
			DataLogger.LogError (this.name, e);
		}
	}

	public void SendData (char command, byte[] data){
		try {
			List<byte> toSend = new List<byte> ();
			toSend.Add ((byte)command);
			toSend.Add ((byte)myPlayerIdentifier);
			foreach (byte b in data) {
				toSend.Add (b);
			}

			//DataLogger.LogMessage("Sending data " + command.ToString ());

			GoogleAPI.s.SendMessage (toSend.ToArray ());
		} catch (Exception e) {
			DataLogger.LogError (this.name, e);
		}
	}

	public void SendData (int targetPlayer,char command, byte[] data) {
		try {
			List<byte> toSend = new List<byte> ();
			toSend.Add ((byte)command);
			toSend.Add ((byte)myPlayerIdentifier);
			foreach (byte b in data) {
				toSend.Add (b);
			}

			//DataLogger.LogMessage("Sending data " + command.ToString ());

			GoogleAPI.s.SendMessage (targetPlayer, toSend.ToArray ());
		} catch (Exception e) {
			DataLogger.LogError (this.name, e);
		}
	}


	//----------------------------------------------------------Receive Data Handlers
























	//----------------------------------------------------------Receive Data Handlers



	public void ReceiveData (byte[] data){
		try {
			char myCommand = (char)data[0];

			switch (myCommand) {
			case a_player:
				ReceivePlayerAction (data);
				break;
			case a_cardtype:
				ReceiveCardType (data);
				break;
			case a_power:
				ReceivePowerUpAction (data);
				break;
			case a_npc:
				ReceiveNPCAction (data);
				break;
			case a_score:
				ReceiveScore (data);
				break;
			case a_netcorrect:
				ReceiveNetworkCorrection(data);
				break;
			case a_movplayer:
				ReceiveMovementAction(data);
				break;
			/*case a_cardmatcheffect:

				break;*/
			case a_objective:
				ReceiveObjectiveAction(data);
				break;
			case a_request:
				ReceiveRequest (data);
				break;
			case a_dialogEnd:
				ReceiveDialogEnd (data);
				break;
			default:
				DataLogger.LogError ("Unknown command " + myCommand.ToString ());
				break;
			}

			//DataLogger.LogMessage ("Data processing done " + myCommand.ToString ());
		} catch (Exception e){
			//DataLogger.LogMessage ("Data processing failed " + myCommand.ToString (), true);
			DataLogger.LogError (this.name, e);
		}
	}

	void ReceivePlayerAction (byte[] data){
		char player;
		int x, y, x2, y2;
		CardHandler.CardActions action;

		player = (char)data [1];
		x = System.BitConverter.ToInt16 (data, 2);
		y = System.BitConverter.ToInt16 (data, 2 + 2);
		char cardAction = (char)data [2 + 2 + 2];

		switch (cardAction) {
		case  player_select:
			action = CardHandler.CardActions.Select;
			break;
		case  player_unselect:
			action = CardHandler.CardActions.UnSelect;
			break;
		case  player_match:
			action = CardHandler.CardActions.Match;
			break;
		default:
			DataLogger.LogError ("Unknown card action");
			action = CardHandler.CardActions.UnSelect;
			break;
		}


		x2 = System.BitConverter.ToInt16 (data, 2 + 2 + 2 + 1);
		y2 = System.BitConverter.ToInt16 (data, 2 + 2 + 2 + 1 + 2);

		EnemyPlayerHandler.s.ReceiveAction (player, x, y, action, x2,y2);
	}

	void ReceiveCardType (byte[] data){
		int x;
		int y;
		int type;

		x = System.BitConverter.ToInt16 (data, 2);
		y = System.BitConverter.ToInt16 (data, 2 + 2);
		type = System.BitConverter.ToInt16 (data, 2 + 2 + 2);


		CardHandler.s.ReceiveCardType (x, y, type);
	}

	void ReceivePowerUpAction (byte[] data){

		int player = 4;
		char playerChar;
		int x;
		int y;
		PowerUpManager.PUpTypes type;
		int id;
		int power;
		float amount;
		PowerUpManager.ActionType action;


		playerChar = (char)data [1];

		switch (playerChar) {
		case p_blue:
			player = 0;
			break;
		case p_red:
			player = 1;
			break;
		case p_green:
			player = 2;
			break;
		case p_yellow:
			player = 3;
			break;
		default:
			DataLogger.LogError ("Unknown player char");
			break;
		}


		x = System.BitConverter.ToInt16 (data, 2);
		y = System.BitConverter.ToInt16 (data, 2 + 2);
		char pType = (char)data [2 + 2 + 2];

		switch (pType) {
		case  power_equipment:
			type = PowerUpManager.PUpTypes.equipment;
			break;
		case  power_potion:
			type = PowerUpManager.PUpTypes.potion;
			break;
		default:
			DataLogger.LogError ("Unknown power up action");
			type = PowerUpManager.PUpTypes.equipment;
			break;
		}

		id = System.BitConverter.ToInt16 (data, 2 + 2 + 2 + 1);
		power = System.BitConverter.ToInt16 (data, 2 + 2 + 2 + 1 + 2);
		amount = System.BitConverter.ToSingle (data, 2 + 2 + 2 + 1 + 2 + 2);


		char pAction = (char)data[2 + 2 + 2 + 1 + 2 + 2 + 4];

		switch (pAction) {
		case  power_activate:
			action = PowerUpManager.ActionType.Activate;
			break;
		case  power_disable:
			action = PowerUpManager.ActionType.Disable;
			break;
		case  power_enable:
			action = PowerUpManager.ActionType.Enable;
			break;
		case  power_select:
			action = PowerUpManager.ActionType.SelectCard;
			break;
		default:
			//Error
			action = PowerUpManager.ActionType.Disable;
			break;
		}

		PowerUpManager.s.ReceiveEnemyPowerUpActions (player, x, y, type, id, power, amount, action);
	}

	void ReceiveNPCAction (byte[] data) {

		int player = 4;
		char playerChar;
		int x;
		int y;
		int index;
		int _data;
		NPCManager.ActionType action;


		playerChar = (char)data[1];

		switch (playerChar) {
		case p_blue:
			player = 0;
			break;
		case p_red:
			player = 1;
			break;
		case p_green:
			player = 2;
			break;
		case p_yellow:
			player = 3;
			break;
		default:
			DataLogger.LogError ("Unknown player char");
			break;
		}


		x = System.BitConverter.ToInt16 (data, 2);
		y = System.BitConverter.ToInt16 (data, 2 + 2);
		index = System.BitConverter.ToInt16 (data, 2 + 2 + 2);
		_data = System.BitConverter.ToInt16 (data, 2 + 2 + 2 + 2);
		char pAction = (char)data[2 + 2 + 2 + 2 + 2];
		
		switch (pAction) {
		case npc_spawn:
			action = NPCManager.ActionType.Activate;
			break;
		case npc_goto:
			action = NPCManager.ActionType.GoToPos;
			break;
		case npc_select:
			action = NPCManager.ActionType.SelectCard;
			break;
		case npc_activate:
			action = NPCManager.ActionType.Activate;
			break;
		case npc_die:
			action = NPCManager.ActionType.Die;
			break;
		default:
			//Error
			action = NPCManager.ActionType.Die;
			DataLogger.LogError ("Unknow NPC action type detected: " + pAction.ToString ());
			break;
		}

		NPCManager.s.ReceiveNPCAction (x, y, index, action, _data);
	}

	public void ReceiveScore (byte [] data) {
		char player;
		int scoreType;
		int totalScore;
		bool isDelayed;

		player = (char)data [2];
		scoreType = System.BitConverter.ToInt16 (data, 3);
		totalScore = System.BitConverter.ToInt16 (data, 3 + 2);
		isDelayed = (byte)data [3 + 2 + 1] == 0x01 ? true : false;

		ScoreBoardManager.s.ReceiveScore (player, scoreType, totalScore, isDelayed);
	}

	public void ReceiveMovementAction (byte[] data){
		int player = 0;
		char playerChar;
		int x;
		int y;

		playerChar = (char)data [2];

		switch (playerChar) {
		case p_blue:
			player = 0;
			break;
		case p_red:
			player = 1;
			break;
		case p_green:
			player = 2;
			break;
		case p_yellow:
			player = 3;
			break;
		default:
			DataLogger.LogError ("Unknown player char");
			break;
		}


		x = System.BitConverter.ToInt16 (data, 3);
		y = System.BitConverter.ToInt16 (data, 3 + 2);

		EnemyPlayerHandler.s.ReceiveMovementAction (player, x, y);
	}

	/*public void ReceiveCardMatchEffectAction (byte [] data) {
		int player = 0;
		char playerChar;
		int x1,x2;
		int y1, y2;

		playerChar = (char)data [2];

		switch (playerChar) {
		case p_blue:
			player = 0;
			break;
		case p_red:
			player = 1;
			break;
		case p_green:
			player = 2;
			break;
		case p_yellow:
			player = 3;
			break;
		default:
			DataLogger.LogError ("Unknown player char");
			break;
		}


		x1 = System.BitConverter.ToInt16 (data, 3);
		y1 = System.BitConverter.ToInt16 (data, 3 + 2);
		x2 = System.BitConverter.ToInt16 (data, 3 + 2 + 2);
		y2 = System.BitConverter.ToInt16 (data, 3 + 2 + 2 + 2);

		CardMatchCoolEffect.s.ReceiveNetworkMatchTwo (player, 
			CardHandler.s.allCards [x1, y1], CardHandler.s.allCards [x2, y2]);
	}*/


	public void ReceiveObjectiveAction (byte[] data){
		bool isOn;
		int cardType;
		int objectiveType;

		isOn = (byte)data [2] == 0x01 ? true : false;


		cardType = System.BitConverter.ToInt16 (data, 3);
		objectiveType = System.BitConverter.ToInt16 (data, 3 + 2);

		//ObjectivesSystem.s.ReceiveNetworkObjectiveAction (isOn, cardType, objectiveType);
	}


	public void ReceiveNetworkCorrection (byte[] data){
		int x;
		int y;
		int type;
		int selectEffectID;
		cardStates state;

		x = System.BitConverter.ToInt16 (data, 2);
		y = System.BitConverter.ToInt16 (data, 2 + 2);
		type = System.BitConverter.ToInt16 (data, 2 + 2 + 2);
		selectEffectID = System.BitConverter.ToInt16 (data, 2 + 2 + 2 + 2);

		char cState = (char)data [2 + 2 + 2 + 2 + 2];

		switch (cState) {
		case cs_open:
			state = cardStates.open;
			break;
		case cs_close:
			state = cardStates.close;
			break;
		case cs_match:
			state = cardStates.matched;
			break;
		default:
			state = cardStates.close;
			break;
		}

		IndividualCard myCard = null;

		try{
			myCard = CardHandler.s.allCards [x, y];

			//these will unselect the card from respective places
			LocalPlayerController.s.ReceiveNetworkCorrection(myCard);
			PowerUpManager.s.ReceiveNetworkCorrection(myCard);
			NPCManager.s.ReceiveNetworkCorrection (myCard);

		} catch (Exception e) {
			DataLogger.LogMessage (e.Message + " - " + e.StackTrace);
		}

		if (myCard != null) {
			//after unselection is done, we will update the card type to be correct
			CardHandler.s.UpdateCardType (x, y, type);

			switch (state) {
			case cardStates.open:
				myCard.SelectCard (-1);
				if (selectEffectID > 0) {
					myCard.DestroySelectedEfect ();
					myCard.selectedEffect = (GameObject)Instantiate (GS.a.gfxs.selectEffects[selectEffectID].gameObject, myCard.transform.position, Quaternion.identity);
				}
				break;
			case cardStates.close:
				myCard.UnSelectCard ();
				break;
			case cardStates.matched:
				myCard.NetworkCorrectMatch ();
				break;
			default:
				myCard.UnSelectCard ();
				break;
			}
		}
	}

	public void ReceiveRequest (byte[] data){

		char playerChar = (char)data[1];
		char dataChar = (char)data [2];

		int playerInt = toInt (playerChar);


		if (myPlayerInteger == 0) {
			CardHandler.s.SendCardTypesAgain (playerInt);
		}
		/*int variant = 0;

		switch (dataChar) {
		case v_tap:
			variant = 0;
			break;
		case v_slide:
			variant = 1;
			break;
		case v_cardTypeRequest:
			if (myPlayerInteger == 0)
				CardHandler.s.InitializeFirstStartingCards ();
			else
				SendGameVariant (-1);
			break;
		default:
			variant = 0;
			break;
		}*/


	}

	void ReceiveDialogEnd (byte[] data) {
		int player = 0;
		char playerChar;

		playerChar = (char)data[1];

		switch (playerChar) {
		case p_blue:
			player = 0;
			break;
		case p_red:
			player = 1;
			break;
		case p_green:
			player = 2;
			break;
		case p_yellow:
			player = 3;
			break;
		default:
			DataLogger.LogError ("Unknown player char");
			break;
		}

		GameStarter.s.ReceivePlayerReady (player);
	}
}
