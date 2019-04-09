using UnityEngine;
using System.Collections;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.Multiplayer;
using UnityEngine.SocialPlatforms;
using System.Linq;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using GooglePlayGames.BasicApi.SavedGame;
using System;

public class GoogleAPI : MonoBehaviour, RealTimeMultiplayerListener {

	public static int playerCount = 2;
	public static int gameMode = 0;

	public static GoogleAPI s;
	public List<Participant> participants = new List<Participant>();

	public DataLogger logText;

	public bool canPlay = false;

	public bool gameInProgress = false;
	public bool searchingForGame = false;

	// Use this for initialization
	void Awake () 
	{
		if (s != null && s != this) {
			Destroy (this.gameObject);
			return;
		} else {
			s = this;
		}

		Application.targetFrameRate = 45;

		DontDestroyOnLoad (this.gameObject);
	}

	void Start () 
	{	
		logText = DataLogger.s;

		//RTClient = PlayGamesPlatform.Instance.RealTime;
		//lobbyGUI.SetActive (false);
		//ChangePlayerCount (0);

		DataLogger.LogMessage("Initialising");
		try{
		PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
				.WithInvitationDelegate(ReceiveInvitaion)
				.EnableSavedGames()
				.Build();

		PlayGamesPlatform.InitializeInstance(config);
		PlayGamesPlatform.DebugLogEnabled = true;
		//DataLogger.LogMessage("debug enabled");
		PlayGamesPlatform.Activate();

		//GameObject.FindObjectOfType<MPMenu> ().GetComponent<MPMenu> ().GirisYapildiMi (Social.localUser.authenticated);
		}catch{
			DataLogger.LogMessage ("Initialization Failure, Please Restart the Game");
		}
		DataLogger.LogMessage("Initialization Successful");

		if (Application.internetReachability != NetworkReachability.NotReachable) {
			if (!PlayGamesPlatform.Instance.localUser.authenticated) {
				Login ();
			}
		}

		//Invoke("ShowSelectUI",2f);
	}
	

	public List<Participant> GetParticipants()
	{
		return PlayGamesPlatform.Instance.RealTime.GetConnectedParticipants ();
	}


	public Participant GetPbyID (string participantID) 
	{
		return PlayGamesPlatform.Instance.RealTime.GetParticipant (participantID);
	}

	public Participant GetSelf ()
	{
		return PlayGamesPlatform.Instance.RealTime.GetSelf ();
	}

	int n= 1;
	bool isLoggingin = false;
	public void Login () {
		if (isLoggingin)
			return;

		PlayGamesPlatform.Instance.Authenticate ((bool success) => {
			if (success) {
				DataLogger.LogMessage ("Login Successful");
				canPlay = true;
				VC_MultiplayerMenuController.s.UpdateMenu ();
			} else {
				if (Application.internetReachability != NetworkReachability.NotReachable) {
					DataLogger.LogMessage ("Login attempt " + n.ToString ());
					n++;
					//Invoke ("Login", 0.5f);
				} else {
					DataLogger.LogMessage ("No Internet Access");
				}
				isOnline = false;
				canPlay = false;
			}
		}, false);

		if (PlayGamesPlatform.Instance.localUser.authenticated) {
			canPlay = true;
		}
		isLoggingin = false;
	}


	public void GetQuickMatch (int GameVariant) {
		if (canPlay) {
			isOnline = true;
			canPlay = false;
			searchingForGame = true;
			VC_MultiplayerMenuController.SetSearchingPanelState (true);
			gameInProgress = false;
			DataLogger.LogMessage ("Initiating Search");
			PlayGamesPlatform.Instance.RealTime.ShowWaitingRoomUI ();
			//sInstance = new MultiPlayerConnect();
			//const int MinOpponents = 1, MaxOpponents = 1;
			try {
				gameMode = GameVariant;
				PlayGamesPlatform.Instance.RealTime.CreateQuickGame ((uint)(playerCount - 1), (uint)(playerCount - 1), (uint)GameVariant, this);
				if (!showingWaitingRoom) {
					PlayGamesPlatform.Instance.RealTime.ShowWaitingRoomUI ();
					showingWaitingRoom = true;
				}
			} catch {
				DataLogger.LogMessage ("Game Search Failed");
				canPlay = true;
			}
			//DataLogger.LogMessage("Searching for Room";
		} else {
			if (!isOnline)
				Login ();
		}
		VC_MultiplayerMenuController.s.UpdateMenu ();
	}

	public void GetInvitationMatch (int GameVariant) {
		if (canPlay) {
			isOnline = true;
			canPlay = false;
			searchingForGame = true;
			VC_MultiplayerMenuController.SetSearchingPanelState (true);
			gameInProgress = false;
			DataLogger.LogMessage ("Initiating Invitation");
			PlayGamesPlatform.Instance.RealTime.ShowWaitingRoomUI ();
			//sInstance = new MultiPlayerConnect();
			//const int MinOpponents = 1, MaxOpponents = 1;
			try {
				gameMode = GameVariant;
				PlayGamesPlatform.Instance.RealTime.CreateWithInvitationScreen ((uint)(playerCount - 1), (uint)(playerCount - 1), (uint)GameVariant, this);
				if (!showingWaitingRoom) {
					PlayGamesPlatform.Instance.RealTime.ShowWaitingRoomUI();
					showingWaitingRoom = true;
				}
			} catch {
				DataLogger.LogMessage ("Invitation Failed");
				canPlay = true;
			}
			//DataLogger.LogMessage("Searching for Room";
		}
		VC_MultiplayerMenuController.s.UpdateMenu ();
	}

	public void AcceptInvitation (){
		PlayGamesPlatform.Instance.RealTime.AcceptFromInbox (this);
	}

	Invitation mIncomingInvitation = null;
	public void ReceiveInvitaion (Invitation invitation, bool shouldAutoAccept){
		DataLogger.LogMessage ("Received Invitation");
		shouldAutoAccept = true;
		if (shouldAutoAccept) {
			isOnline = true;
			canPlay = false;
			searchingForGame = true;
			VC_MultiplayerMenuController.SetSearchingPanelState (true);
			gameInProgress = false;
			gameMode = invitation.Variant;
			if (gameMode == -1)
				gameMode = 0;
			PlayGamesPlatform.Instance.RealTime.AcceptInvitation(invitation.InvitationId, this);
			if (!showingWaitingRoom) {
				PlayGamesPlatform.Instance.RealTime.ShowWaitingRoomUI();
				showingWaitingRoom = true;
			}
		} else {
			// The user has not yet indicated that they want to accept this invitation.
			// We should *not* automatically accept it. Rather we store it and 
			// display an in-game popup:
			mIncomingInvitation = invitation;
		}
	}


	public void CancelMatchSearch (){
		PlayGamesPlatform.Instance.RealTime.LeaveRoom ();
		DataLogger.LogMessage ("Game Search Canceled");

		//Login ();
		VC_MultiplayerMenuController.s.UpdateMenu ();
	}

	public void LeaveGame () {
		try {
			PlayGamesPlatform.Instance.RealTime.LeaveRoom ();
		} catch {
			OnLeftRoom ();
		}
	}


	bool oldOn = false;
	void Update () {
		if (logText == null) {
			logText = DataLogger.s;
		}
		/*if (isOnline) {
			if (!PlayGamesPlatform.Instance.RealTime.IsRoomConnected ())
				DataLogger.LogMessage ("Room not connected", true);
		}
		if (Application.internetReachability == NetworkReachability.NotReachable)
			DataLogger.LogMessage ("no internet",true);
			*/
		//DataLogger.LogMessage(GetSelf ().ParticipantId.ToString();
		isOnline = PlayGamesPlatform.Instance.IsAuthenticated();

		if (oldOn != isOnline) {
			oldOn = isOnline;
			VC_MultiplayerMenuController.s.UpdateMenu ();
		}

		if (Input.GetKeyDown (KeyCode.Escape)) {
			/*DataLogger.LogError ("Escape pressed");
			DataLogger.LogMessage ("Escape pressed");
			print ("escape pressed");*/
		}
	}

	public void OnRoomConnected(bool success) {
		DataLogger.LogMessage("OnRoomConnected");
		if (success) 
		{
			gameInProgress = true;
			searchingForGame = false;
			VC_MultiplayerMenuController.SetSearchingPanelState (true);
			DataLogger.LogMessage("Room Connection Successful");
			participants = GetParticipants ();
			SceneMaster.s.LoadPlayingLevel (gameMode);
		} else 
		{
			DataLogger.LogMessage("Room Connection Failure");
			PlayGamesPlatform.Instance.RealTime.LeaveRoom ();
		}
	}


	private bool showingWaitingRoom = false;

	public void OnRoomSetupProgress(float progress) {
		// show the default waiting room.
		

		if (progress == 20) {
			if (!showingWaitingRoom) {
				DataLogger.LogMessage ("Starting game");
				showingWaitingRoom = true;
				PlayGamesPlatform.Instance.RealTime.ShowWaitingRoomUI ();
			}
		} else {
			DataLogger.LogMessage ("Connecting " + ((int)progress).ToString () + "%");
		}

		/*if (progress == 20) {
			DataLogger.LogMessage("Starting game");
			PlayGamesPlatform.Instance.RealTime.ShowWaitingRoomUI();
		} else {
			DataLogger.LogMessage("Connecting " + ((int)progress).ToString() + "%");
		}*/
	}

	public void OnParticipantLeft (Participant participant) {
		DataLogger.LogMessage(participant.DisplayName + " Left");
		PlayGamesPlatform.Instance.RealTime.LeaveRoom ();
	}

	public void OnPeersConnected(string[] participantIds) {
		foreach (string participant in participantIds) {
			DataLogger.LogMessage(participant + " Joined");
		}
		//PlayGamesPlatform.Instance.RealTime.LeaveRoom ();
	}

	public void OnPeersDisconnected(string[] participantIds) {
		foreach (string participant in participantIds) {
			DataLogger.LogMessage(participant + " Disconnected");
		}
		PlayGamesPlatform.Instance.RealTime.LeaveRoom ();
	}

	public void Exit () {
		PlayGamesPlatform.Instance.SignOut();
	}

	public void OnLeftRoom() {
		DataLogger.LogMessage("Left Room");

		showingWaitingRoom = false;
		canPlay = true;

		if (SceneMaster.GetSceneID () != SceneMaster.menuId) {
			if (GameObjectiveFinishChecker.s != null)
				GameObjectiveFinishChecker.s.DisconnectedFromGame ();
			else
				SceneMaster.s.LoadMenu ();
		} else {
			VC_MultiplayerMenuController.SetSearchingPanelState (false);
		}

		searchingForGame = false;

		// display error message and go back to the menu screen
		gameInProgress = false;

		if (PlayGamesPlatform.Instance != null) {
			if (PlayGamesPlatform.Instance.localUser.authenticated) {
				canPlay = true;
			}

			//Login ();
			//MultiplayerMenuController.s.UpdateMenu ();
		} 

		// (do NOT call PlayGamesPlatform.Instance.RealTime.LeaveRoom() here --
		// you have already left the room!)
	}




	public delegate void ReceiveMessage (byte[]data);
	public ReceiveMessage myReceiver;

	public void OnRealTimeMessageReceived(bool isReliable, string senderId, byte[] data) {
		DataLogger.LogMessage("Data received " + ((char)data [0]).ToString ());
		try{
			DataHandler.s.ReceiveData(data);
			//DataLogger.LogMessage("Data processing begun " + ((char)data [0]).ToString());
		} catch (Exception e) {
			//DataLogger.LogMessage ("Data processing failed " + myCommand.ToString (), true);
			DataLogger.LogError ("Data process failed " + ((char)data[0]).ToString (),e);
		}
	}

	public bool isOnline = false;
	public void SendMessage (byte[] data){
		if (isOnline) {
			try {
				PlayGamesPlatform.Instance.RealTime.SendMessageToAll (true, data);
				DataLogger.LogMessage ("Data send " + ((char)data [0]).ToString ());
			} catch (System.Exception e) {
				DataLogger.LogError ("Data failed to send " + ((char)data[0]).ToString () ,e);
			}
		} else {
			//DataLogger.LogMessage ("We are offline",true);
		}
	}

	public void SendMessage (int playerID,byte[] data) {
		if (isOnline) {
			try {
				PlayGamesPlatform.Instance.RealTime.SendMessage (true, GetParticipants ()[playerID].ParticipantId, data);
				DataLogger.LogMessage ("Data send to player: " + playerID.ToString() + " - " + ((char)data[0]).ToString ());
			} catch (System.Exception e) {
				DataLogger.LogError ("Data failed to send " + ((char)data[0]).ToString (), e);
			}
		} else {
			//DataLogger.LogMessage ("We are offline",true);
		}
	}


	/*
	 * 
	 * 
	 * 
	 * 
	 * 	GAME SAVING AND LOADING STUFF DOWN HERE
	 * 
	 * 
	 * 
	 * 
	 */



	void ShowSelectUI() {
		DataLogger.LogError ("show is called");
		int maxNumToDisplay = 5;
		bool allowCreateNew = false;
		bool allowDelete = true;

		try {
			ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
			savedGameClient.ShowSelectSavedGameUI ("Select saved game",
				(uint)maxNumToDisplay,
				allowCreateNew,
				allowDelete,
				OnSavedGameSelected);
		} catch (System.Exception e) {
			DataLogger.LogError (this.name, e);
		}
	}


	public void OnSavedGameSelected (SelectUIStatus status, ISavedGameMetadata game) {
		if (status == SelectUIStatus.SavedGameSelected) {
			// handle selected game save
		} else {
			// handle cancel or error
		}
	}

	void OpenSavedGame(string filename) {
		ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
		savedGameClient.OpenWithAutomaticConflictResolution(filename, DataSource.ReadCacheOrNetwork,
			ConflictResolutionStrategy.UseLongestPlaytime, OnSavedGameOpened);
	}

	public void OnSavedGameOpened(SavedGameRequestStatus status, ISavedGameMetadata game) {
		if (status == SavedGameRequestStatus.Success) {
			// handle reading or writing of saved game.
		} else {
			// handle error
		}
	}
	Texture2D savedImage;
	void SaveGame (ISavedGameMetadata game, byte[] savedData, TimeSpan totalPlaytime) {
		ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;

		SavedGameMetadataUpdate.Builder builder = new SavedGameMetadataUpdate.Builder();
		builder = builder
			.WithUpdatedPlayedTime(totalPlaytime)
			.WithUpdatedDescription("Saved game at " + System.DateTime.Now);
		savedImage = getScreenshot ();
		if (savedImage != null) {
			// This assumes that savedImage is an instance of Texture2D
			// and that you have already called a function equivalent to
			// getScreenshot() to set savedImage
			// NOTE: see sample definition of getScreenshot() method below
			byte[] pngData = savedImage.EncodeToPNG();
			builder = builder.WithUpdatedPngCoverImage(pngData);
		}
		SavedGameMetadataUpdate updatedMetadata = builder.Build();
		savedGameClient.CommitUpdate(game, updatedMetadata, savedData, OnSavedGameWritten);
	}

	public void OnSavedGameWritten (SavedGameRequestStatus status, ISavedGameMetadata game) {
		if (status == SavedGameRequestStatus.Success) {
			// handle reading or writing of saved game.
		} else {
			// handle error
		}
	}

	public Texture2D getScreenshot() {
		// Create a 2D texture that is 1024x700 pixels from which the PNG will be
		// extracted
		Texture2D screenShot = new Texture2D(1024, 700);

		// Takes the screenshot from top left hand corner of screen and maps to top
		// left hand corner of screenShot texture
		screenShot.ReadPixels(
			new Rect(0, 0, Screen.width, (Screen.width/1024)*700), 0, 0);
		return screenShot;
	}

	void LoadGameData (ISavedGameMetadata game) {
		ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
		savedGameClient.ReadBinaryData(game, OnSavedGameDataRead);
	}

	public void OnSavedGameDataRead (SavedGameRequestStatus status, byte[] data) {
		if (status == SavedGameRequestStatus.Success) {
			// handle processing the byte array data
		} else {
			// handle error
		}
	}

	void DeleteGameData (string filename) {
		// Open the file to get the metadata.
		ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
		savedGameClient.OpenWithAutomaticConflictResolution(filename, DataSource.ReadCacheOrNetwork,
			ConflictResolutionStrategy.UseLongestPlaytime, DeleteSavedGame);
	}

	public void DeleteSavedGame(SavedGameRequestStatus status, ISavedGameMetadata game) {
		if (status == SavedGameRequestStatus.Success) {
			ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
			savedGameClient.Delete(game);
		} else {
			// handle error
		}
	}
}
