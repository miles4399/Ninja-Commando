using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DamagePlayer : MonoBehaviour
{
    [SerializeField] private string _targetTag = "Player";


    private GameObject _character;




    private void Start()
    {
        _character = GameObject.FindWithTag(_targetTag);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        PlayerDeath player = other.GetComponent<PlayerDeath>();
        if (player == null) return;
        player?.Death();
    }
}
