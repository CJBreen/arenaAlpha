using UnityEngine;

public class GrapplingGun : MonoBehaviour
{
    // Variables
    // For some reason my camera transform variable displays as green (not used) despite me using it below
    private LineRenderer lineRender;
    private Vector3 grapplePoint;
    public LayerMask whatIsGrappleable;
    public Transform gunTip, camera, player;
    private float maxDistance = 100f;
    private SpringJoint joint;
    public float distanceFromPoint;
    private Vector3 currentGrapplePosition;
    public static bool gamePaused = false;
    public AudioClip grappleSound;
    public AudioSource speaker;

    // Awake method
    void Awake()
    {
        lineRender = GetComponent<LineRenderer>(); // Renders the line for the grapple animation
    }

    // Update method
    void Update()
    {
        if (gamePaused == false) // If the game is not paused
        {
            if (Input.GetMouseButtonDown(0)) // If left click is held
            {
                StartGrapple(); // Start grapple method
            }
            else if (Input.GetMouseButtonUp(0)) // If left click is released
            {
                StopGrapple(); // Stop grapple method
            }
        }
    }

    // Late Update method
    void LateUpdate()
    {
        DrawRope(); // Method for displaying grappling rope
    }

    // Start Grapple method
    void StartGrapple()
    {
        RaycastHit hit;
        
        speaker.enabled = true; // Enables the speaker so the user can hear the sound effect
        
        if (Physics.Raycast(camera.position, camera.forward, out hit, maxDistance, whatIsGrappleable)) // If the player can grapple
        {
            speaker.PlayOneShot(grappleSound, 3f); // Plays sound affect
            grapplePoint = hit.point; // Sets grapple point to where the user hits
            joint = player.gameObject.AddComponent<SpringJoint>(); // Creates spring point between player and grapple location
            joint.autoConfigureConnectedAnchor = false; // Sets an anchor
            joint.connectedAnchor = grapplePoint; // Sets anchor to grapple point

            distanceFromPoint = Vector3.Distance(player.position, grapplePoint); // Find distance between player and grapple point

            joint.maxDistance = distanceFromPoint * 0.5f; // Minimum distance player will be kept from grapple point
            joint.minDistance = distanceFromPoint * 0.1f; // Maximum distance player will be kept from grapple point

            // These can be adjusted very easily to change the way the grappling gun works
            joint.spring = 5f; // Spring
            joint.damper = 5f; // Damper
            joint.massScale = 10f; // Mass

            lineRender.positionCount = 2; // Renders the rope
            currentGrapplePosition = gunTip.position; // Sets it to the end of the grappling gun model
        }
    }


    // Stop Grapple method
    void StopGrapple()
    {
        lineRender.positionCount = 0; // Removes rope texture
        Destroy(joint); // Destroys the joint between the player and the grapple point
    }

    // Draw Rope method
    void DrawRope()
    {
        if (!joint) return; // If the player creates a joint between itself and the grapple point
        {
            currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, grapplePoint, Time.deltaTime * 8f); // Gets current position

            lineRender.SetPosition(0, gunTip.position); // Renders the rope
            lineRender.SetPosition(1, currentGrapplePosition);
        }
    }

    // Is Grappling method
    public bool IsGrappling()
    {
        return joint != null; // Checks if the player is grappling
    }
}
