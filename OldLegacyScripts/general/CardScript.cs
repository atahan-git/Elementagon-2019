using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CardScript : NetworkBehaviour {

	//--------CARD TYPES---------
	// 0 = empty / already gotten
	// 1-7 = normal cards
	// 8-14 = dragons
	//---------------------------
	//1 = Earth
	//2 = Fire
	//3 = Ice
	//4 = Light
	//5 = Nether
	//6 = Poison
	//7 = Shadow
	//---------------------------
	// 8 = Earth Dragon
	// 9 = Fire Dragon
	//10 = Ice Dragon
	//11 = Light Dragon
	//12 = Nether Dragon
	//13 = Poison Dragon
	//14 = Shadow Dragon
	//---------------------------

	public Vector3 mypos;
	public bool isPoison;
	public void SetState(bool gey){
	}
	public void SetType(int geey){
	}
	public int poisonGiver;
	public void setDirtyBitses(){}

	int _cardType = 0;
	[SerializeField]
	public int cardType{
		get{
			return _cardType;
		}
		set{
			_cardType = value;
			UpdateCardType ();
		}
	}
	public bool isSelected = false;
	public SpriteRenderer sRend;
	Animator anim;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
	}

	public void SelectCard () {

	}

	public void UnSelectCard () {

	}

	public void MatchCard () {

	}

	public void ReInitiateCard () {

	}

	void UpdateCardType () {

	}
}
