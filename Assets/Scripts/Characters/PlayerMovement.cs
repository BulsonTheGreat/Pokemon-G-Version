using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour, ISavable
{
    private Vector2 input;
    private Character character;
    [SerializeField] string playerName;
    [SerializeField] Sprite sprite;
    
    void Awake()
    {
        character = GetComponent<Character>();
    }

    public void HandleUpdate()
    {
        if(!character.IsMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            if (input.x != 0)
            {
                input.y = 0;
            }

            if(input != Vector2.zero)
            {
                StartCoroutine(character.Move(input, OnMoveOver));
            }
        }

        character.HandleUpdate();

        if (Input.GetKeyDown(KeyCode.Z))
        {
            StartCoroutine(Interact());
        }
    }

    IEnumerator Interact()
    {
        var facingDir = new Vector3(character.Animator.MoveX, character.Animator.MoveY);
        var interactPos = transform.position + facingDir;

        var collider = Physics2D.OverlapCircle(interactPos, 0.2f, GameLayers.I.InteractibleLayer);
        if(collider != null)
        {
            yield return collider.GetComponent<Interactable>()?.Interact(transform);
        }
    }
    private void OnMoveOver()
    {
        var colliders = Physics2D.OverlapCircleAll(transform.position - new Vector3(0, character.OffsetY), 0.2f, GameLayers.I.TriggerableLayers);

        
        foreach(var collider in colliders)
        {
            var triggerable = collider.GetComponent<IPlayerTriggerable>();
            if(triggerable != null)
            {
                character.Animator.IsMoving = false;
                triggerable.OnPlayerTriggered(this);
                break;
            }
        }
    }

    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.DrawSphere(transform.position - new Vector3(0, character.OffsetY), 0.2f);
    //}
    //object can represend any other type
    public object CaptureState()
    {
        var saveData = new PlayerSaveData
        {
            position = new float[] { transform.position.x, transform.position.y },
            party = GetComponent<PokemonParty>().Pokemons.Select(p => p.GetSaveData()).ToList(),
        };

        return saveData;
    }

    public void RestoreState(object state)
    {
        var saveData = (PlayerSaveData)state;
        //restore position
        var position = saveData.position;
        transform.position = new Vector3(position[0], position[1]);
        //restore pokemon party
        GetComponent<PokemonParty>().Pokemons = saveData.party.Select(s => new Pokemon(s)).ToList();
    }

    public string Name
    {
        get => playerName;
    }
    public Sprite Sprite
    {
        get => sprite;
    }
    public Character Character => character;
}
//class has to be serializable in order to be saved
[System.Serializable]
public class PlayerSaveData
{
    public float[] position;
    public List<PokemonSaveData> party;
}
