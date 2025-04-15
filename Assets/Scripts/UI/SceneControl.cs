using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControl : MonoBehaviour
{
    [SerializeField] private string _sceneName;
    [SerializeField] private GameObject _transitionSlot;
    public Animator transition;
    public float transitionTime = 1f;
    [SerializeField] private GameObject _titleSFX;

    //REFERENCE SOUND EFFECT AS A GAMEOBJECT

    private void Start()
    {
        _transitionSlot.SetActive(true);
    }

    public void NinjaCommandoSound()
    {
        Instantiate(_titleSFX, transform.position, transform.rotation);
    }

    public void TransitionCoroutine()
    {
        StartCoroutine(LoadLevel()); 
    }

    IEnumerator LoadLevel()
    {
        //Play animation
        transition.SetTrigger("Start");

        //Wait
        yield return new WaitForSeconds(transitionTime);

        //Load scene
        SceneManager.LoadScene(_sceneName);
    }

    //Load a scene (type the name of the scene in the inspector)
    /*
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    */
}