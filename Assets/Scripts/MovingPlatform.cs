using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{

    [SerializeField] private Transform _leftPatrol;
    [SerializeField] private Transform _rightPatrol;
    [SerializeField] private float _speed;

    private Rigidbody2D _rigidbody;


    private bool _isMovingRight = true;
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }


    void Update()
    {

        if (_isMovingRight == true)
        {
            _rigidbody.velocity = transform.right * _speed;
            if (transform.position.x >= _rightPatrol.position.x)
            {
                _isMovingRight = false;
            }
        }
        else
        {
            _rigidbody.velocity = transform.right * -_speed;
            if (transform.position.x <= _leftPatrol.position.x)
            {
                _isMovingRight = true;
            }
        }
    }



}
