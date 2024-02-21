using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class speakable : MonoBehaviour
{
    float transitionSpeed = 4.0f;
    public GameObject button;
    public GameObject indicator;
    private void Update()
    {
        Vector3 distanceToPlayer = transform.position - Camera.main.transform.position;
        int interactionDistance = 4;
        Vector3 normalScale = new Vector3(1, 1, 1);
        Vector3 smallScale = new Vector3(0, 0, 0);

        if (distanceToPlayer.magnitude <= interactionDistance && button.transform.localScale != normalScale)
        {
            // Smoothly transition to normal scale
            button.transform.localScale = Vector3.Lerp(button.transform.localScale, normalScale, Time.deltaTime * transitionSpeed);
            indicator.SetActive(false);
        }
        else if (distanceToPlayer.magnitude > interactionDistance && button.transform.localScale != smallScale)
        {
            // Smoothly transition to small scale
            button.transform.localScale = Vector3.Lerp(button.transform.localScale, smallScale, Time.deltaTime * transitionSpeed);
            indicator.SetActive(true);
        }
    }

    private void LateUpdate() {  
        Vector3 targetPosition = new Vector3(Camera.main.transform.position.x, transform.position.y, Camera.main.transform.position.z);
        transform.LookAt(targetPosition, Camera.main.transform.up);
    }

}
