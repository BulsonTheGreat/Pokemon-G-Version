using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainerFov : MonoBehaviour, IPlayerTriggerable
{
    public void OnPlayerTriggered(PlayerMovement player)
    {
        GameController.Instance.OnEnterTrainersView(GetComponentInParent<TrainerController>());
        Debug.Log("Trainer can see you");
    }
}
