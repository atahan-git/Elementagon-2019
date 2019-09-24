using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardGraphicsHolder : MonoBehaviour
{
	public static CardGraphicsHolder s;

	public Sprite[] cardBacks;
	public Sprite[] cardBorders;

	public int cardBackID;
	public int cardBorderID;

	public delegate void GenericDelegate ();
	public GenericDelegate cardSpriteChangedCallback;

	public void Awake () {
		s = this;
		cardBackID = PlayerPrefs.GetInt("cardBackID", 0);
		cardBorderID = PlayerPrefs.GetInt("cardBorderID", 0);
	}

	public Sprite GetCardBack () {
		return cardBacks[cardBackID];
	}

	public Sprite GetCardBorder () {
		return cardBorders[cardBorderID];
	}

	public void SetCardBack (int id) {
		cardBackID = id;
		PlayerPrefs.SetInt("cardBackID", cardBackID);
		if (cardSpriteChangedCallback != null)
			cardSpriteChangedCallback.Invoke();
	}

	public void SetCardBorder (int id) {
		cardBorderID = id;
		PlayerPrefs.SetInt("cardBorderID", cardBorderID);
		if (cardSpriteChangedCallback != null)
			cardSpriteChangedCallback.Invoke();
	}
}
