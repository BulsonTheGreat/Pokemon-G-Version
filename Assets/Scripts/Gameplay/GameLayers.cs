using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLayers : MonoBehaviour
{
    [SerializeField] LayerMask solidObjectsLayer;
    [SerializeField] LayerMask interactibleLayer;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] LayerMask fovLayer;
    [SerializeField] LayerMask portalLayer;
    [SerializeField] LayerMask blockadeLayer;
    [SerializeField] LayerMask cutsceneLayer;

    public static GameLayers I { get; set; }

    public void Awake()
    {
        I = this;
    }

    public LayerMask SolidLayer
    {
        get => solidObjectsLayer;
    }
    public LayerMask InteractibleLayer
    {
        get => interactibleLayer;
    }
    public LayerMask PlayerLayer
    {
        get => playerLayer;
    }
    public LayerMask FovLayer
    {
        get => fovLayer;
    }
    public LayerMask PortalLayer
    {
        get => portalLayer;
    }
    public LayerMask BlockadeLayer
    {
        get => blockadeLayer;
    }
    public LayerMask CutsceneLayer
    {
        get => cutsceneLayer;
    }
    public LayerMask TriggerableLayers
    {
        get => interactibleLayer | fovLayer | portalLayer | blockadeLayer | cutsceneLayer; 
    }
}
