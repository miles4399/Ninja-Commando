using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsTransition : MonoBehaviour
{
    [SerializeField] private string _sceneName;
    [SerializeField] private GameObject _transitionSlot;
    public Animator transition;
    public float transitionTime = 20f;

    private void Start()
    {
        _transitionSlot.SetActive(true);
        StartCoroutine(LoadLevel());
    }

    public void TransitionCoroutine()
    {
        StartCoroutine(LoadLevel());
    }

    IEnumerator LoadLevel()
    {
        //Play animation
        

        //Wait
        yield return new WaitForSeconds(transitionTime);

        transition.SetTrigger("Start");
        
        //Load scene
        SceneManager.LoadScene(_sceneName);
    }
}
