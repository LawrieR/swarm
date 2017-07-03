using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class homingRocket : MonoBehaviour {
    public bool vsPlayer = true;
    PlayerControl target;

    public Vector3 velocity = Vector3.zero;
    public float acceleration = 10;
    public float maxSpeed = 7;
    public float drag = 1;
    public float rotateSpeed = 10;
    public float engageRange = 200;
    //arming radius is how close a missile has to get before it explodes. 
    public float armingRadius = 50;
    private float maxLife = 5;
    private float spawnTime;

	// Use this for initialization
	void Start () {
        spawnTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.time - spawnTime > maxLife)
        {
            Explode();
        }

        if (target == null) {
            if (vsPlayer) {
                target = PlayerControl.player;
            } else {
                //find nearest enemy target
            }
        } else {
            Vector3 inputDir = (target.transform.position - transform.position);

            if (inputDir.magnitude < armingRadius)
            {
                DamageTarget();
                Explode();
            }

            inputDir.Normalize();

            //look towards target
            Vector3 newDir = Vector3.RotateTowards(transform.forward, inputDir, rotateSpeed * Time.deltaTime, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDir);

            //accelerate towards look rotation
            velocity.x += transform.forward.x * acceleration * Time.deltaTime;
            velocity.z += transform.forward.z * acceleration * Time.deltaTime;

            //apply drag
            velocity.x = velocity.x * (1.0f - drag * Time.deltaTime);
            velocity.z = velocity.z * (1.0f - drag * Time.deltaTime);

            //cap move speed
            if (velocity.magnitude > maxSpeed)
            {
                velocity.Normalize();
                velocity = velocity * maxSpeed;
            }

            //move
            gameObject.transform.Translate(velocity, Space.World);
            //gameObject.transform.rotation.SetLookRotation(velocity);

        }
	}

    //todo pass in target instead, make a health/target class that enemies and player can share
    void DamageTarget ()
    {
        target.Damage(0.1f);
    }

    void Explode()
    {
        Destroy(gameObject);
    }
}
