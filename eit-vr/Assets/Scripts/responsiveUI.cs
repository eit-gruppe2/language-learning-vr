using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System;
using Unity.XR.CoreUtils;
using Unity.VisualScripting;
using UnityEditor.Animations;

public class responsiveUI : MonoBehaviour
{
    public TextMeshProUGUI feedbacktext;
    public GameObject grammarStars;
    public GameObject pronounciationStars;
    public GameObject ExpressivenessStars;

    public GameObject FeedBackform;

    public GameObject ObjectiveStruct;

    public void InitObjectives(Objective objective) {
      VisualizeObjectives(objective, 0);
    } 
    public void ReceiveUpdatedObjectives(List<Objective> objectives)
    {
        // We only want to show the previous and current objective, no past objectives
        List<Objective> objectivesToShow = new List<Objective>(objectives);
        int lastToShow = objectivesToShow.FindIndex((obj) => !obj.IsCompleted);
        if(lastToShow >= 0) {
          objectivesToShow.RemoveRange(lastToShow + 1, objectivesToShow.Count - lastToShow - 1);
          VisualizeObjectives(objectivesToShow[lastToShow], lastToShow);
          Debug.Log("current objective: "+objectivesToShow[lastToShow].TaskDescription);
        } else {
          FeedBackform.SetActive(true);
        }
        
    }

    private void VisualizeObjectives(Objective objective, int index)
{
    // Instantiate an instance of a visual objective
    GameObject instance = Instantiate(ObjectiveStruct);
    instance.transform.SetParent(transform);

    // Set the rotation of the instance to match the parent's rotation
    instance.transform.rotation = transform.rotation;

    TextMeshProUGUI text = instance.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    text.text = objective.TaskDescription;

    // Move the visual objective further down
    float POSITION_MULTIPLIER = 0.2f;
    float STARTING_POSITION = 3.1f;
    instance.transform.position = new Vector3(instance.transform.position.x, STARTING_POSITION - index * POSITION_MULTIPLIER, instance.transform.position.z);
    Debug.Log("position: " + instance.transform.position);

    // Find the corresponding ObjectiveHolder
    string holderName = "ObjectiveHolder(" + index + ")";
    Transform objectiveHolder = transform.Find("ObjectiveInfo/Background/Positions/" + holderName);

    if (objectiveHolder != null)
    {
        // Set the position of the instantiated object within the ObjectiveHolder
        instance.transform.SetParent(objectiveHolder);
        instance.transform.localPosition = Vector3.zero;

        // Visualize current objective
        foreach (Transform child in transform)
        {
            if (child.name == "ObjectiveStruct(Clone)")
            {
                child.GetComponent<Image>().color = new Color32(179, 143, 47, 255);
                Debug.Log("child found and color changed");
            }
        }

        int lastChildIndex = transform.childCount - 1;
        GameObject lastChildObject = transform.GetChild(lastChildIndex).gameObject;
        lastChildObject.GetComponent<Image>().color = new Color32(255, 210, 90, 255);
    }
    else
    {
        Debug.LogWarning("ObjectiveHolder not found for index: " + index);
    }
}


    public void ReceiveFeedback(Feedback feedback)
    {
        FillStars(feedback.grammar, grammarStars);
        FillStars(feedback.pronunciation, pronounciationStars);
        FillStars(feedback.expressiveness, ExpressivenessStars);
        FillFeedBackText(feedback.generalFeedback);   
    }

    private void FillStars(int stars, GameObject starline){
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
    
    private void FillFeedBackText(string feedback){
      feedbacktext.text = feedback;
    }
}   
