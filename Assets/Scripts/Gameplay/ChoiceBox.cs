using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceBox : MonoBehaviour
{
    [SerializeField] ChoiceText textPrefab;
    bool ChoiceSelected = false;
    List<ChoiceText> choiceTexts;
    int currentChoice;

    public IEnumerator ShowChoices(List<string> choices, Action<int> OnChoiceSelected)
    {
        ChoiceSelected = false;
        currentChoice = 0;
        gameObject.SetActive(true);
        //delete existing ones
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        //create available choices
        choiceTexts = new List<ChoiceText>();
        foreach(var choice in choices)
        {
            var choiceTextObj = Instantiate(textPrefab, transform);
            choiceTextObj.Textfield.text = choice.ToString();
            choiceTexts.Add(choiceTextObj);
        }

        yield return new WaitUntil(() => ChoiceSelected == true);

        OnChoiceSelected?.Invoke(currentChoice);
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.DownArrow))
            currentChoice++;
        
        if(Input.GetKeyDown(KeyCode.UpArrow))
            currentChoice--;

        currentChoice = Mathf.Clamp(currentChoice, 0, choiceTexts.Count - 1);

        for(int i = 0; i < choiceTexts.Count; i++)
        {
            if(i == currentChoice)
                choiceTexts[i].SetSelected(true);
            else
                choiceTexts[i].SetSelected(false);
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            ChoiceSelected = true;
        }
    }
}
