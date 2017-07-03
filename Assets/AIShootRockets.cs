using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIShootRockets : MonoBehaviour {

    private float rof = 1;
    private float lastFired;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.time - lastFired > rof)
        {
            FireMissile();
        }
	}

    void FireMissile()
    {
        lastFired = Time.time;
        Instantiate(Resources.Load("HMissile"), transform.position, Quaternion.identity);
    }
}
