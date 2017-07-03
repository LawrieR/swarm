using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour {
    float startTime;
    float speed = 1000f;
    Rigidbody body;
	// Use this for initialization
	void Start () {
        startTime = Time.fixedTime;
        body = GetComponent<Rigidbody>();
        body.AddForce(transform.forward * speed, ForceMode.Impulse);
        body.AddRelativeForce(new Vector3(Random.Range(-10,10),Random.Range(-10,10),Random.Range(-10,10)));
	}
	
	// Update is called once per frame
	void Update () {
        //transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);

        if (Time.fixedTime - startTime > 1) {
            destroyThis();
        }
	}

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Player") {
            startTime -= 0.5f;
            //destroyThis();
            collision.gameObject.GetComponent<shootable>().damage();
        } else { 
            
        }
        //if (collision.relativeVelocity.magnitude > 2)
        //audio.Play();
    }

    void destroyThis() {
        Destroy(gameObject);
    }
}
