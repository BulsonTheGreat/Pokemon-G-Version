using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour, IPlayerTriggerable
{
    //[SerializeField] int nextScene = -1; //so that it gives error in case i forget to set it
    [SerializeField] DestinationIdentifier destinationPortal;
    [SerializeField] Transform spawnPoint; //where the player should enter from once exiting the portal
    PlayerMovement player;

    public void OnPlayerTriggered(PlayerMovement player)
    {
        Debug.Log("Went through a portal");
        StartCoroutine(Teleport());
        this.player = player;
    }
    Fader fader;
    private void Start()
    {
        fader = FindObjectOfType<Fader>();
    }

    IEnumerator Teleport()
    {
        GameController.Instance.PauseGame(true);
        yield return fader.FadeIn(0.5f);
        
        //set the destination point
        var destination = FindObjectsOfType<Portal>().First(x => x != this && x.destinationPortal == this.destinationPortal);
        player.Character.SetCorrectPosition(destination.SpawnPoint.position);
        yield return fader.FadeOut(0.5f);
        GameController.Instance.PauseGame(false);
    }
    public Transform SpawnPoint => spawnPoint;
}

public enum DestinationIdentifier
{
    A, B, C, D, E, F, G, H, I
}
