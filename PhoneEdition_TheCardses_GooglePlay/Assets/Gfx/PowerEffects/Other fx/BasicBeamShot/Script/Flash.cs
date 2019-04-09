using UnityEngine;
using System.Collections;

public class Flash : MonoBehaviour {

	public float MaxSize = 1.0f;
	public float AnimationSpd = 0.1f;

	private float NowAnm = 0;
	private float t = 0;

	public float time = 1f;
	float delta = 0f;
	bool isEnlarge = true;

	private Light pl;

	// Use this for initialization
	void Start () {
		Vector3 m_scale = new Vector3(5,5,5);
		transform.localScale = m_scale;
		pl = (Light)this.gameObject.GetComponent<Light>();
	}

	float s = 0;
	// Update is called once per frame
	void Update () {
		/*float s = Mathf.Lerp(0,MaxSize,Mathf.Min(t,1.0f));
		s = Mathf.Lerp(s,MaxSize/2,Mathf.Min(t-1.0f,1.0f));
		s = Mathf.Lerp(s,0.0f,NowAnm);
		t+=0.25f;*/
		if (isEnlarge) {
			s = Mathf.Lerp (s, MaxSize, AnimationSpd * Time.deltaTime);
		} else {
			s = Mathf.Lerp (s, 0, AnimationSpd * Time.deltaTime);
		}

		if (delta > time)
			isEnlarge = false;

		delta += Time.deltaTime;

		Vector3 m_scale = new Vector3(s,s,s);
		transform.localScale = m_scale;

		if(pl != null)
		{
			pl.intensity = s*0.1f;
		}

		//NowAnm += AnimationSpd;
		/*if(NowAnm > 1.0f)Destroy(this.gameObject);*/
	}
}
