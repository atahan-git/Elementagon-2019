using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_NehirYilani : NPCBase {

	public float reactionTime = 2f;
	public float periodicAttackTime = 1f;

	public override IEnumerator MainLoop () {
		yield return new WaitForSeconds (0.5f);
		IndividualCard curTarget = null;
		List<IndividualCard> myCards;

		while (true){
			do {
				myCards = GetRandomizedMoveableCardList ();
				yield return new WaitForSeconds (0.5f);
			} while (myCards.Count == 0);

			curTarget = GetRandomizedMoveableCardList ()[0];
			yield return MoveToPosition (curTarget);

			if (!Select (curTarget))
				Denied ();

			yield return new WaitForSeconds (selectWaitTime);

			if (curTarget == null) {
				yield return new WaitForSeconds (reactionTime);
				continue;
			}

			while (curTarget.cBase != GS.a.cardSet.matchedCard) {
				Activate (curTarget);
				ScoreBoardManager.s.AddScore (DataHandler.s.myPlayerInteger, 0, -1, true);

				yield return new WaitForSeconds (periodicAttackTime);
			}

			Die (false);
		}
	}
}
