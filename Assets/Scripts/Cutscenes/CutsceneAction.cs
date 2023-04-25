using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CutsceneAction
{
    [SerializeField] string CutsceneName;

    public virtual IEnumerator PlayAction()
    {
        yield break;
    }

    public string Name
    {
        get => CutsceneName;
        set => CutsceneName = value;
    }

}
