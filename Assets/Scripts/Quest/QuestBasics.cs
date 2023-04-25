using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Create a new quest")]
public class QuestBasics : ScriptableObject
{
    [SerializeField] new string name;
    [SerializeField] string description;

    [SerializeField] Dialogs startDialog;
    [SerializeField] Dialogs inProgressDialog;
    [SerializeField] Dialogs endDialog;

    [SerializeField] ItemSlot requiredItem;
    [SerializeField] ItemSlot rewardItem;

    public string Name => name;
    public string Description => description;

    public Dialogs StartDialog => startDialog;
    public Dialogs InProgressDialog => inProgressDialog?.Lines?.Count > 0? inProgressDialog : startDialog;
    public Dialogs EndDialog => endDialog;

    public ItemSlot RequiredItem => requiredItem;
    public ItemSlot RewardItem => rewardItem;
}
