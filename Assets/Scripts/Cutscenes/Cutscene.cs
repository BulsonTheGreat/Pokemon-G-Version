using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene : MonoBehaviour, IPlayerTriggerable
{
    [SerializeReference]
    [SerializeField] List<CutsceneAction> actions;

    public IEnumerator Play()
    {
        GameController.Instance.StartCutscene();
        foreach(var action in actions)
        {
            yield return action.PlayAction();
        }
        GameController.Instance.StartFreeroam();
        gameObject.SetActive(false);
    }

    public void AddAction(CutsceneAction action)
    {
        action.Name = action.GetType().ToString();
        actions.Add(action);
    }

    public void OnPlayerTriggered(PlayerMovement player)
    {
        player.Character.Animator.IsMoving = false;
        StartCoroutine(Play());
    }
}
