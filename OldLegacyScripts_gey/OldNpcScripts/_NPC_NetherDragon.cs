using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _NPC_NetherDragon : _NPCBehaviour {

	public GameObject netherEffectPrefab;
	public GameObject getEffectPrefab;

	// Use this for initialization
	void Start () {
		SetUp (5, 20, 25, 5, 2);
		NormalSelectSetUp (2, 2f, 3f, 1f, 3, 0.6f, false);
	}


	public override void Activate (){
		StartCoroutine (NetherRoutine ());
	}

	int netherCount = 0;
	IEnumerator NetherRoutine(){

		//----------------------new
		//GameObject netherEffect = (GameObject)Instantiate (netherEffectPrefab, Vector3.zero, Quaternion.identity);
		//yield return new WaitForSeconds (netherActiveTime);
		//----------------------new

		int gridSizeX = CardHandler.s.allCards.GetLength (0);
		int gridSizeY = CardHandler.s.allCards.GetLength (1);
		netherCount = 0;

		IndividualCard lastCard = null;

		for (int y = 0; y < gridSizeY; y++) {

			//----------------------old
			IndividualCard NetherPos = CardHandler.s.allCards [0, y];
			GameObject netherEffect = (GameObject)Instantiate (netherEffectPrefab, NetherPos.transform.position, Quaternion.identity);
			//----------------------old

			for (int x = 0; x < gridSizeX; x++) {

				IndividualCard myCardS = CardHandler.s.allCards [x, y];


				if (myCardS.cardType == 0) {

					netherCount++;
					if (netherCount >= 2) {
						ScoreBoardManager.s.AddScore (DataHandler.NPCInteger, 5, 1, true);
						//CardMatchCoolEffect.s.MatchTwo (lastCard, myCardS,1);
						netherCount = 0;
					}

					GameObject netherGetEffect = (GameObject)Instantiate (getEffectPrefab, myCardS.transform.position - Vector3.forward, Quaternion.identity);
					//myCardS.cardType = 5;
					//myCardS.cardType = 0;
					yield return new WaitForSeconds (0.01f);

				} else {
					//myCardS.JustRotate ();
					myCardS.NetherReset ();
				}

				lastCard = myCardS;

				yield return new WaitForSeconds (0.005f);
			}
			yield return new WaitForSeconds (0.05f);
		}
		netherCount = 0;
		yield return null;

	}
}
