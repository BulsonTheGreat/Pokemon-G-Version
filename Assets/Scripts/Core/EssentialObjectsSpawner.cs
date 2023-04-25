using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssentialObjectsSpawner : MonoBehaviour
{
    [SerializeField] GameObject essentialObjectsPrefab;
    //this thing exists so that they don't duplicate once you switch baack to the previous scene
    private void Awake()
    {
        var objects = FindObjectsOfType<EssentialObjects>();
        if(objects.Length == 0)
        {
            var spawnPos = new Vector3(0, 0, 0);
            var grid = FindObjectOfType<Grid>();
            if (grid != null)
                spawnPos = grid.transform.position;
            Instantiate(essentialObjectsPrefab, spawnPos, Quaternion.identity);
        }
    }
}
