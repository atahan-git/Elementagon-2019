using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DragonPanel : MonoBehaviour {

	public int _type = -1;
	public int type {
		get{ 
			return _type;
		}
		set{ 
			_type = value;
			bg.color = colors [type - 8];
			myImage.sprite = GS.a.gfxs.cardSprites [type];
		}
	}


	int _count = -1;
	public int count {
		get{ 
			return _count;
		}
		set{ 
			_count = value;
			myText.text = _count.ToString();
		}
	}

	public Image myImage;
	public Text myText;
	public Image bg;

	public Color[] colors;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ActivatePowerUp () {
		//PowerUpManager.s.PowerUpButton (type);
	}

	public void UpdateGraphics () {
		myImage.sprite = GS.a.gfxs.cardSprites [type];
		myText.text = count.ToString();
		bg.color = colors [type - 8];
	}
}
