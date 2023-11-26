using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GUI : MonoBehaviour
{
    private TextMeshProUGUI playerSpeed;
    private Rigidbody playerRb;
    private Canvas escMenu;
    private Canvas hudMenu;

    public bool isEsc;
    private bool allowGuiActivate = true;

    void Start()
    {
        playerSpeed = GameObject.Find("PlayerSpeed").GetComponent<TextMeshProUGUI>();
        escMenu = GameObject.Find("EscMenu").GetComponent<Canvas>();
        hudMenu = GameObject.Find("HUD").GetComponent<Canvas>();
        playerRb = GameObject.Find("PlayerObject").GetComponent<Rigidbody>();
        escMenu.enabled = false;
    }

    void Update()
    {
        playerSpeed.text = "Speed: "+Mathf.RoundToInt(Mathf.Abs(playerRb.velocity.magnitude)) + " m/s";
        // playerSpeed.text = "Speed: "+Mathf.RoundToInt(Mathf.Abs(playerRb.velocity.magnitude)*0.2f) + " b/s";    // Bananas Per Second
    }
    void FixedUpdate() {
        if (Input.GetKey(KeyCode.Escape) && allowGuiActivate) {
            if (!isEsc){
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                hudMenu.enabled = false;
                escMenu.enabled = true;
                isEsc = true;
            }
            else {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                hudMenu.enabled = true;
                escMenu.enabled = false;
                isEsc = false;
            }
            allowGuiActivate = false;
            Invoke("guiActivateCooldown", 1);
        }
    }
    private void guiActivateCooldown() {
        allowGuiActivate = true;
    }
}
