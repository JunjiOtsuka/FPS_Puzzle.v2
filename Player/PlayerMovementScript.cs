using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    public CharacterController controller;

    public PlayerCamera PC;
    public GroundDetector GD;
    public WallDetection WD;
    public InteractDetector ID;
    public GrappleDetector GrD;

    public enum PlayerState {
        idle, walk, jump, wallStick, wallClimb, wallRun, wallJump, midAir, zipline, zipJump, grappling
    };
    public PlayerState state;

    public float gravity;
    public float speed = 12;
    public float jumpHeight;
    public float wallRunCameraTilt, maxWallRunCameraTilt, wallRunningCameraTiltMultiplier;

    public Vector3 move;
    public Vector3 velocity;

    bool wallRunning;
    bool wallStick;
    bool isInteracting;
    bool zipJump;

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalSpeed = new Vector3(controller.velocity.x, 0, controller.velocity.z).magnitude;

        if (GD.isGrounded) {
            if (velocity.y < 0) {
                state = PlayerState.idle;
            }

            if ((horizontalInput != 0 || verticalInput != 0)) {
                state = PlayerState.walk;
            }

            if (Input.GetButtonDown("Jump")) {
                state = PlayerState.jump;
            }
        }


        if (!GD.isGrounded) {
            if (!wallStick && !wallRunning && !WD.rightWall && !WD.leftWall && !WD.frontWall && !WD.backWall && !isInteracting || state == PlayerState.jump) {
                state = PlayerState.midAir;
            }

            if (Input.GetButtonDown("Jump")) {
                if((WD.rightWall || WD.leftWall)) { 
                    state = PlayerState.wallRun;
                }

                if (WD.frontWall) {
                    state = PlayerState.wallStick;
                }

                if (wallRunning) {
                    state = PlayerState.wallJump;
                } 
            }
        }

        if (ID.isInteracting) {
            if (Input.GetButtonDown("Interact")) {
                state = PlayerState.zipline;
            }
        }

        if (GrD.isGrappling && Input.GetButtonDown("Fire1")) {
            state = PlayerState.grappling;
        }

        if (state == PlayerState.zipline && Input.GetButtonDown("Jump")) {
            state = PlayerState.zipJump;
        }

        if (wallStick) {
            if (verticalInput != 0) {
                state = PlayerState.wallClimb;
            }
            if (verticalInput == 0 || !WD.frontWall) {
                state = PlayerState.wallStick;
            }
            if (WD.rightWall || WD.leftWall || WD.backWall) {
                if (Input.GetButtonDown("Jump")) {
                    state = PlayerState.wallJump;
                }
            }
            if (!WD.rightWall && !WD.leftWall && !WD.backWall && !WD.frontWall) {
                state = PlayerState.midAir;
            }
            if (GD.isGrounded){
                state = PlayerState.idle;
            }
        } 

        PC.transform.localRotation = Quaternion.Euler(Mathf.Clamp(PC.xRotation, -90, 90), PC.transform.localRotation.eulerAngles.y + PC.mouseX, wallRunCameraTilt);

        // PC.playerBody.Rotate(Vector3.up * PC.mouseX);

        if (wallRunning) {
            if (!WD.rightWall && !WD.leftWall && !WD.backWall && !WD.frontWall) {
                wallRunning = false;
                state = PlayerState.wallJump;
            }

            if (Mathf.Abs(wallRunCameraTilt) < maxWallRunCameraTilt){
                if (WD.rightWall) {
                    wallRunCameraTilt += Time.deltaTime * maxWallRunCameraTilt * wallRunningCameraTiltMultiplier;
                    if (Mathf.Abs(wallRunCameraTilt) > maxWallRunCameraTilt) {
                        wallRunCameraTilt = maxWallRunCameraTilt;
                    }
                }
                if (WD.leftWall) {
                    wallRunCameraTilt -= Time.deltaTime * maxWallRunCameraTilt * wallRunningCameraTiltMultiplier;
                    if (Mathf.Abs(wallRunCameraTilt) > maxWallRunCameraTilt) {
                        wallRunCameraTilt = -maxWallRunCameraTilt;
                    }
                }
            }

            if (Mathf.Abs(wallRunCameraTilt) == maxWallRunCameraTilt){
                if (WD.rightWall) {
                    wallRunCameraTilt += Time.deltaTime * maxWallRunCameraTilt * wallRunningCameraTiltMultiplier;
                    if (Mathf.Abs(wallRunCameraTilt) > maxWallRunCameraTilt) {
                        wallRunCameraTilt = maxWallRunCameraTilt;
                    }
                }
                if (WD.leftWall) {
                    wallRunCameraTilt -= Time.deltaTime * maxWallRunCameraTilt * wallRunningCameraTiltMultiplier;
                    if (Mathf.Abs(wallRunCameraTilt) > maxWallRunCameraTilt) {
                        wallRunCameraTilt = -maxWallRunCameraTilt;
                    }
                }
            }
        }

        if (!wallRunning){
            if (wallRunCameraTilt > 0) {
                wallRunCameraTilt -= Time.deltaTime * maxWallRunCameraTilt * wallRunningCameraTiltMultiplier;
                if (!GD.isGrounded || state == PlayerState.midAir) {
                    wallRunCameraTilt = 0;
                }
            }
            if (wallRunCameraTilt < 0) {
                wallRunCameraTilt += Time.deltaTime * maxWallRunCameraTilt * wallRunningCameraTiltMultiplier;
            }
        }

        switch (state) {
            case PlayerState.idle:
            {
                executeIdle();
                break;
            }
            case PlayerState.walk:
            {
                executeWalk(horizontalInput, verticalInput, horizontalSpeed);
                break;
            }
            case PlayerState.jump:
            {
                executeJump();
                break;
            }
            case PlayerState.wallStick:
            {
                executeWallStick(horizontalInput, verticalInput);
                break;
            }
            case PlayerState.wallClimb:
            {
                executeWallClimb(horizontalInput, verticalInput);
                break;
            }
            case PlayerState.wallRun:
            {
                executeWallRun();
                break;
            }
            case PlayerState.wallJump:
            {
                executeWallJump();
                break;
            }
            case PlayerState.midAir:
            {
                executeMidAir(horizontalInput, verticalInput, horizontalSpeed);
                break;
            }
            case PlayerState.zipline:
            {
                executeZipline();
                break;
            }
            case PlayerState.zipJump:
            {
                executeZipJump();
                break;
            }
            case PlayerState.grappling:
            {
                executeGrapple();
                break;
            }
            default:
                break;
        }
    }

    void resetBool () {
        if (wallRunning){
            wallRunning = false;
        }

        if (wallStick) {
            wallStick = false;
        }

        if (isInteracting) {
            isInteracting = false;
        }
    }

    void executeIdle() {
        resetBool();

        velocity = new Vector3 (0f, -2f, 0f);

        //this is gravity
        velocity.y += gravity * 1 * Time.deltaTime;

        //final velocity
        controller.Move(velocity * Time.deltaTime);
    }

    void executeWalk(float horizontalInput, float verticalInput, float horizontalSpeed) {
        //jump calculation v = sqrt (h x -2 x g)
        if (horizontalSpeed == 0) {
            move = transform.right * horizontalInput + transform.forward * verticalInput;
        } else if (horizontalSpeed > 0) {
        //if crouched
            move = transform.right * horizontalInput;
        }

        move.Normalize();

        controller.Move(move * speed * Time.deltaTime);

        Vector3 localVelocity = transform.InverseTransformDirection(velocity);
        localVelocity.x = 0;
        velocity = transform.TransformDirection(localVelocity);

        //this is gravity
        velocity.y += gravity * 3f * Time.deltaTime;

        //final velocity
        controller.Move(velocity * Time.deltaTime);
    }

    void executeJump() {
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        controller.Move(velocity * Time.deltaTime);
    }

    void executeGrapple() {

            move = GrD.hit.point - transform.position;
            move.Normalize();
            controller.Move(move * speed * Time.deltaTime);
            controller.Move(velocity * Time.deltaTime);
    }

    void executeZipline() {
        isInteracting = true;

        velocity.y = 0;
        controller.Move(velocity * Time.deltaTime);
    }

    void executeZipJump() {
        resetBool();

        if (isInteracting) {
            velocity = (1.5f * PC.transform.forward - transform.right) * speed * 3f;
        } 

        velocity.y = Mathf.Sqrt((jumpHeight * 2) * -2f * 3f * gravity);
        controller.Move(velocity * Time.deltaTime);
    }

    void executeWallStick(float horizontalInput, float verticalInput) {
        wallStick = true;

        move = transform.right * horizontalInput;

        move.Normalize();

        controller.Move(move * (speed / 5) * Time.deltaTime);

        velocity.y = 0;

        controller.Move(velocity * Time.deltaTime);
    }

    void executeWallClimb(float horizontalInput, float verticalInput) {
        move = transform.right * horizontalInput;

        move.Normalize();

        controller.Move(move * (speed / 5) * Time.deltaTime);

        if (verticalInput > 0) {
            velocity.y = 7f;
        } else if (verticalInput < 0) {
            velocity.y += gravity * 3f * Time.deltaTime;
        }

        controller.Move(velocity * Time.deltaTime);
    }

    void executeWallRun() {
        wallRunning = true;

        if (WD.rightWall) {
            velocity = (1.5f * transform.forward + transform.right)  * speed;
        } else if (WD.leftWall) {
            velocity = (1.5f * transform.forward - transform.right)  * speed;
        } 

        controller.Move(velocity * Time.deltaTime);
    }

    void executeWallJump() {
        resetBool();

        if (WD.rightWall) {
            velocity = (1.5f * transform.forward - transform.right) * speed * 1.5f;
        } else if (WD.leftWall) {
            velocity = (1.5f * transform.forward + transform.right) * speed * 1.5f;
        }else if (WD.frontWall) {
            velocity = -(1.5f * transform.forward)  * speed * 1.5f;
        } else if (WD.backWall) {
            velocity = (1.5f * transform.forward)  * speed * 1.5f;
        }

        velocity.y = Mathf.Sqrt((jumpHeight * 2) * -2f * gravity);
        controller.Move(velocity * Time.deltaTime);
    }

    void executeMidAir(float horizontalInput, float verticalInput, float horizontalSpeed) {
        resetBool();

        //if not crouched
        if (horizontalSpeed == 0) {
            move = transform.right * horizontalInput + transform.forward * verticalInput;
        } else if (horizontalSpeed > 0) {
        //if crouched
            move = transform.right * horizontalInput;
        }

        move.Normalize();

        controller.Move(move * speed * Time.deltaTime);

        Vector3 localVelocity = transform.InverseTransformDirection(velocity);
        localVelocity.x = 0;
        velocity = transform.TransformDirection(localVelocity);

        //this is gravity
        velocity.y += gravity * 3f * Time.deltaTime;

        //final velocity
        controller.Move(velocity * Time.deltaTime);

        //grasshopper like control
        // controller.Move((move + velocity) * speed * Time.deltaTime);

    }
}
