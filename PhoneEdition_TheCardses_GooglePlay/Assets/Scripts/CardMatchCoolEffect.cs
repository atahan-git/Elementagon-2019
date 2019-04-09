using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardMatchCoolEffect : MonoBehaviour {

	/*blic static CardMatchCoolEffect s;

	public GameObject effect;
	public float z = -1;

	public Transform[] scoreGetTargets = new Transform[6];

	private void Start () {
		s = this;
	}

	public void MatchTwo (int myPlayerinteger, IndividualCard card1, IndividualCard card2, float score) {
		if (score > 0) {
			_MatchTwo (myPlayerinteger, card1, card2);
			DataHandler.s.SendCardMatchEffectAction (myPlayerinteger, card1.x, card1.y, card2.x, card2.y);
		}
	}

	public void ReceiveNetworkMatchTwo (int myPlayerinteger, IndividualCard card1, IndividualCard card2) {
		_MatchTwo (myPlayerinteger, card1, card2);
	}


	void _MatchTwo (int myPlayerinteger, IndividualCard card1, IndividualCard card2) {
		int myType = card1.cBase.cardType;
		GameObject myEfect = Instantiate (effect);
		Vector3 [] places = new Vector3 [2];
		places [0] = card1.transform.position + (Vector3.forward * z);
		places [1] = card2.transform.position + (Vector3.forward * z);
		myEfect.GetComponent<MatchEfect> ().SetUp (places, myType, scoreGetTargets[myPlayerinteger]);
	}*/


}
