using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RadialChargeImage : MonoBehaviour {

	public Image myImg;
	public Image myBg;

	public Color activeColor;
	public Color unactiveColor;
	public Color activeColorBG;
	public Color unactiveColorBG;

	public TextMeshProUGUI myName;

	public ItemInfoDisplay myReqs;

	public float maxCharge = 3;
	public float curCharge = 3;

	public bool canActivate = false;
	public bool isActive = false;

	ParticleSystem[] myActiveParticles = new ParticleSystem[0];


	public void SetUp (Sprite mySprite, float _maxCharge, string _name, int dragonID) {
		SetUp (mySprite, _maxCharge, _name, PowerUpManager.s.dragonColors[dragonID]);
	}

	// Use this for initialization
	public void SetUp (Sprite mySprite, float _maxCharge, string _name, Color myColor) {
		myActiveParticles = GetComponentsInChildren<ParticleSystem> ();
		foreach (ParticleSystem sys in myActiveParticles) {
			if (sys != null)
				sys.startColor = myColor;
		}

		myImg.sprite = mySprite;

		if (_maxCharge <= 0) {
			maxCharge = 1;
			curCharge = 1;
		} else {
			maxCharge = _maxCharge;
			curCharge = _maxCharge;
		}

		canActivate = true;
		isActive = false;

		myName.text = _name;

		UpdateGraphics ();
	}

	bool timedActive = false;
	public void SetState (float _maxCharge, float _curCharge, bool _canActivate, bool _isActive) {
		if (_maxCharge < 0) {
			isActive = false;
			CharacterStuffController.s.UpdateChargeReqs ();
			return;
		}

		maxCharge = _maxCharge;
		if (_curCharge >= 0) {
			curCharge = _curCharge;
			timedActive = false;
		} else {
			curCharge = _maxCharge;
			timedActive = true;
		}
		canActivate = _canActivate;
		isActive = _isActive;
		UpdateGraphics ();
	}
	public float fillSpeed = 2f;
	private void Update () {
		myImg.fillAmount = Mathf.MoveTowards (myImg.fillAmount, (float)curCharge / (float)maxCharge, fillSpeed * Time.deltaTime);

		if (isActive && timedActive) {
			curCharge -= Time.deltaTime;

			if (curCharge <= 0) {
				curCharge = 0;
				timedActive = false;
				isActive = false;
				CharacterStuffController.s.UpdateChargeReqs ();
			}
		}
	}

	public void UpdateGraphics () {
		if (canActivate || isActive) {
			myImg.color = activeColor;
			myBg.color = activeColorBG;
		} else {
			myImg.color = unactiveColor;
			myBg.color = unactiveColorBG;
		}

		if (isActive) {
			foreach (ParticleSystem sys in myActiveParticles) {
				if (sys != null)
					sys.Play ();
			}
		} else {
			foreach (ParticleSystem sys in myActiveParticles) {
				if (sys != null)
					sys.Stop ();
			}
		}
	}
}
