using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMasterController : MonoBehaviour {

	public static MenuMasterController s;

	public ViewController root;
	int maxDepth = 5;

	public static List<int> currentPlace = new List<int> ();

	public ViewController[] overlays;

	private void Awake () {
		s = this;
	}

	// Use this for initialization
	void Start () {
		AssignIds (root.children, 0, new int[0]);

		if (currentPlace.Count > 0) {
			GoToPlace (currentPlace);
		} else {
			GoToPlace (new List<int> (new int[] { 0}));
		}
	}


	void AssignIds (ViewController[] views, int depth ,int[] parentIds) {
		for (int i = 0; i < views.Length; i++) {
			if (views[i] != null) {
				views[i].myId = new int[parentIds.Length + 1];
				parentIds.CopyTo (views[i].myId, 0);
				views[i].myId[parentIds.Length] = i;
				if (views[i].children.Length > 0)
					AssignIds (views[i].children, depth + 1, views[i].myId);
			}
		}
	}

	public void GoToPlace (ViewController vc) {
		//add cool animations?
		GoToPlace (new List<int> (vc.myId));
	}


	public void GoToPlace (List<int> place) {
		HideAll (root.children);

		ViewController curPlace = root;

		int depth = 0;
		while (true) {
			if (place.Count > depth) {
				if (curPlace.children.Length > place[depth]) {
					curPlace = curPlace.children[place[depth]];
				} else {
					Debug.LogError ("Trying to go to non-existent place! " + place.ToString ());
					break;
				}
			} else {
				break;
			}
			currentPlace = place;
			curPlace.Show ();
			depth++;
		}

		if (curPlace.openChildAsDefault) {
			currentPlace.Add (0);
			GoToPlace (currentPlace);
		}

	}

	void HideAll (ViewController[] views) {
		for (int i = 0; i < views.Length; i++) {
			views[i].Hide ();
			if (views[i].children.Length > 0)
				HideAll (views[i].children);
		}
	}

	public void GoDown (int id) {
		ViewController curPlace = GetPlace (currentPlace);
		currentPlace.Add (id);
		ViewController toGo = GetPlace (currentPlace);
		if (toGo != null) {
			toGo.TransitionShow ();
			print ("Went down to " + GetPlace (currentPlace).gameObject.name + " from " + curPlace.gameObject.name);
		} else {
			currentPlace.RemoveAt (currentPlace.Count - 1);
		}
	}

	public void GoUp () {
		ViewController toGetBack = GetPlace (currentPlace);
		currentPlace.RemoveAt (currentPlace.Count - 1);
		if (toGetBack != null) {
			toGetBack.TransitionHide ();
			print ("Went up to " + GetPlace(currentPlace).gameObject.name + " from " + toGetBack.gameObject.name);
		}
	}

	ViewController GetPlace (List<int> place){
		ViewController curPlace = root;

		int depth = 0;
		while (true) {
			if (place.Count > depth) {
				if (curPlace.children.Length > place[depth]) {
					curPlace = curPlace.children[place[depth]];
				} else {
					string all = "";
					foreach (var k in place) {
						all += k.ToString () + " - ";
					}
					Debug.LogError ("Trying to get non-existent place! " + curPlace.gameObject.name + " - " + all);
					return null;
				}
			} else {
				return curPlace;
			}
			depth++;
		}
	}

	public ViewController OpenOverlay (int id) {
		overlays[id].TransitionShow ();
		return overlays[id];
	}

	public void CloseOverlay () {
		foreach (ViewController v in overlays)
			v.TransitionHide ();
	}
}
