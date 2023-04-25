using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    public static RespawnPoint Instance { get; private set; }
    Fader fader;

    private void Awake()
    {
        Instance = this;
        fader = FindObjectOfType<Fader>();
    }

    public IEnumerator RestartPosition(PlayerMovement player)
    {
        yield return fader.FadeIn(2f);
        player.transform.position = gameObject.transform.position;
        yield return fader.FadeOut(1f);
    }

}
