using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextGrowandShrink : MonoBehaviour
{
	public Vector2 ranges;
	public float speed = 0.2f;

	private void OnEnable () {
		StopAllCoroutines ();
		StartCoroutine (Cycle ());
	}

	IEnumerator Cycle () {
		TMPro.TextMeshProUGUI myText = GetComponent<TMPro.TextMeshProUGUI> ();
		while (true) {
			while (myText.fontSize < ranges.y) {
				myText.fontSize = Mathf.MoveTowards (myText.fontSize, ranges.y, speed * Time.deltaTime);
				yield return null;
			}

			while (myText.fontSize > ranges.x) {
				myText.fontSize = Mathf.MoveTowards (myText.fontSize, ranges.x, speed * Time.deltaTime);
				yield return null;
			}
		}
	}
}
