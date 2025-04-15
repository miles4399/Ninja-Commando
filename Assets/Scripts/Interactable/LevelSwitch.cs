using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSwitch : MonoBehaviour
{
    [SerializeField] private string _targetTag = "Player";
    [SerializeField] private string _sceneName;


    private GameObject _character;
    private CharacterMovement _player;

    public Animator transition;
    public float transitionTime = 1f;

    public PlayerDeath playerDeath;

    void Start()
    {
        _character = GameObject.FindWithTag(_targetTag);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (_player != null) return;
        _player = col.GetComponent<CharacterMovement>();

        playerDeath.Death();

        StartCoroutine(LoadLevel());
    }


    IEnumerator LoadLevel()
    {
        //Play animation
        transition.SetTrigger("Start");

        Debug.Log("coroutine started");

        //Wait
        yield return new WaitForSeconds(transitionTime);

        Debug.Log("done");
        //Load scene
        SceneManager.LoadScene(_sceneName);
    }
}
