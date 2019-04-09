using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicObjectiveGfx : MonoBehaviour {

	public static BasicObjectiveGfx s;

	public Image img;
	public Text txt;


	Animator anim;
	AudioSource aud;
	// Use this for initialization
	void Awake () {
		s = this;
		anim = GetComponent<Animator> ();
		aud = GetComponent<AudioSource> ();
	}
	

	public void Open (int cardId, string text){
		txt.text = text;
		cardId = (int)Mathf.Clamp (cardId, 0, GS.a.gfxs.cardSprites.Length);
		img.sprite = GS.a.gfxs.cardSprites [cardId];
		anim.SetBool ("isOpen",true);
		aud.Play ();
	}

	public void Close (){
		anim.SetBool ("isOpen",false);
	}
}
