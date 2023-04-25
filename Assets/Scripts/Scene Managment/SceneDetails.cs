using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneDetails : MonoBehaviour
{
    public bool IsLoaded { get; private set; }
    [SerializeField] List<SceneDetails> connectedScenes;
    [SerializeField] AudioClip sceneSoundtrack;
    List<SavableEntity> savableEntities;

    AudioClip currentSoundtrack;

    public static SceneDetails S { get; private set; }

    private void Awake()
    {
        S = this;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            LoadScene();
            GameController.Instance.SetCurrentScene(this);

            if(sceneSoundtrack != null)
            {
                AudioManager.A.PlayMusic(sceneSoundtrack, fade: true);
                currentSoundtrack = sceneSoundtrack;
                Debug.Log($"SD: {currentSoundtrack}");
            }

            foreach (var scene in connectedScenes)
            {
                scene.LoadScene();
            }

            //var prevScene = GameController.Instance.PrevScene;
            //if (prevScene != null)
            //{
            //    var prevLoaded = prevScene.connectedScenes;
            //    foreach (var scene in prevLoaded)
            //    {
            //        if (!connectedScenes.Contains(scene) && scene != this)
            //        {
            //            scene.UnloadScene();
            //        }
            //    }
            //    if (!connectedScenes.Contains(prevScene))
            //        prevScene.UnloadScene();
            //}
        }
    }

    public void LoadScene()
    {
        if (!IsLoaded)
        {
            SceneManager.LoadSceneAsync(gameObject.name, LoadSceneMode.Additive);
            IsLoaded = true;

            savableEntities = GetSavableEntities();
            SavingSystem.i.RestoreEntityStates(savableEntities);
        }
    }

    public void UnloadScene()
    {
        if (IsLoaded)
        {
            SavingSystem.i.CaptureEntityStates(savableEntities);

            SceneManager.UnloadSceneAsync(gameObject.name);
            IsLoaded = false;
        }
    }

    List<SavableEntity> GetSavableEntities()
    {
        var currentScene = SceneManager.GetSceneByName(gameObject.name);
        var savableEntities = FindObjectsOfType<SavableEntity>().Where(x => x.gameObject.scene == currentScene).ToList();
        return savableEntities;
    }

    public AudioClip SceneSoundtrack
    {
        get => sceneSoundtrack;
    }

    public AudioClip CurrentSoundtrack
    {
        get => currentSoundtrack;
    }
}
