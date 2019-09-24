using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewController : MonoBehaviour {
	/*	Required tree structure
		 *	Root
		 *		>Gfx
		 *		>Child_1
		 *			>Gfx
		 *		>Child_2
		 *			>Gfx
		 *			>Child_2_1
		 *	naming isnt important but child order is
		 */

	[Header("View Controller")]

	[HideInInspector]
	public GameObject gfxs;
	[HideInInspector]
	public ViewController[] children = new ViewController[0];
	[HideInInspector]
	public int[] myId;

	public bool isShown = false;

	float transitionSpeed = 20f;

	public void AssignChildren () {
		gfxs = transform.GetChild(0).gameObject;

		children = new ViewController[transform.childCount - 1];

		for (int i = 1; i < transform.childCount; i++) {
			children[i - 1] = transform.GetChild(i).GetComponent<ViewController>();
		}
	}

	public void InstaShow () {
		/*if(!isShown)
		print("Insta showe " + name);*/
		gfxs.SetActive(true);
		isShown = true;
	}

	public void InstaHide () {
		/*if (isShown) {
			print("Insta hide " + name);
		}*/
		isShown = false;
		gfxs.SetActive(false);
	}

	public virtual void TransitionShow () {
		if (!isShown)
			StartCoroutine(TransitionShowAnimated());
	}


	public virtual IEnumerator TransitionShowAnimated () {
		//print("animated showing: " + name);
		//MenuMaster.s.BlackTransitionEffect.color = new Color(0, 0, 0, 1);
		MenuMasterController.s.BlackTransitionEffect.enabled = true;
		a = MenuMasterController.s.BlackTransitionEffect.color.a;
		while (a <= 1f) {
			MenuMasterController.s.BlackTransitionEffect.color = new Color(0, 0, 0, a);
			a += transitionSpeed * Time.deltaTime;
			yield return null;
		}
		gfxs.SetActive(true);
		while (a >= 0f) {
			MenuMasterController.s.BlackTransitionEffect.color = new Color(0, 0, 0, a);
			a -= transitionSpeed * Time.deltaTime;
			yield return null;
		}
		MenuMasterController.s.BlackTransitionEffect.color = new Color(0, 0, 0, 0);
		MenuMasterController.s.BlackTransitionEffect.enabled = false;

		InstaShow();
		yield return null;
	}

	public virtual void TransitionHide () {
		if (isShown)
			StartCoroutine(TransitionHideAnimated());
	}

	public float a;
	public virtual IEnumerator TransitionHideAnimated () {
		//print("animated hiding: " + name);
		if (!isShown)
			InstaHide();

		//MenuMaster.s.BlackTransitionEffect.color = new Color(0, 0, 0, 0);
		MenuMasterController.s.BlackTransitionEffect.enabled = true;
		a = MenuMasterController.s.BlackTransitionEffect.color.a;
		while (a >= 0f) {
			MenuMasterController.s.BlackTransitionEffect.color = new Color(0, 0, 0, a);
			a -= transitionSpeed * Time.deltaTime;
			yield return null;
		}
		while (a <= 1f) {
			MenuMasterController.s.BlackTransitionEffect.color = new Color(0, 0, 0, a);
			a += transitionSpeed * Time.deltaTime;
			yield return null;
		}
		MenuMasterController.s.BlackTransitionEffect.color = new Color(0, 0, 0, 1);
		MenuMasterController.s.BlackTransitionEffect.enabled = true;

		InstaHide();
		yield return null;
	}
}
