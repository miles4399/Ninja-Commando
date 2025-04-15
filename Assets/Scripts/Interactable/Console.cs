using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Console : MonoBehaviour
{
    [SerializeField] private GameObject[] _toDestroy = { null, null};
    [SerializeField] private string _targetTag = "Player";
    [SerializeField] private Sprite _sprite;
    [SerializeField] private GameObject _laserDeactivateSFX;
    [SerializeField] private GameObject _consoleDestroySFX;

    private GameObject _character;
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _character = GameObject.FindWithTag(_targetTag);
    }


    public void DestroyObject()
    {
        //sound and destruction for laser instances
        foreach(var _gameObject in _toDestroy)
        {
            if (_gameObject == null) return;
            Instantiate(_consoleDestroySFX, gameObject.transform.position, gameObject.transform.rotation);
            Instantiate(_laserDeactivateSFX, _gameObject.transform.position, _gameObject.transform.rotation);
            Destroy(_gameObject);

            Animator animator = GetComponent<Animator>();
            animator.enabled = false;

            _spriteRenderer.sprite = _sprite;
        }
        
        //instantiates sound on console destruction
        
        
        //Destroy(this.gameObject);
    }
}
