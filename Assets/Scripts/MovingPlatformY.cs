using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformY : MonoBehaviour
{
    [SerializeField] private Transform _downPatrol;
    [SerializeField] private Transform _upPatrol;
    [SerializeField] private float _speed;

    private Rigidbody2D _rigidbody;


    private bool _isMovingUp = true;
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }


    void Update()
    {

        if (_isMovingUp == true)
        {
            _rigidbody.velocity = transform.up * _speed;
            if (transform.position.y >= _upPatrol.position.y)
            {
                _isMovingUp = false;
            }
        }
        else
        {
            _rigidbody.velocity = transform.up * -_speed;
            if (transform.position.y <= _downPatrol.position.y)
            {
                _isMovingUp = true;
            }
        }
    }
}
