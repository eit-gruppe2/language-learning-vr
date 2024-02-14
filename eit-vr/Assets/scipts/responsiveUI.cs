using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class responsiveUI : MonoBehaviour
{
    private void LateUpdate() {       
        Vector3 targetPosition = new Vector3(Camera.main.transform.position.x, transform.position.y, Camera.main.transform.position.z);
        transform.LookAt(targetPosition, Camera.main.transform.up);
    }
}   
