using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class PowerUp_Nether : PowerUp_Active_Instant {
	public override void Enable (int _power, float _amount, Color _effectColor) {
		base.Enable (_power, _amount, _effectColor);

		StartCoroutine (_Enable (power, amount));
	}

	[Space]
	public GameObject netherEffectPrefab;
	public GameObject getEffectPrefab;
	int netherCount = 0;
	IEnumerator _Enable (int _power, float _amount) {

		//----------------------new
		//GameObject netherEffect = (GameObject)Instantiate (netherEffectPrefab, Vector3.zero, Quaternion.identity);
		//yield return new WaitForSeconds (netherActiveTime);
		//----------------------new

		int gridSizeX = CardHandler.s.allCards.GetLength (0);
		int gridSizeY = CardHandler.s.allCards.GetLength (1);
		netherCount = 0;

		int necessaryNetherCount = _power == 2 ? 3 : 2;

		IndividualCard lastCard = null;

		for (int y = 0; y < gridSizeY; y++) {
			if (_power <= 1)
				if (y == 0 || y == 3)
					continue;

			//----------------------old
			IndividualCard NetherPos = CardHandler.s.allCards[0, y];
			GameObject netherEffect = (GameObject)Instantiate (netherEffectPrefab, NetherPos.transform.position, Quaternion.identity);
			//----------------------old

			for (int x = 0; x < gridSizeX; x++) {

				IndividualCard myCardS = CardHandler.s.allCards[x, y];


				if (myCardS.cBase == null) {

					netherCount++;
					if (netherCount >= necessaryNetherCount) {
						ScoreBoardManager.s.AddScore (DataHandler.s.myPlayerInteger, 5, 1, true);
						//CardMatchCoolEffect.s.MatchTwo (myCardS, lastCard, 1);
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

	public override void NetworkedEnable (int player, int _power, float _amount) {
		StartCoroutine (NetworkNetherRoutine (power, amount));
	}

	IEnumerator NetworkNetherRoutine (int _power, float _amount) {

		//----------------------new
		//GameObject netherEffect = (GameObject)Instantiate (netherEffectPrefab, Vector3.zero, Quaternion.identity);
		//yield return new WaitForSeconds (netherActiveTime);
		//----------------------new

		int gridSizeX = CardHandler.s.allCards.GetLength (0);
		int gridSizeY = CardHandler.s.allCards.GetLength (1);

		for (int y = 0; y < gridSizeY; y++) {

			//----------------------old
			IndividualCard NetherPos = CardHandler.s.allCards[0, y];
			GameObject netherEffect = (GameObject)Instantiate (netherEffectPrefab, NetherPos.transform.position, Quaternion.identity);
			//----------------------old

			for (int x = 0; x < gridSizeX; x++) {

				IndividualCard myCardS = CardHandler.s.allCards[x, y];


				if (myCardS.cBase == null) {

					GameObject netherGetEffect = (GameObject)Instantiate (getEffectPrefab, myCardS.transform.position - Vector3.forward, Quaternion.identity);
					//myCardS.cardType = 5;
					//myCardS.cardType = 0;
					yield return new WaitForSeconds (0.01f);

				} else {
					//myCardS.JustRotate ();
					myCardS.NetherReset ();
				}

				yield return new WaitForSeconds (0.005f);
			}
			yield return new WaitForSeconds (0.05f);
		}
		yield return null;

	}
}