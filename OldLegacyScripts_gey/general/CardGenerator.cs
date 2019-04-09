using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CardGenerator : MonoBehaviour {

	public static CardGenerator s;

	public Vector3[,] grid = new Vector3[4,4];
	public GameObject[,] allCards = new GameObject[4,4];
	public int gridSizeX = 10;
	public int gridSizeY = 5;

	public float gridScaleX = 1.49f;
	public float gridScaleY = 1.99f;

	public GameObject card;

	public float scaleMultiplier = 1f;

	void Awake(){
		s = this;
	}

	// Use this for initialization
	void Start () {

		/*if (!isServer)
			return;*/

		if (DataHandler_LEGACY.s != null) {
			gridSizeX = DataHandler_LEGACY.gridSizeX;
			gridSizeY = DataHandler_LEGACY.gridSizeY;
		}

		/*if (gridSizeY >= 5) {
			scaleMultiplier = 0.9f;
			gridScaleX *= 0.85f;
			gridScaleY *= 0.85f;
			transform.position = transform.position + new Vector3 (0,-0.15f,0);
		}*/

		SetUpGrid ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetUpGrid (){

		//first clean old cards if they exist
		DeleteCards ();

		//give us a better centered position
		Vector3 center = new Vector3 (transform.position.x - gridScaleX * ((float)gridSizeX/2f - 0.5f), transform.position.y - gridScaleY * ((float)gridSizeY/2f - 0.5f), transform.position.z);

		//set up arrays according to the sizes that are given to us
		grid = new Vector3[gridSizeX,gridSizeY];
		allCards = new GameObject[gridSizeX,gridSizeY];

		//set up grid positions
		for (int i = 0; i < gridSizeX; i++) {
			for (int m = 0; m < gridSizeY; m++) {

				grid [i, m] = new Vector3();
				grid [i, m] = center + new Vector3 (i * gridScaleX, m * gridScaleY, 0);

			}
		}
			
		//instantiate cards at correct positions
		for (int i = 0; i < gridSizeX; i++) {
			for (int m = 0; m < gridSizeY; m++) {

				GameObject myCard = (GameObject)Instantiate (card, grid [i, m], Quaternion.identity);
				myCard.transform.parent = transform;
				myCard.transform.localScale = new Vector3 (scaleMultiplier, scaleMultiplier, scaleMultiplier);
				allCards [i, m] = myCard;
				//myCard.GetComponent<CardScript> ().myPos = new Vector3 (i, m, 0);

				//NetworkServer.Spawn (myCard);

			}
		}


	}

	void DeleteCards (){
		for (int i = 0; i < allCards.GetLength(0); i++) {
			for (int m = 0; m < allCards.GetLength(1); m++) {
				if (allCards[i,m] != null) {
					Destroy (allCards [i, m].gameObject);
					allCards [i, m] = null;
				}
			}
		}
	}

	void RandomizeArray(int[] arr){
		for (int i = arr.Length - 1; i > 0; i--) {
			int r = Random.Range (0, i);
			int tmp = arr [i];
			arr [i] = arr [r];
			arr [r] = tmp;
		}
	}
}
