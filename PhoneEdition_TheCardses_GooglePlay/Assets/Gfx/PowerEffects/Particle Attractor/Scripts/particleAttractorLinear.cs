using System.Collections;
using UnityEngine;
[RequireComponent(typeof(ParticleSystem))]
public class particleAttractorLinear : MonoBehaviour {
	ParticleSystem ps;
	ParticleSystem.Particle[] m_Particles;
	public Transform target;
	public float activeSpeed = 2f;
	public float counter = 0f;
	public float multiplier = 2f;

	public bool selfReset = false;
	public float effectTime = 2f;

	public AnimationCurve myCurve;

	float [] rnd;

	int numParticlesAlive;
	void Start () {
		ps = GetComponent<ParticleSystem>();
		if (!GetComponent<Transform>()){
			GetComponent<Transform>();
		}
		ResetSelf ();

		rnd = new float [ps.main.maxParticles];
		for (int i = 0; i < rnd.Length; i++) {
			rnd [i] = Random.Range (-1f, 0f);
		}
		m_Particles = new ParticleSystem.Particle [ps.main.maxParticles];
		numParticlesAlive = ps.GetParticles (m_Particles);
	}
	void Update () {
		//m_Particles = new ParticleSystem.Particle[ps.main.maxParticles];
		numParticlesAlive = ps.GetParticles(m_Particles);


		counter += Time.deltaTime;
		//print ((m_Particles [0].startLifetime - m_Particles [0].remainingLifetime));

		activeSpeed = 10;
		for (int i = 0; i < numParticlesAlive; i++) {
			activeSpeed = myCurve.Evaluate ((rnd [i] + (m_Particles [i].startLifetime - m_Particles [i].remainingLifetime)) * multiplier);
			m_Particles [i].position = Vector3.MoveTowards(m_Particles[i].position, target.position, activeSpeed * Time.deltaTime);
		}
		ps.SetParticles(m_Particles, numParticlesAlive);
	}

	private void ResetSelf () {
		counter = 0;
		if(selfReset)
		Invoke ("ResetSelf", effectTime);
	}
}
