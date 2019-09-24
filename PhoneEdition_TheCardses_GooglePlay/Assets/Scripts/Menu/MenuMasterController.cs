using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MenuMasterController : MonoBehaviour {

	public static MenuMasterController s;

	public UnityEngine.UI.Image BlackTransitionEffect;

	public ViewController root;

	public static List<int> currentPlace = new List<int> ();


	public ViewController[] overlays;

	private void Awake () {
		s = this;
	}

	// Use this for initialization
	void Start () {
		AssignIds (root, 0, new int[0]);

		foreach (ViewController vc in overlays)
			vc.AssignChildren();

		if (currentPlace.Count > 0) {
			GoToPlace (currentPlace);
		} else {
			root.InstaShow();
		}
	}


	void AssignIds (ViewController view, int depth, int[] parentIds) {
		view.AssignChildren();

		for (int i = 0; i < view.children.Length; i++) {
			if (view.children[i] != null) {
				view.children[i].myId = new int[parentIds.Length + 1];
				parentIds.CopyTo(view.children[i].myId, 0);
				view.children[i].myId[parentIds.Length] = i;

				AssignIds(view.children[i], depth + 1, view.children[i].myId);
			}
		}

		view.InstaHide();
	}

	public void GoToPlace (ViewController vc) {
		//add cool animations?
		DataLogger.LogMessage ("Menu is going to " + vc.name);
		GoToPlace (new List<int> (vc.myId));
	}

	public void GoToPlace (List<int> place) {
		StartCoroutine(_GoToPlace(place));
	}

	public static bool traversalLock = false;
	public IEnumerator _GoToPlace (List<int> place) {
		while (traversalLock)
			yield return null;

		traversalLock = true;

		string placeID = "";
		foreach (var k in place) {
			placeID += k.ToString() + " - ";
		}
		//print("-*-*-*-*-*-*-*-*-*- We are goint to places " + placeID + " -*-*-*-*-*-*-*-*-*-");
		yield return null;


		//print("Finding how deep we need to retrace our steps");
		int depth = 0;
		while (true) {
			if (depth < currentPlace.Count && depth < place.Count)
				if (currentPlace[depth] == place[depth])
					depth++;
				else
					break;
			else
				break;
		}

		//print("Going up to depth: " + depth.ToString());
		while (currentPlace.Count > depth)
			yield return StartCoroutine(GoUp());


		//print("Going down to depth: " + place.Count);
		while (currentPlace.Count < place.Count) {
			//print(currentPlace.Count);
			//print(place.Count);
			//print(depth);
			yield return StartCoroutine(GoDown(place[depth]));
			depth++;
		}

		if (!GetPlace(currentPlace).isShown)
			GetPlace(currentPlace).InstaShow();

		traversalLock = false;
	}

	IEnumerator HideAll (ViewController[] views) {
		//print("Hiding all: " + views[0].name);
		yield return null;
		for (int i = 0; i < views.Length; i++) {
			if (views[i] != null) {
				if (views[i].children.Length > 0) {
					yield return StartCoroutine(HideAll(views[i].children));
				}
				if (views[i].isActiveAndEnabled)
					yield return views[i].StartCoroutine(views[i].TransitionHideAnimated());
				else
					views[i].InstaHide();
			}
		}
	}

	IEnumerator GoDown (int id) {
		//print("Going down " + id.ToString());
		ViewController curPlace = GetPlace(currentPlace);
		currentPlace.Add(id);
		ViewController toGo = GetPlace(currentPlace);
		if (toGo != null) {
			yield return curPlace.StartCoroutine(curPlace.TransitionHideAnimated());
			yield return toGo.StartCoroutine(toGo.TransitionShowAnimated());
			//print("Went down to " + GetPlace(currentPlace).gameObject.name + " from " + curPlace.gameObject.name);
		} else {
			currentPlace.RemoveAt(currentPlace.Count - 1);
		}
	}

	IEnumerator GoUp () {
		//print("Going up");
		ViewController curPlace = GetPlace(currentPlace);
		currentPlace.RemoveAt(currentPlace.Count - 1);
		ViewController toGo = GetPlace(currentPlace);
		if (curPlace != null) {
			yield return curPlace.StartCoroutine(curPlace.TransitionHideAnimated());
			yield return toGo.StartCoroutine(toGo.TransitionShowAnimated());
			//print("Went up to " + GetPlace(currentPlace).gameObject.name + " from " + curPlace.gameObject.name);
		}
	}

	ViewController GetPlace (List<int> place) {
		ViewController curPlace = root;

		int depth = 0;
		while (true) {
			if (place.Count > depth) {
				if (curPlace.children.Length > place[depth]) {
					curPlace = curPlace.children[place[depth]];
				} else {
					string all = "";
					foreach (var k in place) {
						all += k.ToString() + " - ";
					}
					Debug.LogError("Trying to get non-existent place! " + curPlace.gameObject.name + " - " + all);
					return null;
				}
			} else {
				return curPlace;
			}
			depth++;
		}
	}

	public ViewController OpenOverlay (int id) {
		overlays[id].InstaShow ();
		return overlays[id];
	}

	public void CloseOverlay () {
		foreach (ViewController v in overlays)
			v.InstaHide ();
	}
}
