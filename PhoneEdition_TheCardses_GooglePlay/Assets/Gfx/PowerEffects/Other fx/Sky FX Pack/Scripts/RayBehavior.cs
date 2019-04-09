using UnityEngine;
using System.Collections;

public class RayBehavior : MonoBehaviour 
{
    public GameObject BeginLocation;
    public GameObject EndLocation;

    public Color BeginColor = Color.white;
    public Color EndColor = Color.white;

    public Vector3 PositionRange;


    public float WidthA = 1.0f;
    public float WidthB = 1.0f;

    public float RadiusA = 1.0f;
    public float RadiusB = 1.0f;

    //public float Offset = 1.0f;

    private LineRenderer Line;
    private Animation Anim;

    private bool changed = true;
    private Vector3 Offset;


    public float AlphaCurve;

    public float FadeSpeed = 1.0f;


	// Use this for initialization
    public void ResetRay()
    {
        Offset = new Vector3( Random.Range(-PositionRange.x, PositionRange.x), 
            Random.Range(-PositionRange.y, PositionRange.y),
            Random.Range(-PositionRange.z, PositionRange.z)
            );


		//Offset = Vector3.ClampMagnitude (Offset, PositionRange.x);


        changed = true;
    }

    public void UpdateLineData()
    {
        Line.SetPosition(0, BeginLocation.transform.position + (Offset * RadiusA));
        Line.SetPosition(1, EndLocation.transform.position + (Offset * RadiusB));
        
        Line.SetWidth(WidthA, WidthB);
    }


	void Start () 
    {
        Line = GetComponent<LineRenderer>();
        Anim = GetComponent<Animation>();


        Anim["RayAlphaCurve"].speed = FadeSpeed;        
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (changed)
        {
            changed = false;
            UpdateLineData();
        }

		float mappedAlpha = map (AlphaCurve, 0f, 1f, 0f, BeginColor.a);
        
		Line.SetColors(new Color(BeginColor.r, BeginColor.g, BeginColor.b, mappedAlpha),
			new Color(EndColor.r, EndColor.g, EndColor.b, mappedAlpha));
        

        
        //Line.renderer.material.color = new Color(1, 1, 1, AlphaCurve);
	
	}

	float map(float x, float in_min, float in_max, float out_min, float out_max)
	{
		return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
	}
}
