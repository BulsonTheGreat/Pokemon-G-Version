using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MoveActors : CutsceneAction
{
    [SerializeField] Character actor;
    [SerializeField] List<Vector2> movePatterns;

    public override IEnumerator PlayAction()
    {
        foreach(var moveVec in movePatterns)
        {
            yield return actor.Move(moveVec);
        }
    }
}
