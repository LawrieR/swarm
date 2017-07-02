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

    
    Plane plane = new Plane(Vector3.up, new Vector3(0,385,0)); //Plane used to help get the mouse position in world space as a 3D point - plane must be at the same height as the player...
    Vector3 m_lastMousePos = Vector3.zero; //Used to track the last position of the mouse - used to determine if the mouse is active also
    bool m_mouseActive; //Flag to check if the mouse is active - If the mouse is not active then we do not need to do expensive calls such as figuring out the movement direction from rays
    Vector3 mouseProjectedPosition = Vector3.zero; //Mouse position as a point in 3D space

    // Use this for initialization
    void Start () {
        //gameObject.GetComponent<ParticleSystem>();
        player = gameObject;
        
	}

    private void OnEnable()
    {
        m_mouseActive = false;
        m_lastMousePos = Input.mousePosition;
    }

    void CheckMouseInUse()
    {
        //HorizontalJoy/VerticalJoy is a duplicate of the default Horizontal for the joystick entry. Edit Prefs Inputs, right click - duplicate, rename done :D 
        bool joystickInUse = (new Vector2(Input.GetAxis("HorizontalJoy"), Input.GetAxis("VerticalJoy")).sqrMagnitude > 0.001f); //Joystick in use?
        bool keysInUse = (new Vector2(Input.GetAxis("HorizontalKey"), Input.GetAxis("VerticalKey")).sqrMagnitude > 0.001f); //Keyboard in use?

        //Turn mouseActive off if the keyboard or joystick is being used //Turn it back on or keep on if the mouse moves
        m_mouseActive = (joystickInUse || keysInUse) ? false : ((m_lastMousePos - Input.mousePosition).sqrMagnitude > 0.001f) ? true : m_mouseActive;
        
        this.m_lastMousePos = Input.mousePosition; //Update the last position after checks
    }

    void SetMousePositionInWorldSpace()
    {
        if (this.m_mouseActive)
        {
            //Cast a ray based on the mouse cursor position
            Ray ray = Camera.main.ScreenPointToRay(this.m_lastMousePos);
            float distance;
            if (plane.Raycast(ray, out distance))
            {
                mouseProjectedPosition = ray.GetPoint(distance);
            }
        }
    }

    // Update is called once per frame
    void Update () {

        CheckMouseInUse();
        SetMousePositionInWorldSpace();
        
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

        Debug.DrawRay(transform.position, newDir*20, Color.red);

        Vector3 heading = mouseProjectedPosition - transform.position;
        float distance = heading.magnitude;
        Vector3 direction = heading / distance; // Normalized direction.

        Vector3 mNewDir = Vector3.RotateTowards(transform.forward, direction, tempRotSpeed * Time.deltaTime, 0.0f);


        Debug.DrawRay(transform.position, direction * 100f, Color.cyan);
        Debug.DrawRay(mouseProjectedPosition, direction * 100f, Color.blue);

        if (m_mouseActive)
        { //If the mouse is active the rotate towards the mouse instead of the keyboard direction
            transform.rotation = Quaternion.LookRotation(mNewDir);
        }
        else
        {
            transform.rotation = Quaternion.LookRotation(newDir);
        }
	}
}
