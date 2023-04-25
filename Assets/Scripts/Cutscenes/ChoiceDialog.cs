using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceDialog : CutsceneAction
{
    [SerializeField] Dialogs mainDialog;
    [SerializeField] string choice1;
    [SerializeField] string choice2;
    [SerializeField] Dialogs dialog1;
    [SerializeField] Dialogs dialog2;

    public override IEnumerator PlayAction()
    {
        int selectedChoice = 0;
        yield return DialogueManager.Instance.ShowDialogue(mainDialog,
            new List<string>() { choice1, choice2 },
            (choiceIndex) => selectedChoice = choiceIndex);

        if (selectedChoice == 0)
        {
            yield return DialogueManager.Instance.ShowDialogue(dialog1);
        }
        else if (selectedChoice == 1)
        {
            yield return DialogueManager.Instance.ShowDialogue(dialog2);
        }
    }

}
