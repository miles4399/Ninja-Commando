using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TurretProjectile : MonoBehaviour
{
    [SerializeField] protected float _speed = 10f;              //initial speed velocity
    [SerializeField] private string _targetTag = "Player";

    protected Rigidbody2D _rigidbody;
    private GameObject _character;

    //Start is called before the first frame update
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        //set initial velocity/speed in forwards direction;
        _rigidbody.velocity = _speed * transform.right;
        _character = GameObject.FindWithTag(_targetTag);
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(this.gameObject);
        PlayerDeath player = other.GetComponent<PlayerDeath>();
        if (player == null) return;
        player?.Death();
    }


}
