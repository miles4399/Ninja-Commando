using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuriken : MonoBehaviour
{
    [SerializeField] float _speed = 20f;
    [SerializeField] private float _spinRate = 360f;
    [SerializeField] private float _homingRadius = 5f;
    [SerializeField] private LayerMask _targetLayerMask;

    private Rigidbody2D _rigidbody;
    private Vector2 _direction;
    private Collider2D[] _hitColliders = new Collider2D[10];
    private Transform _target;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.velocity = _direction * _speed;
    }

    public void SetDirecetion(Vector2 direction)
    {
        _direction = direction;
    }

    private void FixedUpdate()
    {
        _target = GetNearestTarget(GetEnemiesInRadius(_homingRadius));
        //Homing();

    }

    private void Update()
    {
        transform.Rotate(Vector3.forward * _spinRate * Time.deltaTime);
        if (_target != null)
        {
            Debug.DrawLine(transform.position, _target.position);
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        other.GetComponent<DroneAI>()?.Kill();
        other.GetComponent<EnemyAI>()?.Stagger();
        other.GetComponent<Console>()?.DestroyObject();
        Destroy(this.gameObject);
    }

    private void Homing()
    {
        if (_target == null)
        {
            return;
        }

        Vector2 dir = (_target.position - transform.position).normalized;
        //Quaternion rotation = Quaternion.LookRotation(dir);
        //transform.rotation = rotation;
        _rigidbody.velocity = dir * _speed;
    }

    private Collider2D[] GetEnemiesInRadius(float radius)
    {
        Physics2D.OverlapCircleNonAlloc(transform.position, radius, _hitColliders, _targetLayerMask);
        return _hitColliders;
    }

    private Transform GetNearestTarget(Collider2D[] targets)
    {
        Transform nearestTarget = null;
        float minDist = Mathf.Infinity;
        foreach (Collider2D target in targets)
        {
            if (target != null)
            {
                float dist = Vector3.Distance(target.transform.position, transform.position);
                if (dist < minDist)
                {
                    nearestTarget = target.transform;
                    minDist = dist;
                }
            }
        }
        return nearestTarget;
    }

}
