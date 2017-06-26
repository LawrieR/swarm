using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gun : MonoBehaviour {
    float lastShot = 0;
    public float rof = 10f;
    public float inaccuracy = 0.3f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetAxis("Fire2") > 0.5f) {
            if (Time.fixedTime - lastShot > rof) {
                lastShot = Time.fixedTime;
                GameObject bullet = (GameObject)Instantiate(Resources.Load("Bullet"));
                bullet.transform.position = transform.position;
                bullet.transform.rotation = transform.rotation;
                bullet.transform.Rotate(0, Random.Range(-inaccuracy, inaccuracy), 0);
            }
        }
	}


}
