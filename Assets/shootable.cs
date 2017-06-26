using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shootable : MonoBehaviour {
    public float hp = 10;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (hp < 0) {
            Destroy(gameObject);
        }
	}

    public void damage() {
        Debug.Log("damaged");
        hp--;
    }
}
