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

public class MultiPlayerConnectionManager : MonoBehaviour/*, RealTimeMultiplayerListener*/ {

	public int playerCount = 2;


	public static MultiPlayerConnectionManager s;
	private List<Participant> participants;
	IRealTimeMultiplayerClient RTClient;

	public Text logText;

	//public MPSecondPlayer secP;

	// Use this for initialization
	void Awake () 
	{
		DontDestroyOnLoad (this.gameObject);
		s = this;
	}

	void Start () 
	{	
		
		RTClient = PlayGamesPlatform.Instance.RealTime;
		//lobbyGUI.SetActive (false);
		//ChangePlayerCount (0);
		

		PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();

		PlayGamesPlatform.InitializeInstance(config);
		PlayGamesPlatform.DebugLogEnabled = true;
		PlayGamesPlatform.Activate();

		GameObject.FindObjectOfType<MPMenu> ().GetComponent<MPMenu> ().GirisYapildiMi (Social.localUser.authenticated);
		if (Social.localUser.authenticated != true) {
			//Login ();
		}
	}

	public List<Participant> GetParticipants()
	{
		return PlayGamesPlatform.Instance.RealTime.GetConnectedParticipants ();
	}

	public Participant OtherPlayer ()
	{
		List <Participant> katilimci = PlayGamesPlatform.Instance.RealTime.GetConnectedParticipants ();
		katilimci.Remove(PlayGamesPlatform.Instance.RealTime.GetSelf());
		Participant karsiOyuncu = katilimci [0];
		return karsiOyuncu;
	}

	public Participant GetPbyID (string katilimciID) 
	{
		return PlayGamesPlatform.Instance.RealTime.GetParticipant (katilimciID);
	}

	public Participant GetSelf ()
	{
		return PlayGamesPlatform.Instance.RealTime.GetSelf ();
	}

	/*public void Login ()
	{
		Social.localUser.Authenticate((bool success) => 
			{
				if(success)
				{
					DataLogger.text = "Giriş Yapıldı";
					GameObject.FindObjectOfType<MPMenu> ().GetComponent<MPMenu> ().GirisYapildiMi (true);
				}
				else
				{
					DataLogger.text = "Girişte Hata";
					GameObject.FindObjectOfType<MPMenu> ().GetComponent<MPMenu> ().GirisYapildiMi (false);
					Login();
				}
			});
	}

	public void GetMatch ()
	{
		//sInstance = new MultiPlayerConnect();
		//const int MinOpponents = 1, MaxOpponents = 1;
		const int GameVariant = 0;
		PlayGamesPlatform.Instance.RealTime.CreateQuickGame((uint)(playerCount - 1), (uint)(playerCount - 1), GameVariant, this);
		DataLogger.text = "Oda aranıyor";
	}
	bool isFirst = true;
	void Update () {
		if (logText == null) {
			logText = FindObjectOfType<Text> ();
		}
		//DataLogger.text = GetSelf ().ParticipantId.ToString();
	}

	public void OnRoomConnected(bool success) {
		DataLogger.text = "OnRoomConnected";
		if (success) 
		{
			DataLogger.text = "Odaya bağlanıldı";
			SceneManager.LoadScene (1);
			participants = GetParticipants ();
			/*MPOrganizer.firstPlayerid = OyunculariAl ().First ().ParticipantId;
			MPOrganizer.secondPlayerid = OyunculariAl ().Last ().ParticipantId;

			MPOrganizer.firstPlayerName = OyunculariAl ().First ().DisplayName;
			MPOrganizer.secondPlayerName = OyunculariAl ().Last ().DisplayName;*

		} else 
		{
			DataLogger.text = "Odaya bağlanılamadı";
			PlayGamesPlatform.Instance.RealTime.LeaveRoom ();
		}
	}

	private bool showingWaitingRoom = false;

	public void OnRoomSetupProgress(float progress) {
		// show the default waiting room.
		if (!showingWaitingRoom) {
			showingWaitingRoom = true;
			PlayGamesPlatform.Instance.RealTime.ShowWaitingRoomUI();
		}

		if (progress == 20) {
			DataLogger.text = "Eş Bekleniyor";
		} else {
			DataLogger.text = "bağlanılıyor " + ((int)progress).ToString() + "%";
		}
	}

	public void OnParticipantLeft (Participant katilimci) {
		DataLogger.text = katilimci.DisplayName + " odadan çıktı";
	}

	public void OnPeersConnected(string[] participantIds) {
		foreach (string participant in participantIds) {
			DataLogger.text = participant + " Joined";
		}
	}

	public void OnPeersDisconnected(string[] participantIds) {
		foreach (string participant in participantIds) {
			DataLogger.text = participant + " Left";
		}
	}

	public void Exit () {
		PlayGamesPlatform.Instance.SignOut();
		GameObject.FindObjectOfType<MPMenu> ().GetComponent<MPMenu> ().GirisYapildiMi (false);
	}

	public void OnLeftRoom() {
		// display error message and go back to the menu screen

		// (do NOT call PlayGamesPlatform.Instance.RealTime.LeaveRoom() here --
		// you have already left the room!)
	}

	public void OnRealTimeMessageReceived(bool isReliable, string senderId, byte[] data) {
		/*if (secP == null) {
			secP = FindObjectOfType<MPSecondPlayer> ();
		}
		if (secP != null) {
			char Tip = (char)data [0];
			switch (Tip) {
			case 'P':
				Vector3 ikiP;

				float posX = System.BitConverter.ToSingle (data, 1);
				float posY = System.BitConverter.ToSingle (data, 4 + 1);
				float posZ = System.BitConverter.ToSingle (data, 4 + 4 + 1);

				ikiP = new Vector3 (posX, posY, posZ);

				secP.recPos = ikiP;

				break;
			case 'R':

				Vector3 ikiR;

				float rotX = System.BitConverter.ToSingle (data, 1);
				float rotY = System.BitConverter.ToSingle (data, 4 + 1);
				float rotZ = System.BitConverter.ToSingle (data, 4 + 4 + 1);

				ikiR = new Vector3 (rotX, rotY, rotZ);
				secP.recRot = ikiR;

				break;
			default:
				Debug.LogError ("Unknown Data Received");
				break;
			}
		}*
	}*/



}

/*

using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using GooglePlayGames;
public void KonumGonder (float posX, float posY, float posZ){
	veriListesi.Clear ();
	veriListesi.Add ((byte) 'P');
	veriListesi.AddRange (System.BitConverter.GetBytes(posX));
	veriListesi.AddRange (System.BitConverter.GetBytes(posY));
	veriListesi.AddRange (System.BitConverter.GetBytes(posZ));
	gonderilecekVeriler = veriListesi.ToArray ();

	PlayGamesPlatform.Instance.RealTime.SendMessageToAll (false, gonderilecekVeriler);
}

*/
