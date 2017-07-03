using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chase : MonoBehaviour {

    public Vector3 velocity = Vector3.zero;
    public float acceleration = 10;
    public float maxSpeed = 7;
    public float drag = 1;
    public float rotateSpeed = 10;
    public float engageRange = 200;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
    void Update () {
        Vector3 inputDir = (PlayerControl.player.transform.position - transform.position);
        if (inputDir.magnitude > 200) {
            inputDir.Normalize();

            if (Vector3.Dot(inputDir.normalized, gameObject.transform.forward) > 0.9f || velocity.magnitude < 1.0f) {

                velocity.x += inputDir.x * acceleration * Time.deltaTime;
                velocity.z += inputDir.z * acceleration * Time.deltaTime;
            }
        }


        if (velocity.magnitude > maxSpeed) {
            velocity.Normalize();
            velocity = velocity * maxSpeed;
        }

        velocity.x = velocity.x * (1.0f - drag * Time.deltaTime);
        velocity.z = velocity.z * (1.0f - drag * Time.deltaTime);

        gameObject.transform.Translate(velocity, Space.World);
        if (velocity.magnitude > 0)
        {
            gameObject.transform.rotation.SetLookRotation(velocity);
        }

        Vector3 newDir = Vector3.RotateTowards(transform.forward, inputDir , rotateSpeed*Time.deltaTime, 0.0f);
        Debug.DrawRay(transform.position, newDir*engageRange, Color.red);
        transform.rotation = Quaternion.LookRotation(newDir);
    }
}
