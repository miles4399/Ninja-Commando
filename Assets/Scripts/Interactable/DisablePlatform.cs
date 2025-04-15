using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisablePlatform : MonoBehaviour
{
    private DisappearingPlatform _disappearingPlatform;
    private bool _isDisabling;
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _disappearingPlatform = GetComponentInParent<DisappearingPlatform>();
    }


    private void OnEnable()
    {
        _isDisabling = false;
    }

    private void OnTriggerEnter2D (Collider2D col)
    {

        if (_isDisabling == true) return;
        CharacterMovement player = col.GetComponent<CharacterMovement>();
        if (player == null) return;
        _animator.SetTrigger("Disappear");
        float clipTime = _animator.GetCurrentAnimatorClipInfo(0).Length;
        _disappearingPlatform.StartTimer(clipTime);
        _isDisabling = true;


    }
}
