using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

    public Vector3 velocity = new Vector3();
    public float acceleration =3f;
    public float drag = 0.2f;
    public float rotateSpeed = 5;
    public float maxSpeed = 10.0f;
    public float directBoostMin = -0.8f;
    public float oppositeBoost = 2.0f;
    public float boostTurnMod = 0.25f;
    public float boostAccelerationMod = 3.0f;
    public ParticleSystem particles;
    public static GameObject player;

	// Use this for initialization
	void Start () {
        //gameObject.GetComponent<ParticleSystem>();
        player = gameObject;
	}
	
	// Update is called once per frame
	void Update () {
        bool boost = Input.GetAxis("Fire1") > 0.5f;
        var em = particles.emission;

        Vector3 inputDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        inputDir.Normalize();

        if (Vector3.Dot(inputDir.normalized, gameObject.transform.forward) > 0.9f || velocity.magnitude < 1.0f || boost) {
            //if (!particles.isPlaying) { 
                //particles.Play(); 

            em.enabled = true;
            var emMain = particles.main;
            emMain.startSpeed = -10-velocity.magnitude*2.0f;
            //}


            float tempAcceleration = acceleration;

            if (Vector3.Dot(inputDir.normalized, velocity) < directBoostMin) {
                tempAcceleration = tempAcceleration * oppositeBoost;
            }

            if (boost) {
                velocity.x += transform.forward.x * acceleration * boostAccelerationMod * Time.deltaTime;
                velocity.z += transform.forward.z * acceleration * boostAccelerationMod * Time.deltaTime;

                //float currentSpeed = velocity.magnitude;
                //velocity = inputDir * currentSpeed;
            } else {
                velocity.x += Input.GetAxis("Horizontal") * tempAcceleration * Time.deltaTime;
                velocity.z += Input.GetAxis("Vertical") * tempAcceleration * Time.deltaTime;
            }
        } else {
            //particles.Stop();
            //particles.emission.enabled = false;
            em.enabled = false;
        }

        if (velocity.magnitude > maxSpeed) {
            velocity.Normalize();
            velocity = velocity * maxSpeed;
        }

        velocity.x = velocity.x * (1.0f - drag * Time.deltaTime);
        velocity.z = velocity.z * (1.0f - drag * Time.deltaTime);

        gameObject.transform.Translate(velocity, Space.World);
        

        gameObject.transform.rotation.SetLookRotation(velocity);

        float tempRotSpeed = rotateSpeed;

        if (boost) {
            tempRotSpeed *= boostTurnMod;
        }

        float tempVelocityTurnDampen = (1-(Mathf.Clamp(velocity.magnitude-3, 0, 7)/10));

        if (boost) {
            tempRotSpeed *= tempVelocityTurnDampen;
        }

        Vector3 newDir = Vector3.RotateTowards(transform.forward, inputDir , tempRotSpeed*Time.deltaTime, 0.0f);
        Debug.DrawRay(transform.position, newDir*10, Color.red);
        transform.rotation = Quaternion.LookRotation(newDir);
	}
}
