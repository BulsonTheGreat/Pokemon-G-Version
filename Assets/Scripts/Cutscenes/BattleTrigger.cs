using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTrigger : CutsceneAction
{
    [SerializeField] TrainerController trainer;
    [SerializeField] Dialogs dialog;

    public override IEnumerator PlayAction()
    {
        yield return DialogueManager.Instance.ShowDialogue(dialog);
        GameController.Instance.OnEnterTrainersView(trainer);
    }
}
