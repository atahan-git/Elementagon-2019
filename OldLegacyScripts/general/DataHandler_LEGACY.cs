using UnityEngine;
using System.Collections;

public class DataHandler_LEGACY : MonoBehaviour {

	public static int playerCount = 2;
	public static int gridSizeX = 10;
	public static int gridSizeY = 4;

	public static bool isTimeAttack = true; //if not it is score

	public static int gameSetting = 120;

	public static DataHandler_LEGACY s;

	// Use this for initialization
	void Awake () {

		s = this;
		DontDestroyOnLoad (gameObject);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
