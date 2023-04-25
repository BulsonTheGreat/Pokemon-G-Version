using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogAction : CutsceneAction
{
    [SerializeField] Dialogs dialog;

    public override IEnumerator PlayAction()
    {
        yield return DialogueManager.Instance.ShowDialogue(dialog);
    }
}
