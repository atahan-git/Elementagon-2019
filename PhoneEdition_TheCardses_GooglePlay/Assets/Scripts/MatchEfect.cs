using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchEfect : BetweenCardsEffect {

	public GameObject [] particles = new GameObject[2];

	[Tooltip ("//--------CARD TYPES---------\n// 0 = empty / already gotten\n// 1-7 = normal cards\n// 8-14 = dragons\n//---------------------------\n// 1 = Earth\n// 2 = Fire\n// 3 = Ice\n// 4 = Light\n// 5 = Nether\n// 6 = Poison\n// 7 = Shadow\n//---------------------------\n// 8 = Earth Dragon\n// 9 = Fire Dragon\n//10 = Ice Dragon\n//11 = Light Dragon\n//12 = Nether Dragon\n//13 = Poison Dragon\n//14 = Shadow Dragon\n//---------------------------")]
	public Color [] colors = new Color [32];
	// Use this for initialization

	public override void SetUp (int playerID, bool isPowerUp, IndividualCard card1, IndividualCard card2) {
		int elementalType = card1.cBase.elementType;
		Vector3[] places = new Vector3[2];
		places[0] = card1.transform.position + (-Vector3.forward);
		places[1] = card2.transform.position + (-Vector3.forward);
		List<Transform> targets = new List<Transform> ();
		if (!isPowerUp) {
			targets.Add (ScoreBoardManager.s.scoreGetTargets[playerID]);
		} else {
			targets.Add (ScoreBoardManager.s.powerGetTargets[0]);
			targets.Add (ScoreBoardManager.s.powerGetTargets[1]);
		}
		foreach (ParticleSystem part in GetComponentsInChildren<ParticleSystem> ()) {
			part.startColor = colors[elementalType];
		}

		GetComponentInChildren<LineRenderer> ().startColor = colors[elementalType];
		GetComponentInChildren<LineRenderer> ().endColor = colors[elementalType];


		GetComponentInChildren<DigitalRuby.LightningBolt.LightningBoltScript> ().StartPosition = places[0];
		GetComponentInChildren<DigitalRuby.LightningBolt.LightningBoltScript> ().EndPosition = places[1];

		try {
			int n = 0;
			foreach (particleAttractorLinear att in GetComponentsInChildren<particleAttractorLinear> ()) {
				if (targets[n] == null)
					n = IncrementN (n, targets);
				if (targets[n] == null)
					DataLogger.LogError ("Trying to set null match effect target!");
				att.target = targets[n];
				n = IncrementN (n, targets);
			}
		} catch (System.Exception e) {
			DataLogger.LogError ("Cant set match effect target " + e.Message);
		}

		particles[0].transform.position = places[0];
		particles[1].transform.position = places[1];
	}

	int IncrementN (int n, List<Transform> targets) {
		n++;

		if (n >= targets.Count)
			n = 0;

		return n;
	}
}
