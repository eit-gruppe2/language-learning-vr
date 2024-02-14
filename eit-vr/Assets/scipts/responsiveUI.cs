using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class responsiveUI : MonoBehaviour
{
    public int stars;
    public string feedback;
    public TextMeshProUGUI feedbacktext;
    public GameObject StarLine;

    private void fillStars(int stars){
      int i = 0;
      foreach (Transform star in StarLine.transform) {
        if (i<stars) {
          star.gameObject.GetComponent<Image>().color = new Color32(255, 210, 90, 255);
          Debug.Log("Star changed color");
          i++;
        } else {
          star.gameObject.GetComponent<Image>().color = new Color32(179, 143, 47, 255);
        }
      }
    }
    
    private void fillFeedBackText(string feedback){
      feedbacktext.text = feedback;
    }

    private void Update(){
      if(Input.GetKeyDown("space")){
        fillStars(4);
        fillFeedBackText(feedback);
      }
      
    }
}   
