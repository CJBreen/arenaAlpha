using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Mouse Sensitivity")]
    public float mouseSens_X;
    public float mouseSens_Y;

    [Header("Player Movement")]
    public float forwardSpeed;
    public float strafeSpeed;
    public float maxSpeed;
    [Range(1, 3)]
    public float decelerationMultiplier = 2f;
    public float jumpPower;
    public float jumpCooldown;
    [Header("Gravity of player when normal & falling, extremely low values only")]
    [Range(0, 3)]
    public float playerNegativeGravity = 1f;
    [Range(0, 3)]
    public float playerNormalGravity = 1f;

    [Header("Camera")]
    public float cameraOffset_Y;

    [Header("Other")]
    public LayerMask groundDef;
    public float playerHeight;
    public KeyCode jumpKey = KeyCode.Space;
    public float fieldOfView = 60f;


    private Rigidbody playerRb;
    private float rotation_X;
    private float rotation_Y;
    private Transform cameraTransform;
    private float groundCheck_heightOffset = 0.7f;
    private bool isJump;
    private bool isOnGround;
    private GUI guiScript;
    
    void Start() {

        // Locking the mouse cursor to the center of the screen and making it invisible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerRb = GetComponent<Rigidbody>();
        guiScript = GameObject.Find("GUI").GetComponent<GUI>();
        cameraTransform = GameObject.Find("Main Camera").transform;
        Camera.main.fieldOfView = fieldOfView;

        // Setting Rigidbody settings
        playerRb.mass = 0.1f;
        playerRb.drag = 0;
        playerRb.freezeRotation = true;
    }

    void Update() {
        if (!guiScript.isEsc) {
            // Player rotation
            rotatePlayer();
            // Camera rotation
            cameraController();
        }
    }

    void FixedUpdate() {
        if (!guiScript.isEsc) {
            // Checking if the player is on the ground
            isOnGround = Physics.Raycast(transform.position, Vector3.down, playerHeight*groundCheck_heightOffset, groundDef);
            // Moving the player
            movePlayer();
            // Speedlimiting the player
            playerSpeed();
            // The player's jump
            Jump();
            // The player's gravity
            playerGravity();
        }
        
    }

    private void movePlayer() {
        float horizontalInput = Input.GetAxis("Horizontal") * Time.fixedDeltaTime;
        float verticalInput = Input.GetAxis("Vertical") * Time.fixedDeltaTime;

        // Calculating the total move direction of the player
        Vector3 moveDirection = transform.forward * verticalInput * forwardSpeed + transform.right * horizontalInput * strafeSpeed;
        // This is required so that the player can only move on the X & Y axis
        moveDirection.y = 0;
        // This is checking if the player is pressing any buttons, if they are not, the player will rapidly slow down, its drag but better
        if (moveDirection.magnitude == 0) {
            Vector3 newVelocity = playerRb.velocity/decelerationMultiplier; // Very low numbers of deceleration required, maybe instead of dividing, change it to "-"
            newVelocity.y = playerRb.velocity.y;    // Making it so that the speed limiter doesnt touch the players vertical velocity
            playerRb.velocity = newVelocity;    // setting the players new velocity
        }
        else {
            playerRb.AddForce(moveDirection*10f, ForceMode.Force);  // If the player is pressing buttons, move
        }
    }
    private void rotatePlayer() {
        // Getting mouse input
        float mouse_X = Input.GetAxis("Mouse X") * Time.fixedDeltaTime * mouseSens_X;
        float mouse_Y = Input.GetAxis("Mouse Y") * Time.fixedDeltaTime * mouseSens_Y;

        rotation_X += mouse_X;

        // Used in the CameraController method
        rotation_Y -= mouse_Y;  // Change this to a += instead to inverse mouse controls
        rotation_Y = Mathf.Clamp(rotation_Y, -90f, 90f);    // Making it so that the player cant look more than 90 degrees up and down

        transform.rotation = Quaternion.Euler(0, rotation_X, 0);    // Rotating the player model on the X axis
    }

    private void cameraController() {
        // Setting the cameras position to the player + a Y value so that you can have the camera in the players head...
        cameraTransform.position = transform.position + Vector3.up * cameraOffset_Y;
        // Rotating the camera on the X & Y axis
        cameraTransform.rotation = Quaternion.Euler(rotation_Y, rotation_X, 0);
    }
    private void playerSpeed() {
        Vector3 playerVelocity = new Vector3(playerRb.velocity.x, 0f, playerRb.velocity.z);
        if (playerVelocity.magnitude > maxSpeed) {
            Vector3 fixedVelocity = playerVelocity.normalized * maxSpeed;
            fixedVelocity.y = playerRb.velocity.y;
            playerRb.velocity = fixedVelocity;
        }
    }
    private void Jump() {
        // Player jump method
        if (Input.GetKey(jumpKey) && !isJump && isOnGround) {
            isJump = true;
            playerRb.velocity = new Vector3(playerRb.velocity.x, 0 , playerRb.velocity.z); // making the vertical velocity for the player 0 since otherwise you could stack jumps
            playerRb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            Invoke("jumpReset", jumpCooldown);  // invoking the method to reset the jump with a delay of jumpCooldown
        }
    }
    private void jumpReset() {
        isJump = false;
    }
    // A custom "Gravity" implementation since the alternative is garb
    private void playerGravity() {
        Vector3 newPlayerVerticalSpeed = playerRb.velocity; // Getting initial player velocity

        // Checking if the player is not on the ground and they are falling (aka in the air and falling)
        if (!isOnGround && Mathf.Sign(playerRb.velocity.y) == -1) {
            // increasing the rate the player is falling
            newPlayerVerticalSpeed.y = playerRb.velocity.y - playerNegativeGravity;
        }
        else {
            // increasing the rate the player is falling under normal conditions, aka either jumping or stationary
            newPlayerVerticalSpeed.y = playerRb.velocity.y - playerNormalGravity;
        }
        // setting the player's new velocity that is generated above
        playerRb.velocity = newPlayerVerticalSpeed;
    }
}
