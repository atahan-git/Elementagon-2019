using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectHitScoreBoard : BetweenCardsEffect {


	public float vUnifier, vx, vy;

	float aUnifier, ax, ay;

	public float xMul = -2f;
	public float yMul = -5f;

	public float uniTimer = 0.3f;
	public float timer = 1.2f;

	public GameObject unifier1;
	public GameObject unifier2;
	public GameObject projectile;
	public GameObject hitEffect;

	Transform target;

	IEnumerator Move () {
		projectile.SetActive (false);
		while (uniTimer > 0f) {
			unifier1.transform.position = Vector3.MoveTowards (unifier1.transform.position, transform.position, vUnifier * Time.deltaTime);
			unifier2.transform.position = Vector3.MoveTowards (unifier2.transform.position, transform.position, vUnifier * Time.deltaTime);

			vUnifier += Time.deltaTime * aUnifier;

			uniTimer -= Time.deltaTime;
			yield return null;
		}
		Destroy (unifier1);
		Destroy (unifier2);

		projectile.SetActive (true);
		while (timer > 0f) {
			projectile.transform.position += new Vector3(vx * Time.deltaTime, vy * Time.deltaTime, 0);
			projectile.transform.Rotate (0, 0, -180f * Time.deltaTime);

			vx += Time.deltaTime * ax;
			vy += Time.deltaTime * ay;

			timer -= Time.deltaTime;
			yield return null;
		}

		Destroy (projectile);

		Instantiate (hitEffect, target.position, target.rotation);
		yield return null;

		/*projectile.transform.position = transform.position;
		projectile.transform.rotation = Quaternion.Euler (0, 0, 95f);
		uniTimer = 0.3f;
		timer = .9f;
		SetUp (-1, card1, card2);*/
	}



	public override void SetUp (int playerID, bool isPowerUp, IndividualCard card1, IndividualCard card2) {
		AlignBetweenCards (card1, card2, AlignMode.position);

		unifier1.transform.position = card1.transform.position - Vector3.forward * 0.1f;
		unifier2.transform.position = card2.transform.position - Vector3.forward * 0.1f;

		float deltaUnifier = Mathf.Abs((card1.transform.position - card2.transform.position).magnitude) / 2f;
		aUnifier = 2 * deltaUnifier / (uniTimer * uniTimer);
		vUnifier = 0;

		List<Transform> targets = new List<Transform> ();
		if (!isPowerUp) {
			targets.Add (ScoreBoardManager.s.scoreGetTargets[playerID]);
		} else {
			targets.Add (ScoreBoardManager.s.powerGetTargets[0]);
			targets.Add (ScoreBoardManager.s.powerGetTargets[0]);
		}
		target = targets[Random.Range(0,targets.Count)];

		float deltax = target.position.x - transform.position.x;
		float deltay = target.position.y - transform.position.y;

		vx = deltax * xMul;
		vy = deltay * yMul;

		ax = 2 * (deltax - (vx * timer)) / (timer * timer);
		ay = 2 * (deltay - (vy * timer)) / (timer * timer);

		StartCoroutine (Move ());
	}
}
