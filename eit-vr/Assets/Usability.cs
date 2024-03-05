using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Usability : MonoBehaviour
{

    [SerializeField]
    private GameObject buttonObject; // Assign your button object here in the inspector

    [SerializeField]
    private Material blinkMaterial; // Assign a blinking material

    [SerializeField]
    private float blinkDuration = 1f; // Duration of each blink in seconds

    [SerializeField]
    private float blinkInterval = 0.5f; // Interval between blinks

    private MeshRenderer buttonRenderer; // Renderer of the button
    private Material originalMaterial; // Original material of the button
    private bool isBlinking = false;

    void Start()
    {
        Debug.Log("Starting usability script");
        if (buttonObject == null)
        {
            Debug.LogError("VRButtonBlink: No button object assigned!");
            return;
        }

        buttonRenderer = buttonObject.GetComponent<MeshRenderer>();
        if (buttonRenderer == null)
        {
            Debug.LogError("VRButtonBlink: No MeshRenderer found on the button object!");
            return;
        }

        originalMaterial = buttonRenderer.material;

        StartBlinking();
    }

    public void StartBlinking()
    {
        if (!isBlinking)
        {
            Debug.Log("blink");
            StartCoroutine(BlinkRoutine());
        }
    }

    public void StopBlinking()
    {
        if (isBlinking)
        {
            StopCoroutine(BlinkRoutine());
            buttonRenderer.material = originalMaterial;
            isBlinking = false;
        }
    }

    private IEnumerator BlinkRoutine()
    {
        isBlinking = true;
        while (isBlinking)
        {
            buttonRenderer.material = blinkMaterial;
            yield return new WaitForSeconds(blinkDuration);
            buttonRenderer.material = originalMaterial;
            yield return new WaitForSeconds(blinkInterval);
        }
    }
}
