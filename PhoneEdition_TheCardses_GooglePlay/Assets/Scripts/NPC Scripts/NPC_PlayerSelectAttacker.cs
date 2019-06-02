using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_PlayerSelectAttacker : NPCBase {

	public float reactionTime = 2f;
	public float periodicAttackTime = 1f;

	public override IEnumerator MainLoop () {
		yield return new WaitForSeconds (0.5f);
		IndividualCard curTarget = null;

		while (true){
			List<IndividualCard> allCards = GetAllCards ();
			RandFuncs.Shuffle (allCards);
			foreach (IndividualCard card in allCards) {
				if (card.currentSelectingPlayer != -1) {
					curTarget = card;
					break;
				}
			}

			if (curTarget == null) {
				yield return new WaitForSeconds (reactionTime);
				continue;
			}

			yield return MoveToPosition (curTarget);

			while (curTarget.currentSelectingPlayer != -1) {
				Activate (curTarget);
				ScoreBoardManager.s.AddScore (curTarget.currentSelectingPlayer, 0, -1, true);

				yield return new WaitForSeconds (periodicAttackTime);
			}

			yield return new WaitForSeconds (reactionTime);
		}
	}
}
