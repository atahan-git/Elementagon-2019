using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _NPC_PosionDragon : _NPCBehaviour {

	public GameObject poisonSelectEfect;

	public int maxActivePoison = 3;
	public Vector2 poisonInterval = new Vector2 (20, 25);

	// Use this for initialization
	void Start () {
		SetUp (0, poisonInterval.x, poisonInterval.y, 6, 2);
		NormalSelectSetUp (1, 0.8f, 1.4f, 0.5f, 3, 0.3f, true);
	}

	public override void Activate (){
		if (isFin || isGoToPosActive) {
			if (IndividualCard.poisonCount < maxActivePoison)
				StartCoroutine (_Activate (SelectRandomCard ()));
		}
	}

	bool isFin = true;
	IEnumerator _Activate (IndividualCard myCard){
		isFin = false;
		while (!IsOnPos () || isGoToPosActive)
			yield return 0;

		GoToPos (myCard.x, myCard.y, "Poison");

		while (!IsOnPos ())
			yield return 0;

		yield return new WaitForSeconds (0.5f);

		myCard.SelectCard ();
		myCard.cardType = 15;
		myCard.isPoison = true;
		myCard.selectedEffect = (GameObject)Instantiate (poisonSelectEfect, myCard.transform.position, Quaternion.identity);

		yield return new WaitForSeconds (GS.a.powerUpSettings.poison_activeTime);

		myCard.UnSelectCard ();

		isFin = true;
	}
}
