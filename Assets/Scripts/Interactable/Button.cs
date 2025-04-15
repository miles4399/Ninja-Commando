using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField] private GameObject[] _toDestroy = { null, null };
    [SerializeField] private string _targetTag = "Player";
    [SerializeField] private Sprite _sprite;
    [SerializeField] private GameObject _laserDeactivateSFX;
    [SerializeField] private GameObject _buttonPressedSFX;

    private GameObject _character;
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _character = GameObject.FindWithTag(_targetTag);
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        DestroyObject();
        
        _spriteRenderer.sprite = _sprite;
    }

    public void DestroyObject()
    {
        //sound and destruction for laser instances
        foreach (var _gameObject in _toDestroy)
        {
            if (_gameObject == null) return;
            Instantiate(_buttonPressedSFX, transform.position, transform.rotation);
            Instantiate(_laserDeactivateSFX, _gameObject.transform.position, _gameObject.transform.rotation);
            Destroy(_gameObject);
        }
    }
}
