using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
#pragma warning disable 649

[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private MovementAttributes _movementAttributes;
    [SerializeField] private Animator _art;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Transform _spawnPosition;
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _audioJump;
    [SerializeField] private Vector2 _rayCastOriginModifier;
    [SerializeField] private SpriteRenderer _characterSprite;

    public MovementAttributes MovementAttributes => _movementAttributes;
    public bool IsGrounded { get; private set; } = true;
    public bool IsFudgeGrounded { get; private set; }

    private bool _canGrab;
    private bool _isClingToWall;

    public Vector3 GroundNormal { get; private set; } = Vector3.up;
    public Vector3 MoveInput {get; private set;}
    public bool HasMoveInput { get; private set; }
    public Vector3 LookDirection {get; private set;}
    public bool IsWallJumping { get; private set; }
    public Vector2 WallJumpDirection { get; private set; }
    
    public bool CanMove {get; set;} = true;
    public float MoveSpeedMultiplier { get; set; } = 1f;
    public float ForcedMovement { get; set; } = 0f;
    
    private Rigidbody2D _rigidbody;
    private float _groundedFudgeTimer;
    private int _currentJumpCount;
    private float _gravity;
    private Rigidbody2D _platformRB;

    public Vector3 Velocity
    {
        get => _rigidbody.velocity;
        set => _rigidbody.velocity = value;
    }
    public bool IsClingToWall { get => _isClingToWall; set => _isClingToWall = value; }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();                           //setting up rigidbody through code
        _rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        _rigidbody.interpolation = RigidbodyInterpolation2D.Interpolate;        
        _rigidbody.gravityScale = 0f;

        LookDirection = _art.transform.forward;

        _currentJumpCount = _movementAttributes.NumberOfJumps;
        _gravity = _movementAttributes.Gravity;


    }

    public void SetMoveInput(Vector3 input)
    {
        //Vector3 flattened = input.Flatten();
        MoveInput = Vector3.ClampMagnitude(input, 1f);
        HasMoveInput = input.magnitude > 0.1f;
    }

    public void Jump()      //Call from OnJump;
    {
        float gravity = _gravity;
        if (gravity == 0)
        {
            gravity = _movementAttributes.Gravity;
        }
        
           float jumpVelocity = Mathf.Sqrt(2f * -gravity * MovementAttributes.JumpHeight);
        if(_canGrab || _isClingToWall)
        {
            WallJump(jumpVelocity, WallJumpDirection, 1f);      //Check if the character is on the wall
            Instantiate(_audioJump, transform.position, transform.rotation);
        }
        else if (CanMove && _currentJumpCount > 0)          //Check if the character is on the ground and still have jumps avilable
        {
            GroundJump(jumpVelocity);
            Instantiate(_audioJump, transform.position, transform.rotation);

        }
    }

    public void Grab()
    {
        //if (_clingToWall)
        //{
        //    _gravity = _movementAttributes.Gravity;
        //    CanMove = true;
        //    _clingToWall = false;
        //}

        if (_canGrab && !_isClingToWall)
        {
            _gravity = 0f;
            Velocity = Vector2.zero;
            CanMove = false;
            _isClingToWall = true;
            PlayerAttack playerAttack = GetComponent<PlayerAttack>();
            playerAttack.AttackDisabled = true;
            
            
        }


    }

    private void WallJump(float jumpVelocity, Vector2 wallJumpDirection, float horizontalJumpModifier)
    {
        DisableWallCling();
        Velocity = new Vector3(wallJumpDirection.x * _movementAttributes.WallJumpHeight * horizontalJumpModifier, jumpVelocity, Velocity.z);
        StartCoroutine(MovementDisabler(_movementAttributes.MovementDisableTime));

    }

    private void DisableWallCling()
    {
        _gravity = _movementAttributes.Gravity;
        CanMove = true;
        _isClingToWall = false;
        PlayerAttack playerAttack = GetComponent<PlayerAttack>();
        playerAttack.AttackDisabled = false;

    }

    private IEnumerator MovementDisabler(float movementDisableTime)         //Freezing the character movement for a certain time after the wall jump
    {
        IsWallJumping = true;
        CanMove = false;
        yield return new WaitForSeconds(movementDisableTime);
        CanMove = true;
        IsWallJumping = false;
    }

    private void GroundJump(float jumpVelocity)                         //lose one jump count after jump
    {
        Velocity = new Vector3(Velocity.x, jumpVelocity, Velocity.z);
        _currentJumpCount--;
        
    }

    private void FixedUpdate()                  //Setting up movement;
    {
        float input = MoveInput.x;
        if (ForcedMovement > 0f) input = (LookDirection * ForcedMovement).x;
        float targetVelocity = (input * MovementAttributes.Speed * MoveSpeedMultiplier);
        if (_platformRB != null) targetVelocity += _platformRB.velocity.x * (1f - Mathf.Abs(MoveInput.x));
        float velocityDiff = targetVelocity - Velocity.x;
        float control = IsGrounded ? 1f : MovementAttributes.AirControl;
        Vector3 acceleration = Vector2.right * (velocityDiff * MovementAttributes.Acceleration * control);
        if (IsGrounded) acceleration += GroundNormal * _gravity;
        else acceleration += Vector3.up * _gravity;
        _rigidbody.AddForce(acceleration);
        Debug.Log(HasMoveInput);

   
    }

    private void Update()
    {                           //Checking if the character is on the ground or on the wall or not
        IsGrounded = CheckGrounded();
        if (IsGrounded) _groundedFudgeTimer = MovementAttributes.GroundedFudgeTime;
        else _groundedFudgeTimer -= Time.deltaTime;
        IsFudgeGrounded = _groundedFudgeTimer > 0f;

        _canGrab = CheckWall() && !IsGrounded;

        if (!CanMove)
        {
            Vector3 cannotMoveVector = new Vector3(0f, MoveInput.y, Velocity.z);
            SetMoveInput(cannotMoveVector);
        }
        if (_playerTransform.position.y < -20)          //if the character falls in to the void, reset it's position ( Debuging use)
        {
            _playerTransform.position = _spawnPosition.position;
            _currentJumpCount = _movementAttributes.NumberOfJumps;
        }
        if (!IsFudgeGrounded && _currentJumpCount == _movementAttributes.NumberOfJumps)
        {
            _currentJumpCount--;
        }
        if (IsWallJumping)
        {
            _currentJumpCount--;
        }
        if (IsGrounded)
        {
            _currentJumpCount = _movementAttributes.NumberOfJumps;          //Resets number of jumps when touched the ground
        }
        if(_canGrab)
        {
            Grab();
        }
        if (_isClingToWall == true && MoveInput.y < -0.1f)
        {
            float gravity = _movementAttributes.Gravity;
            float jumpVelocity = Mathf.Sqrt(Mathf.Abs(2f * gravity * MovementAttributes.WallFallHeight));
            WallJump(-jumpVelocity, WallJumpDirection, _movementAttributes.WallFallHeight);
        }




    }

    private bool CheckWall()        //Functional for now, add 3 more raycast for head height, body height and foot height after prototype;
    {
        int mod = _characterSprite.flipX == true ? -1 : 1;
        Vector2 origin = new Vector2(transform.position.x + _rayCastOriginModifier.x * mod, transform.position.y + _rayCastOriginModifier.y);
            
        Vector2 direction = new Vector2(1 * mod, 0f);
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, _movementAttributes.WallJumpDistance, _movementAttributes.WallMask);
        Debug.DrawRay(origin, direction, Color.green);
        if (hit.collider == null) 
        {
            DisableWallCling();
            return false;
        }
        WallJumpDirection = Vector2.Reflect(direction, hit.normal);
        return true;
    }

    private bool CheckGrounded()
    {
        GroundNormal = Vector3.up;
        Vector3 start = transform.TransformPoint(MovementAttributes.GroundCheckStart);
        Vector3 end = transform.TransformPoint(MovementAttributes.GroundCheckEnd);
        Vector3 diff = end - start;
        Vector3 dir = diff.normalized;
        float distance = diff.magnitude;
        RaycastHit2D hit = Physics2D.CircleCast(start, MovementAttributes.GroundCheckRadius, dir, distance, MovementAttributes.GroundMask);
        if(hit)
        {
            if (hit.rigidbody != null) _platformRB = hit.rigidbody;

            GroundNormal = hit.normal;
            bool angleValid = Vector3.Angle(Vector3.up, GroundNormal) < MovementAttributes.MaxSlopeAngle;
            if (angleValid) _groundedFudgeTimer = MovementAttributes.GroundedFudgeTime;
            return angleValid;
        }

        _platformRB = null;
        GroundNormal = Vector3.up;
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = IsGrounded ? Color.green : Color.red;
        Vector3 start = transform.TransformPoint(MovementAttributes.GroundCheckStart);
        Vector3 end = transform.TransformPoint(MovementAttributes.GroundCheckEnd);
        Gizmos.DrawWireSphere(start, MovementAttributes.GroundCheckRadius);
        Gizmos.DrawWireSphere(end, MovementAttributes.GroundCheckRadius);
    }
}