using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallChange : MonoBehaviour
{
    [SerializeField] private string _targetTag = "Player";
    [SerializeField] private Sprite _sprite;

    private GameObject _character;
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _character = GameObject.FindWithTag(_targetTag);
    }

    public void SpriteSwitch()
    {
        _spriteRenderer.sprite = _sprite;
        GetComponent<Collider2D>().enabled = false;
    }
}
