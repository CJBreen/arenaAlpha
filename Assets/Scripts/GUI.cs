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
    void Start()
    {
        playerSpeed = GameObject.Find("PlayerSpeed").GetComponent<TextMeshProUGUI>();
        playerRb = GameObject.Find("PlayerObject").GetComponent<Rigidbody>();
    }

    void Update()
    {
        playerSpeed.text = "Speed: "+Mathf.RoundToInt(Mathf.Abs(playerRb.velocity.magnitude)) + " m/s";
        // playerSpeed.text = "Speed: "+Mathf.RoundToInt(Mathf.Abs(playerRb.velocity.magnitude)*0.2f) + " b/s";    // Bananas Per Second
    }
}
