using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed;
    public float gravity;
    public float jumpSpeed;
	
	float normalSpeed; // do not modify this fixed speed
    float crouchSpeed;

    public float maxUpDown;
    float limitVertCameraRotation = 0;

    Camera mainCam;

    Vector3 moveDirection = Vector3.zero;

    GameObject spawn;

    CharacterController cc;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        mainCam = GetComponentInChildren<Camera>();
        normalSpeed = walkSpeed;
        crouchSpeed = walkSpeed / 2;
    }

    // Update is called once per frame
    void Update()
    {
    	Cursor.lockState = CursorLockMode.Locked;
    	//give the player to freely look around with mouse.
    	transform.Rotate(0, Input.GetAxis("Mouse X"), 0);

    	limitVertCameraRotation -= Input.GetAxis("Mouse Y");
    	limitVertCameraRotation = Mathf.Clamp(limitVertCameraRotation, -maxUpDown, maxUpDown);
    	mainCam.transform.localRotation = Quaternion.Euler(limitVertCameraRotation, 0, 0);

    	moveDirection = (new Vector3 (Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"))) * walkSpeed;

    	//when the player hit the crouch button
    	//take character controller's height and center to 1/2 the size.
    	//when the player lets go of crouch button revert back to original size.
    	if (Input.GetButtonDown("Crouch"))
    	{
    		cc.height = cc.height/2;
    		cc.center = new Vector3 (0f, cc.center.y / 2, 0f);
    		mainCam.transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
    		walkSpeed = crouchSpeed;
    	}

    	if (Input.GetButtonUp("Crouch"))
    	{
    		cc.height = cc.height * 2;
    		cc.center = new Vector3 (0f, cc.center.y * 2, 0f);
    		mainCam.transform.position = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
    		walkSpeed = normalSpeed;
    	}

    	if (Input.GetButtonDown("Sprinting"))
    	{
    		walkSpeed = normalSpeed * 2;
    	}

    	if (Input.GetButtonUp("Sprinting"))
    	{
    		walkSpeed = normalSpeed;
    	}

    	//when the player interacts with an object
    	//use vector3.forward to make it look like its holding the object.

		moveDirection.y -= gravity * Time.deltaTime;
    	cc.Move(transform.rotation * moveDirection * Time.deltaTime);
    }
}
