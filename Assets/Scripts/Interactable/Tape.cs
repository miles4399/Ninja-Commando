using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Tape : MonoBehaviour
{
    [SerializeField] private string _targetTag = "Player";
    [SerializeField] private GameObject _audioBP;


    public static event Action<int> OnAddScoreEvent;

    private GameObject _character;
    private CharacterMovement _player;

    private void Start()
    {
        _character = GameObject.FindWithTag(_targetTag);
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        if (_player != null) return;
        _player = col.GetComponent<CharacterMovement>();
        Instantiate(_audioBP, transform.position, transform.rotation);
        Gate.tapeCount++;
        OnAddScoreEvent?.Invoke(Gate.tapeCount);

        Destroy(this.gameObject);
    }
}
