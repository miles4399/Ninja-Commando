using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _chaseSpeed;
    [SerializeField] private float _agroRange;
    [SerializeField] private float _fleeDistance;
    [SerializeField] private float _attackPlayerDistance;
    [SerializeField] private float _attackCoolDown;
    [SerializeField] private float _staggerTime = 0.5f;
    [SerializeField] private float _patrolPointWaitTime = 2f;
    [SerializeField] private float _minPatrolDistance = 0.25f;
    [SerializeField] private Transform _player;
    [SerializeField] private Tape _tape;
    [SerializeField] private Transform soilderPos;
    [SerializeField] private Transform _leftPatrol;
    [SerializeField] private Transform _rightPatrol;
    [SerializeField] private Transform _attackPos;
    [SerializeField] private bool _dropTape = true;
    [SerializeField] private GameObject _halt;
    [SerializeField] private GameObject _hit;
    [SerializeField] private string _playerTag = "Player";
    [SerializeField] private State _state = State.Patrol;
    [SerializeField] private float _maxYDis = 2f;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private float _dissolveTime = 0.5f;
    [SerializeField] private float _distanceCheckOffset = -0.9f;
    [SerializeField] private AnimationClip _hitAnimation;
    [SerializeField] private AudioSource _audio;

    private bool _hasSpotPlayer = false;
    private bool _isMovingRight = true;
    Rigidbody2D _rigidBody;
    private Coroutine _currentState;
    private GameObject _target;
    private bool _isGrounded;
    [SerializeField]private float _horizontalOffset = 0.75f;
    [SerializeField]private float _verticalOffsetModifier = -2.5f;
    [SerializeField]private LayerMask _soliderGroundMask;
    [SerializeField]private float _groundCheckDistance = 0.75f;
    [SerializeField] private GameObject _shurikenTinkSFX;
    [SerializeField] private GameObject _laughSFX;
    [SerializeField] private float _laughDelay = 0.5f;

    private float _defaultFleeDistance;
    private float _tempFleeDistance = 999;

    private Animator _animator;
    private CharacterMovement _characterMovement;
    private float _currentTime;
    private GameObject _haltSoundInstance;
    
    public bool IsDead = false;


    private enum State
    {
        Patrol,
        Chase,
        Attack,
        Stagger,
        Death
    }

    private void Start()
    {
        _defaultFleeDistance = _fleeDistance;
        _rigidBody = GetComponent<Rigidbody2D>();
        _target = GameObject.FindWithTag(_playerTag);
        _animator = GetComponent<Animator>();
        NextState(PatrolState());
        
    }

    private void Update()
    {
        //_renderer.flipX = _isMovingRight;

        // Flips the sprite renderer
        if (_isMovingRight == true)
        {
            _renderer.flipX = true;
        }
        else if (_isMovingRight == false)
        {
            _renderer.flipX = false;
        }

        _animator.SetFloat("Speed", Mathf.Abs(_rigidBody.velocity.x), 0.1f, Time.deltaTime);
    }

    private void NextState(IEnumerator nextState)
    {
        // stop existing state
        if (_currentState != null) StopCoroutine(_currentState);
        //start next state
        _currentState = StartCoroutine(nextState);
    }

    private IEnumerator PatrolState()
    {
        _state = State.Patrol;
        _rigidBody.velocity = new Vector2(0f, 0f);

        Vector2 targetPos = _isMovingRight ? new Vector2(_rightPatrol.position.x, transform.position.y) : new Vector2(_leftPatrol.position.x, transform.position.y);
        
        float speed = _isMovingRight ? _speed : -_speed;

        while (Vector2.Distance(targetPos, transform.position) > _minPatrolDistance)
        {
            MoveTo(targetPos);

            yield return null;
            float distToPlayer = Vector2.Distance(transform.position, _player.position);

            if (distToPlayer < _agroRange && _isGrounded && _player.position.y + 2.2f >= transform.position.y)
            {
                NextState(ChaseState());
            }



            yield return null;
            
        }

        _rigidBody.velocity = new Vector2(0f, 0f);
        float waitTime = 0f;
        while(waitTime < _patrolPointWaitTime)
        {
            waitTime += Time.deltaTime;
            float distToPlayer = Vector2.Distance(transform.position, _player.position);
            if (distToPlayer < _agroRange /*&& PlayerYDis() < _maxYDis*/)
            {
                
                NextState(ChaseState());
            }
            yield return null;
        }
        //yield return new WaitForSeconds(_patrolPointWaitTime);
        _isMovingRight = !_isMovingRight;
        NextState(PatrolState());

    }

    private void MoveTo(Vector2 position, float speedMultiplier = 1f)
    {
        float diff = position.x - transform.position.x;
        float distance = Mathf.Abs(diff);
        float velocity = Mathf.Clamp01(distance) * _speed * Mathf.Sign(diff) * speedMultiplier;
        _rigidBody.velocity = new Vector2(velocity, 0f);
    }

    private IEnumerator ChaseState()
    {
        _animator.SetBool("isStaggered", false);
        _state = State.Chase;
        _rigidBody.velocity = new Vector2(0f, 0f);

        if (_hasSpotPlayer == false)
        {   
            _haltSoundInstance = Instantiate(_halt, transform.position, transform.rotation);
            _hasSpotPlayer = true;
        }

        while (Vector2.Distance(_player.position, transform.position) > _attackPlayerDistance)
        {
            if(_isGrounded == true)
            {
                if (transform.position.x < _player.position.x)
                {
                    //enemy is left to the player
                    _isMovingRight = true;
                }
                else if (transform.position.x > _player.position.x)
                {
                    //enemy is right to the player
                    _isMovingRight = false;
                }

                MoveTo(_player.position, 2f);
                
            }
            else
            {
                _fleeDistance = _defaultFleeDistance;
                _rigidBody.velocity = new Vector2(0f, 0f);
                PrepareForPatrol();
                NextState(PatrolState());
            }

            if (Vector2.Distance(_player.position, transform.position) >= _fleeDistance /*|| PlayerYDis() > _maxYDis*/)
            {
                PrepareForPatrol();
                NextState(PatrolState());
            }
            yield return null;

        }

        yield return null;
        _fleeDistance = _defaultFleeDistance;
        NextState(AttackState());
           
    }

    private void PrepareForPatrol()
    {
        float leftDistance = Vector2.Distance(transform.position, new Vector2(_leftPatrol.position.x, transform.position.y));
        float rightDistance = Vector2.Distance(transform.position, new Vector2(_rightPatrol.position.x, transform.position.y));
        _isMovingRight = rightDistance > leftDistance ? true : false;
    }

    private IEnumerator AttackState()
    {
        _animator.SetBool("isStaggered", false);
        _state = State.Attack;
        _rigidBody.velocity = new Vector2(0f, 0f);

        while (Vector2.Distance(_player.position, transform.position) <= _attackPlayerDistance)
        {
            yield return new WaitForSeconds(_attackCoolDown);
            //if(PlayerYDis() > _maxYDis)
            //{
            //    Debug.Log("step 2");
            //    yield return null;
            //    NextState(PatrolState());
            //}
            if(Vector2.Distance(_player.position, GetEnemyModifiedPos()) > _attackPlayerDistance)
            {
                yield return null;
                _animator.SetBool("IsAttacking", false);
                NextState(ChaseState());
            }
            else
            {
                _animator.SetBool("IsAttacking", true);
                Debug.Log("hit");
                PlayerDeath player = _player.GetComponent<PlayerDeath>();
                player.Death();
            }
        }
        yield return null;

        _animator.SetBool("IsAttacking", false);
        NextState(ChaseState());
    }

    private IEnumerator StaggerState()
    {
            _animator.SetBool("isStaggered", true);
        

            _state = State.Stagger;


            
            while(_currentTime < _staggerTime)
            {
                _currentTime += Time.deltaTime;
                _rigidBody.velocity = Vector2.zero;


                yield return null;
            }
        _currentTime = 0f;
        
       

        //NextState(PatrolState());

        if(Vector2.Distance(_player.position, transform.position) > _attackPlayerDistance) 
        {
            _fleeDistance = _tempFleeDistance;
            NextState(ChaseState());
        }
        NextState(AttackState());
    }

    private void FixedUpdate()
    {
        _isGrounded = CheckGround(_horizontalOffset) && CheckGround(-_horizontalOffset);
    }

    private bool CheckGround(float groundCheckOffset)
    {
        Vector2 offset = new Vector2(groundCheckOffset, _groundCheckDistance * _verticalOffsetModifier);
        Vector2 origin = offset + (Vector2)transform.position;


        if (Physics2D.Raycast(origin, Vector2.down, _groundCheckDistance, _soliderGroundMask))
        {
            Debug.DrawRay(origin, Vector2.down * _groundCheckDistance, Color.green);
            return true;
        }
        else
        {
            Debug.DrawRay(origin, Vector2.down * _groundCheckDistance, Color.red);
            return false;
        }
    }

    public void PlayerHitSound()
    {
        if(_audio.isPlaying == false)
        {
            _audio.Play();
        }
    }

    private void ResetIsAttacking()
    {
        _animator.SetBool("IsAttacking", false);
    }




    public void Kill()
    {
        if(IsDead == false)
        {
            IsDead = true;
            StopAllCoroutines();
            if (_dropTape == true)
            {
                _dropTape = false;
                Instantiate(_tape, soilderPos.position, soilderPos.rotation);
            }
            StartCoroutine(LerpDissolve(0f, _dissolveTime));
                
            }
       
    }

    public void Stagger()
    {
        Instantiate(_shurikenTinkSFX, transform.position, transform.rotation);
        NextState(StaggerState());
    }

    /*private void LaughSound()
    {
        Instantiate(_laughSFX, transform.position, transform.rotation);
    }
    */

    private IEnumerator LerpDissolve(float target, float duration = 0.5f)
    {
        _state = State.Death;

        _rigidBody.velocity = Vector2.zero;
        float start = _renderer.material.GetFloat("_Dissolve");
        float timer = 0f;
        while (timer <= duration)
        {
            timer += Time.deltaTime;
            float current = Mathf.Lerp(start, target, timer / duration);
            _renderer.material.SetFloat("_Dissolve", current);
            Destroy(_haltSoundInstance);
            Destroy(gameObject, 0.5f);

            yield return null;
        }
    }



    private float PlayerYDis()
    {
        Vector3 playY = new Vector3(0f, _player.position.y, 0f) ;
        Vector3 enemyY = new Vector3(0f, GetEnemyModifiedPos().y, 0f);
        float dis = Vector3.Distance(playY, enemyY);
        Debug.Log($"distance between AI and player{dis}");
        return dis;

    }

    private Vector3 GetEnemyModifiedPos()
    {
        Vector3 modifiedPos = new Vector3(transform.position.x, transform.position.y + _distanceCheckOffset, 0f);
        return modifiedPos;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _agroRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _fleeDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _fleeDistance);


        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(GetEnemyModifiedPos(), _attackPlayerDistance);
    }

}
