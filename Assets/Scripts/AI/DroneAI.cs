using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneAI : MonoBehaviour
{
    [SerializeField] private float _range;
    [SerializeField] private float _fireRate;
    [SerializeField] private float _ammoSpeed;
    [SerializeField] private Transform _player;
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private GameObject _ammoPrefab;
    [SerializeField] private Transform _leftPatrol;
    [SerializeField] private Transform _rightPatrol;
    [SerializeField] private float _speed;
    [SerializeField] private Tape _tape;
    [SerializeField] private bool _dropTape = true;
    //insert laser shot sfx
    [SerializeField] private GameObject _lasershoot;
    [SerializeField] private GameObject _droneDestroyedSFX;
    [SerializeField] private float _dissolveTime = 0.5f;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private float _verticalAimOffset = 0.5f;

    private Rigidbody2D _rigidbody;

    private float _shootTimer = 0f;
    private Vector2 _direction;
    private bool _isMovingRight = true;
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _renderer = GetComponent<SpriteRenderer>();
    }



    void Update()
    {
        _shootTimer-= Time.deltaTime;
        Vector2 playerPos = _player.position;
        playerPos = new Vector2(playerPos.x, playerPos.y + _verticalAimOffset);
        _direction = playerPos - (Vector2)transform.position;

        Collider2D[] rayInfo = Physics2D.OverlapCircleAll(_attackPoint.position, _range, _playerLayer);
        

        foreach (Collider2D player in rayInfo)
        {
            if (_shootTimer <= 0)
            {
                _shootTimer = _fireRate;
                Shoot();
            }
        }

        

        if(_isMovingRight == true)
        {
            _rigidbody.velocity = transform.right * _speed;
            if(transform.position.x >= _rightPatrol.position.x)
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

    void Shoot()
    {
        GameObject AmmoIns = Instantiate(_ammoPrefab, _attackPoint.position, Quaternion.identity);
        AmmoIns.GetComponent<Rigidbody2D>().AddForce(_direction.normalized * _ammoSpeed);
        Instantiate(_lasershoot, transform.position, transform.rotation);
    }


    public void Kill()
    {
        Instantiate(_droneDestroyedSFX, transform.position, transform.rotation);
        StartCoroutine(LerpDissolve(0f, _dissolveTime));
        if (_dropTape == true)
        {
            Instantiate(_tape, _attackPoint.position, _attackPoint.rotation);
        }
            
    }

    private IEnumerator LerpDissolve(float target, float duration = 0.5f)
    {
        float start = _renderer.material.GetFloat("_Dissolve");
        float timer = 0f;
        while (timer <= duration)
        {
            _rigidbody.velocity = Vector2.zero;
            timer += Time.deltaTime;
            float current = Mathf.Lerp(start, target, timer / duration);
            _renderer.material.SetFloat("_Dissolve", current);
            Destroy(gameObject, 0.5f);
            yield return null;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, _range);
    }


}
