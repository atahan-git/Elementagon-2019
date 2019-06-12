using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_FireExploder : NPCBase {

	public float cardSelectTime = 3f;

	[Tooltip("1-4")]
	public int power = 1;

	public override IEnumerator MainLoop () {
		yield return new WaitForSeconds (0.5f);
		IndividualCard curTarget = null;
		List<IndividualCard> myCards;

		int _power = 1 + (power * 2);

		while (true){

			if (SelectedCardCount < 1) {
				do {
					myCards = GetRandomizedMoveableCardList ();
					yield return new WaitForSeconds (0.5f);
				} while (myCards.Count == 0);

				curTarget = GetRandomizedMoveableCardList ()[0];
				yield return MoveToPosition (curTarget);

				if (curTarget != null) {
					if (curTarget.isSelectable) {
						Activate (curTarget);

						for (int i = 0; i < _power; i++) {
							Select (GetAreaSequence (curTarget, i, _power));
							if (i % 4 == 0)
								yield return new WaitForSeconds (0.03f);
						}

						CheckCards (0.4f + (0.02f * _power));
					} else
						Denied ();
				} else
					Denied ();
			}

			

			yield return new WaitForSeconds (cardSelectTime * RandomTimeMultiplier());
		}
	}
}
