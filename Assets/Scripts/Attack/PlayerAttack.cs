using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private AttackAttributes _attackAttributes;
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private Transform _throwPoint;
    [SerializeField] private CharacterMovement _characterMovement;
    [SerializeField] private Shuriken _shuriken;
    [SerializeField] private GameObject _shurikenThrow;
    [SerializeField] private GameObject _swordStrike;
    [SerializeField] private GameObject _checkpointSFX;
    [SerializeField] private TextPopups _textPopups;
    [SerializeField] private Camera _cam;
    [SerializeField] private float _soundDelay = 0.5f;
    [SerializeField] private LayerMask _ammoLayer;
    [SerializeField] private GameObject _robotDeathSFX;
    [SerializeField] private GameObject _droneDeathSFX;
    [SerializeField] private float _sfxDelay = 0.3f;
    [SerializeField] private GameObject _swordHitsAirSFX;
 

    public Vector2 ThrowPosition => _throwPoint.position;

    public bool AttackDisabled { get => _attackDisabled; set => _attackDisabled = value; }

    private float TimeBtwAttack;            //Attack Cooldown
    private Vector3 _attackPointPosition;
    private SpriteRenderer _sprite;
    private Vector2 _throwDirection = Vector2.right;
    private bool _attackDisabled;
    private bool _hasPlaySound = false;
    private AnimationController _animationController;


    private void Start()
    {
        _sprite = GetComponentInChildren<SpriteRenderer>();
        _animationController = GetComponentInChildren<AnimationController>();
        _attackPointPosition = _attackPoint.localPosition;
    }

    private void Update()
    {
        TimeBtwAttack -= Time.deltaTime;

        if (_characterMovement.IsWallJumping)
        {   
            _attackPoint.localPosition = _characterMovement.WallJumpDirection.x < 0f ?
               new Vector3(-_attackPointPosition.x, _attackPointPosition.y, 0f) : _attackPointPosition;
        }
        else if (_characterMovement.HasMoveInput)
        {
            _attackPoint.localPosition = _characterMovement.MoveInput.x < 0f ? 
                new Vector3(-_attackPointPosition.x, _attackPointPosition.y, 0f) : _attackPointPosition;
        }

        Vector2 mousePosition = _cam.ScreenToWorldPoint(Input.mousePosition);
        _throwDirection = (mousePosition - ThrowPosition).normalized;
        Debug.DrawRay(ThrowPosition, _throwDirection * 5f, Color.red);


    }



    public void Attack()
    {
        if (TimeBtwAttack > 0 || AttackDisabled == true) return;
        _animationController.PlayAttackAnimation();
        Instantiate(_swordHitsAirSFX, transform.position, transform.rotation);
        TimeBtwAttack = _attackAttributes.StartTimeBtwAttack;

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(_attackPoint.position, _attackAttributes.AttackRange, _attackAttributes.EnemyLayers);

        foreach(Collider2D enemy in hitEnemies)
        {

            SwordStrikeSound();
            

            Invoke("ReactionSound", _sfxDelay);
            enemy.GetComponent<EnemyAI>()?.Kill();
            
            
        }
        Collider2D[] hitWalls = Physics2D.OverlapCircleAll(_attackPoint.position, _attackAttributes.AttackRange, _attackAttributes.WallLayers);
        foreach(Collider2D wall in hitWalls)
        {
            SwordStrikeSound();
            Invoke("ReactToCheckpoint", _soundDelay);
            //Destroy(wall.gameObject);
            PlayerDeath player = GetComponent<PlayerDeath>();
            player.ChangeSpawnPoint();
            _textPopups?.EnableText();

            wall.GetComponent<WallChange>()?.SpriteSwitch();
        }
        Collider2D[] hitDrones = Physics2D.OverlapCircleAll(_attackPoint.position, _attackAttributes.AttackRange, _attackAttributes.DroneLayer);
        foreach (Collider2D drone in hitDrones)
        {
            drone.GetComponent<DroneAI>() ?.Kill();
            SwordStrikeSound();
            Invoke("ReactionSound2", _sfxDelay);
        }
        Collider2D[] hitConsole = Physics2D.OverlapCircleAll(_attackPoint.position, _attackAttributes.AttackRange, _attackAttributes.ConsoleLayer);
        foreach (Collider2D console in hitConsole)
        {
            console.GetComponent<Console>()?.DestroyObject();
            SwordStrikeSound();
        }

        Collider2D[] hitAmmo = Physics2D.OverlapCircleAll(_attackPoint.position, _attackAttributes.AttackRange, _ammoLayer);
        foreach (Collider2D ammo in hitAmmo)
        {
            //Destroy(ammo.gameObject);
        }


    }

    public void SwordStrikeSound()
    {
        Instantiate(_swordStrike, transform.position, transform.rotation);              
    }

    public void Throw()
    {
        Shuriken shuriken = Instantiate(_shuriken, _throwPoint.position, Quaternion.identity);
        shuriken.SetDirecetion(_throwDirection);
       
        _animationController.PlayThrowAnimation();
        Instantiate(_shurikenThrow, transform.position, transform.rotation);
        
    }



    private void ReactToCheckpoint()
    {
        Instantiate(_checkpointSFX, transform.position, transform.rotation);
    }
    private void ReactionSound()
    {
        Instantiate(_robotDeathSFX, transform.position, transform.rotation);
    }
    private void ReactionSound2()
    {
        Instantiate(_droneDeathSFX, transform.position, transform.rotation);
    }
    private void OnDrawGizmosSelected()
    {
        if (_attackPoint == null)
            return;

        Gizmos.DrawWireSphere(_attackPoint.position, _attackAttributes.AttackRange);
    }
}
