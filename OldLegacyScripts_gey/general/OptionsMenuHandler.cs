using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OptionsMenuHandler : MonoBehaviour {

	/*public Text textGridSizeX;
	public Slider sliderGridSizeX;
	public Text textGridSizeY;
	public Slider sliderGridSizeY;*/

	public CardGenerator cardGen;

	public Toggle timeAt;
	public Toggle score;

	public bool isTimeAttack = false;
	bool check = false;

	public InputField textModeSetMinute;
	public InputField textModeSetSeconds;
	public Text textModeSetMiddle;
	public Slider sliderModeSet;

	public Vector2 small = new Vector2 (6, 3);
	public Vector2 medium = new Vector2 (8, 4);
	public Vector2 large = new Vector2 (12, 4);

	void Awake () {

		DataHandler_LEGACY.gridSizeX = (int)medium.x;
		DataHandler_LEGACY.gridSizeY = (int)medium.y;

	}

	// Use this for initialization
	void Start () {
		UpdateModeSetSlider ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnSliderChange(){
		UpdateValues ();
	}

	public void OnToggleChange(){
		//print ("called");
		if (isTimeAttack) {
			timeAt.isOn = false;
			score.isOn = true;
		} else {
			timeAt.isOn = true;
			score.isOn = false;
		}

		if (check) {
			isTimeAttack = !isTimeAttack;
			UpdateModeSetSlider ();
			check = false;
		} else {
			check = true;
		}
	}

	void UpdateModeSetSlider(){
		if (!isTimeAttack) {
			sliderModeSet.maxValue = 100;
			sliderModeSet.minValue = 5;
			sliderModeSet.value = 30;
		} else {
			sliderModeSet.maxValue = 300;
			sliderModeSet.minValue = 10;
			sliderModeSet.value = 180;
		}

		UpdateValues ();
	}


	void UpdateValues(){

		/*textGridSizeX.text = sliderGridSizeX.value.ToString();
		textGridSizeY.text = sliderGridSizeY.value.ToString();*/

		if (!isTimeAttack) {
			
			textModeSetSeconds.text = sliderModeSet.value.ToString ();
			/*textModeSetMiddle.text = "";
			textModeSetMinute.text = "";*/
			textModeSetMiddle.gameObject.SetActive (false);
			textModeSetMinute.gameObject.SetActive (false);

		} else {

			textModeSetMiddle.gameObject.SetActive (true);
			textModeSetMinute.gameObject.SetActive (true);
			textModeSetMiddle.text = ":";

			int minuteCount = (int)(sliderModeSet.value / 60);
			int secondCount = (int)(sliderModeSet.value - (minuteCount * 60));

			textModeSetMinute.text = minuteCount.ToString ();
			if (secondCount < 10) {
				textModeSetSeconds.text = "0" + secondCount.ToString ();
			} else {
				textModeSetSeconds.text = secondCount.ToString ();
			}
		}
	}

	public void UpdateTextBoxSecond(){

		if (!isTimeAttack) {
			int value = -5;

			int.TryParse (textModeSetSeconds.text, out value);
			sliderModeSet.value = value;
		} else {

			int value = -5;

			int.TryParse (textModeSetSeconds.text, out value);
			int newSecondCount = value;
			newSecondCount = Mathf.Clamp (newSecondCount, 0, 59);
			//print (value + " - " + newSecondCount);

			int minuteCount = (int)(sliderModeSet.value / 60);
			int secondCount = (int)(sliderModeSet.value - (minuteCount * 60));

			//print (sliderModeSet.value.ToString() + " - " + secondCount.ToString() + " - " + newSecondCount.ToString());

			sliderModeSet.value = ((int)sliderModeSet.value - secondCount) + newSecondCount;
		}

		UpdateValues ();
	}

	public void UpdateTextBoxMinute(){

		int value = -5;

		int.TryParse (textModeSetMinute.text, out value);
		int newMinuteCount = value;

		int minuteCount = (int)(sliderModeSet.value / 60);
		int secondCount = (int)(sliderModeSet.value - (minuteCount * 60));

		sliderModeSet.value = (newMinuteCount * 60) + secondCount;

		UpdateValues ();
	}

	public void Large(){
		SetGrid (large);
	}

	public void Medium(){
		SetGrid (medium);
	}

	public void Small(){
		SetGrid (small);
	}

	void SetGrid (Vector2 values){
		DataHandler_LEGACY.gridSizeX = (int)values.x;
		DataHandler_LEGACY.gridSizeY = (int)values.y;
		cardGen.gridSizeX = (int)values.x;
		cardGen.gridSizeY = (int)values.y;
		cardGen.SetUpGrid ();
	}

}
