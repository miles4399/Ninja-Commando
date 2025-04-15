using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

#pragma warning disable 649

public class PlayerController : MonoBehaviour
{
    [Header("Character")]
    [SerializeField] private GameObject _target;
    [SerializeField] private bool _autoPossess = true;
                                                                         
    private CharacterMovement _characterMovement;
    private PlayerAttack _playerAttack;

    private Vector2 _moveInput;
    private bool _possessed = false;

    private void OnEnable()
    {
        _moveInput = Vector2.zero;
    }

    private void Start()
    {
        if (_autoPossess && _target != null) Possess(_target);
    }

    public void Possess(GameObject target)
    {
        if (_target.TryGetComponent(out CharacterMovement characterMovement))
        {
            _characterMovement = characterMovement;
            _possessed = true;
        }
        else
        {
            Debug.LogWarning($"{gameObject.name} failed to possess {_target.name}", _target);
            _characterMovement = null;
            _possessed = false;
        }
        if (_target.TryGetComponent(out PlayerAttack playerAttack))
        {
            _playerAttack = playerAttack;
            _possessed = true;
        }
        else
        {
            Debug.LogWarning($"{gameObject.name} failed to possess {_target.name}", _target);
            _playerAttack = null;
            _possessed = false;
        }
    }

    public void Depossess()
    {
        _characterMovement = null;
        _possessed = false;
    }

    public void OnMove(InputValue value)        //Call when pree WASD
    {
        _moveInput = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)        //Call when press space
    {
        _characterMovement?.Jump();         
    }


    public void OnAttack(InputValue value)
    {
        _playerAttack?.Attack();
    }

    public void OnThrow(InputValue value)
    {
        _playerAttack?.Throw();
    }



    private void Update()
    {
        if (_possessed)
        {
            //Vector3 moveInput = Vector3.right * _moveInput.x;
            _characterMovement.SetMoveInput(_moveInput);
        }
    }
}
