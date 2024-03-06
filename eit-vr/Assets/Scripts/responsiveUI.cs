using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System;
using Unity.XR.CoreUtils;
using Unity.VisualScripting;
using UnityEditor.Animations;
// We would like to thank the creators of these icons
// <a href="https://www.flaticon.com/free-icons/vr-gaming" title="vr gaming icons">Vr gaming icons created by vectorsmarket15 - Flaticon</a>
// <a href="https://www.flaticon.com/free-icons/listen" title="listen icons">Listen icons created by Voysla - Flaticon</a>

public class responsiveUI : MonoBehaviour
{
    public TextMeshProUGUI feedbacktext;
    public GameObject grammarStars;
    public GameObject pronounciationStars;
    public GameObject ExpressivenessStars;

    public GameObject FeedBackform;

    public GameObject ObjectiveStruct;

    public GameObject EarIcon;
    public GameObject PressAIcon;
    private bool recording;

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

    private void VisualizeObjectives(Objective objective, int index) {
      // Instantiate an instance of a visual objective
      GameObject instance = Instantiate(ObjectiveStruct);
      instance.transform.SetParent(transform);
      TextMeshProUGUI text = instance.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
      text.text = objective.TaskDescription;
      instance.GetComponent<Image>().color = new Color32(179, 143, 47, 255);

      // Position the instantiated object
      string holderName = "ObjectiveHolder (" + index + ")";
      Transform objectiveHolder = transform.Find("ObjectiveInfo/Background/Positions/" + holderName);
      if (objectiveHolder != null)
      {
          // Set the position of the instantiated object within the ObjectiveHolder
          instance.transform.SetParent(objectiveHolder);
          instance.transform.localPosition = Vector3.zero;
          instance.transform.rotation = transform.rotation;
      } else {
        Debug.LogWarning("ObjectiveHolder index not found");
      }

      UpdateObjectiveColors(index);
    }

    private void UpdateObjectiveColors(int index)
    {
      foreach (Transform holder in transform.Find("ObjectiveInfo/Background/Positions"))
          {
              if (holder.childCount > 0)
              {
                  holder.GetChild(0).GetComponent<Image>().color = new Color32(179, 143, 47, 255);
              }
          }

      for (int i = 6; i >= 0; i--)
      {
          string holderNameToCheck = "ObjectiveHolder (" + i + ")";
          Transform holder = transform.Find("ObjectiveInfo/Background/Positions/" + holderNameToCheck);

          if (holder != null && holder.childCount > 0)
          {
              // Change the color of the object found in the last six holders
              holder.GetChild(0).GetComponent<Image>().color = new Color32(255, 210, 90, 255);
              break;  // Exit the loop after changing the color of the first object found
          }
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

    public void UpdateRecordIcons () {
      recording = !recording;
      
    }

    IEnumerator OscillateEarIcon()
    {
        float minScale = 0.0072f;
        float maxScale = 0.0075f;

        while (true)
        {
            // Mathf.PingPong returns a value between 0 and 1, so we scale and offset it to fit the desired range
            float scale = Mathf.PingPong(Time.time, (maxScale - minScale) / 2.0f) + minScale;

            EarIcon.transform.localScale = new Vector3(scale, scale, scale);
            yield return null;
        }
    }


    // private void LateUpdate()
    // {
    //     // Make canvas face the player at all times
    //     Vector3 targetDirection = Camera.main.transform.position - transform.position;
    //     targetDirection = Quaternion.Euler(0, 180, 0) * targetDirection;
    //     targetDirection.y = 0;
    //     Quaternion targetRotation = Quaternion.LookRotation(targetDirection.normalized, Vector3.up);
    //     transform.rotation = targetRotation;
    //     transform.position = new Vector3(transform.position.x, Camera.main.transform.position.y, transform.position.z);
    // }

    private void Update() {

      if(recording || Input.GetKey(KeyCode.Space)) {
        EarIcon.SetActive(true);
        PressAIcon.SetActive(false);
        StartCoroutine(OscillateEarIcon());
      } else {
        PressAIcon.SetActive(true);
        EarIcon.SetActive(true);
        StopAllCoroutines();
      }
    }
}   
