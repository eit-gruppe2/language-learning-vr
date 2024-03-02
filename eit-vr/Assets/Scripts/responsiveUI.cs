using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System;

public class responsiveUI : MonoBehaviour
{
    public int stars;
    public string feedback;
    public TextMeshProUGUI feedbacktext;
    public GameObject grammarStars;
    public GameObject pronounciationStars;
    public GameObject ExpressivenessStars;

    public TextMeshProUGUI currentObjective;

    public void InitObjectives(Objective objective) {
      currentObjective.text = objective.TaskDescription;
    } 
    public void ReceiveUpdatedObjectives(List<Objective> objectives)
    {
        // We only want to show the previous and current objective, no past objectives
        List<Objective> objectivesToShow = new List<Objective>(objectives);
        int lastToShow = objectivesToShow.FindIndex((obj) => !obj.IsCompleted);
        objectivesToShow.RemoveRange(lastToShow + 1, objectivesToShow.Count - lastToShow - 1);
        
        // TODO: show objectivesToShow to the user
        currentObjective.text = objectivesToShow[lastToShow].TaskDescription;
        Debug.Log("current objective: "+objectivesToShow[lastToShow].TaskDescription);
    }


    public void ReceiveFeedback(Feedback feedback)
    {
        // TODO: show the feedback to the user
        fillStars(feedback.grammar, grammarStars);
        fillStars(feedback.pronunciation, pronounciationStars);
        fillStars(feedback.expressiveness, ExpressivenessStars);
        fillFeedBackText(feedback.generalFeedback);   
    }

    private void fillStars(int stars, GameObject starline){
      int i = 0;
      foreach (Transform star in starline.transform) {
        if (i<stars) {
          star.gameObject.GetComponent<Image>().color = new Color32(255, 210, 90, 255);
          i++;
        } else {
          star.gameObject.GetComponent<Image>().color = new Color32(179, 143, 47, 255);
        }
      }
    }
    
    private void fillFeedBackText(string feedback){
      feedbacktext.text = feedback;
    }
}   
