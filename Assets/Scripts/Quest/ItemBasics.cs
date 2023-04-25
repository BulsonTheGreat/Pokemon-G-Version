using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Create a new item")]
[System.Serializable]

public class ItemBasics : ScriptableObject
{
    [SerializeField] new string name;

    public string Name => name;
}

