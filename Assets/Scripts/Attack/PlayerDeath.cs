using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerDeath : MonoBehaviour
{
    [SerializeField] private float _dissolveTime = 0.5f;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Transform _spawnPosition;
    [SerializeField] private GameObject _death;
    [SerializeField] private IntCounter _deathCounter;
    [SerializeField] private GameObject _playerController;

    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rb;

    private Transform _currentCheckPoint;
    private bool IsVulnerable = true;




    void Start()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        _rb = GetComponent<Rigidbody2D>();
        _currentCheckPoint = _spawnPosition;

        _deathCounter.ResetCount();
    }

    public void Death()
    {
        if(IsVulnerable == true)
        {
            _playerController.SetActive(false);
            IsVulnerable = false;
            //waits 1 second before calling the death function
            Invoke(nameof(ReturnCheckPoint), 1f);
            SetMovementEnabled(false);
            StartCoroutine(LerpDissolve(0f, _dissolveTime));
            _deathCounter.IncrementCount();
            Instantiate(_death, transform.position, transform.rotation);
        }
    }

    private IEnumerator LerpDissolve(float target, float duration = 0.5f)
    {
        float start = _spriteRenderer.material.GetFloat("_Dissolve");
        float timer = 0f;
        while (timer <= duration)
        {
            timer += Time.deltaTime;
            float current = Mathf.Lerp(start, target, timer/duration);
            _spriteRenderer.material.SetFloat("_Dissolve", current);
            yield return null;
        }
    }

    private void SetMovementEnabled(bool enabled)
    {
        _rb.velocity = Vector2.zero;
        _rb.isKinematic = !enabled;
    }

    public void ReturnCheckPoint()
    {
        _playerController.SetActive(true);
        IsVulnerable = true;
        StartCoroutine(LerpDissolve(1f, _dissolveTime));
        SetMovementEnabled(true);
        _playerTransform.position = _currentCheckPoint.position;
    }

    public void ChangeSpawnPoint()
    {
        _currentCheckPoint.position = _playerTransform.position;
    }
    
    public void ToggleVulnerable()
    {
        if(IsVulnerable == true)
        {
            IsVulnerable = false;
            Debug.Log("Inulnerable On");
        }
        else
        {
            IsVulnerable = true;
            Debug.Log("Invulnerable Off");
        }

    }
}
