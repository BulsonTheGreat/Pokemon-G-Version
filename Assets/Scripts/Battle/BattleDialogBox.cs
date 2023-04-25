using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BattleDialogBox : MonoBehaviour
{
    [SerializeField] Text dialogText;
    [SerializeField] int lettersPerSecond = 30;
    [SerializeField] GameObject actionSelector;
    [SerializeField] GameObject moveSelector;
    [SerializeField] GameObject moveDetails;
    [SerializeField] Text catText;
    [SerializeField] Text typeText;
    [SerializeField] Image typeImage;
    [SerializeField] Text powerText;
    [SerializeField] Text accuracyText;
    [SerializeField] Text descriptionText;
    [SerializeField] List<Text> actionTexts;
    [SerializeField] List<Text> moveTexts;

    Color highlightedColor;

    private void Start()
    {
        highlightedColor = GlobalSettings.I.HighlightedColor;
    }

    public void SetDialog(string dialog)
    {
        dialogText.text = dialog;
    }
    public IEnumerator TypeDialog(string dialog)
    {
        dialogText.text = "";
        foreach(var letter in dialog.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }
        yield return new WaitForSeconds(1f);
    }
    public void EnableDialogText(bool enabled)
    {
        dialogText.enabled = enabled;
    }
    public void EnableActionSelector(bool enabled)
    {
        actionSelector.SetActive(enabled);
    }
    public void EnableMoveSelector(bool enabled)
    {
        moveSelector.SetActive(enabled);
        moveDetails.SetActive(enabled);
    }
    public void UpdateActionSelection(int selectedAction)
    {
        for(int i = 0; i < actionTexts.Count; i++)
        {
            if(i == selectedAction)
            {
                actionTexts[i].color = highlightedColor;
            }
            else
            {
                actionTexts[i].color = Color.black;
            }
        }
    }
    public void UpdateMoveSelection(int selectedMove, Move move)
    {
        for (int i = 0; i < moveTexts.Count; i++)
        {
            if (i == selectedMove)
            {
                moveTexts[i].color = highlightedColor;
            }
            else
            {
                moveTexts[i].color = Color.black;
            }
        }
        catText.text = $"Cat: {move.Base.MoveCategorie}";
        typeText.text = "Type:";
        typeImage.sprite = GlobalSettings.I.TypeImages[move.Base.Type];
        if(move.Base.MoveCategorie == MoveCategories.Status)
        {
            powerText.text = $"Pow: -";
            accuracyText.text = $"Acc: {move.Base.Accuracy}";
        }
        else
        {
            powerText.text = $"Pow: {move.Base.Power}";
            accuracyText.text = $"Acc: {move.Base.Accuracy}";
        }
        descriptionText.text = move.Base.Description;
    }
    public void SetMoveNames(List<Move>moves)
    {
        for(int i = 0; i < moveTexts.Count; i++)
        {
            if(i < moves.Count)
            {
                moveTexts[i].text = moves[i].Base.Name;
            }
            else
            {
                moveTexts[i].text = "-";
            }
        }
    }
}
