using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretControllerSniper : MonoBehaviour
{
    public LayerMask wallsDef;
    public float turretViewHeight;
    public float shootTimer;
    public float rotationOffset;

    private Transform playerPos;
    private GameObject player;
    private LineRenderer lineRender;
    private bool isSeePlayer;
    private Transform gunTip;
    private float playerHeight;
    private float playerHeight_Offset = 4f;
    private bool isShooting;
    private float currentShootingTimer;

    void Start() {
        player = GameObject.Find("PlayerObject");
        playerPos = player.transform;
        playerHeight = player.GetComponent<PlayerController>().playerHeight;

        lineRender = GetComponent<LineRenderer>();
        gunTip = GameObject.Find("Gun Tip").transform;
        currentShootingTimer = shootTimer;
    }

    void Update(){  
    }

    void FixedUpdate() {
        seePlayer();
        lookAtPlayer();
        drawLine();
    }
    private void seePlayer() {
        isSeePlayer = !(Physics.Linecast(transform.position, playerPos.position, wallsDef));
        if (isSeePlayer) {
            if (!isShooting) {
                isShooting = true;
                InvokeRepeating("shootGun", 0, 1);
            }
        }
        else {
            isShooting = false;
            CancelInvoke("shootGun");
            currentShootingTimer = shootTimer;
        }
    }
    private void lookAtPlayer() {
        if (isSeePlayer) {
            Vector3 turretRotation = playerPos.position - transform.position;
            turretRotation.y = 0;
            transform.rotation = Quaternion.LookRotation(turretRotation, Vector3.up);
            transform.rotation = transform.rotation * Quaternion.Euler(1, rotationOffset, 1);
        }
    }
    private void drawLine() {
        if (isSeePlayer) {
            lineRender.positionCount = 2;
            lineRender.SetPosition(0, gunTip.position);
            lineRender.SetPosition(1, playerPos.position - Vector3.up * (playerHeight-playerHeight_Offset));
        }
        else {
            lineRender.positionCount = 0;
        }
    }
    private void shootGun() {
        if (currentShootingTimer > 0) {
            currentShootingTimer -= 1;
            lineRender.widthMultiplier = currentShootingTimer/shootTimer;
        }
        else {
            // Function to "kill" player will go here
            Debug.Log("You are ded");
        }
        
    }

}
