using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Starter : MonoBehaviour {

	//public MenuHandler menu;
	public static Starter s;

	// Use this for initialization
	void Start () {
		s = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void StartGame(){
		/*if (AirConsole.instance.GetActivePlayerDeviceIds.Count >= 2) {

			/*DataHandler.gridSizeX = (int)menu.sliderGridSizeX.value;
			DataHandler.gridSizeY = (int)menu.sliderGridSizeY.value;

			DataHandler.playerCount = AirConsole.instance.GetActivePlayerDeviceIds.Count;

			DataHandler.isTimeAttack = menu.isTimeAttack;
			DataHandler.gameSetting = menu.modeValue;

			SceneManager.LoadScene (1);
		}*/
	}
}
