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
	public float maxSpeed = 50f;
	public float differenceMultiplier = 0.2f;

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
			activeSpeed = myCurve.Evaluate ((rnd[i] + (m_Particles[i].startLifetime - m_Particles[i].remainingLifetime)) * multiplier);
			if (target != null) {
				//m_Particles[i].position = Vector3.MoveTowards (m_Particles[i].position, target.position, activeSpeed * Time.deltaTime);
				Vector3 targetVelocity = (target.position - m_Particles[i].position).normalized * maxSpeed;
				m_Particles[i].velocity = Vector3.MoveTowards (m_Particles[i].velocity, targetVelocity, 
					(Vector3.Distance(m_Particles[i].velocity, targetVelocity) * differenceMultiplier)*activeSpeed * Time.deltaTime);
			}
		}
		ps.SetParticles(m_Particles, numParticlesAlive);
	}

	private void ResetSelf () {
		counter = 0;
		ps.Stop ();
		ps.Play ();
		if(selfReset)
		Invoke ("ResetSelf", effectTime);
	}
}
