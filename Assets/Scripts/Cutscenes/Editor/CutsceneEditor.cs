using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Cutscene))]
public class CutsceneEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var cutscene = target as Cutscene;
        if(GUILayout.Button("Add dialogue"))
        {
            cutscene.AddAction(new DialogAction());
        }

        else if (GUILayout.Button("Move an actor"))
        {
            cutscene.AddAction(new MoveActors());
        }

        else if (GUILayout.Button("Start a battle"))
        {
            cutscene.AddAction(new BattleTrigger());
        }

        else if (GUILayout.Button("Add optional dialog"))
        {
            cutscene.AddAction(new ChoiceDialog());
        }

        base.OnInspectorGUI();
    }
}
