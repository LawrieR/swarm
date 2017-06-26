using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parrallax : MonoBehaviour {
    public GameObject camera;
    public float distance = 0.15f;
	// Use this for initialization
	void Start () {
        camera = Camera.main.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
        gameObject.transform.position = camera.transform.position * distance;
	}
}
