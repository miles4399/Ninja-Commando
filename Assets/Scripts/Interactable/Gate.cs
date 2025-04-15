using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Gate : MonoBehaviour
{
    [SerializeField] private GameObject _gate;
    [SerializeField] private GameObject _gatePortal;
    [SerializeField] private GameObject _particles;
    [SerializeField] private int _totalTape;
    [SerializeField] private GameObject _allTapesCollectedSFX;
    [SerializeField] private GameObject _enterGateSFX;

    



    public static int tapeCount;

    public static int totalTape;
    private bool _playSound = false;
    

    private void Awake()
    {
        totalTape = _totalTape;
    }

    void Start()
    {
        _gate.SetActive(false);
        _gatePortal.SetActive(false);
        _particles.SetActive(false);
        tapeCount = 0;
    }

     void Update()
    {
        if (tapeCount >= _totalTape)
        {
            ActiveGate();
            PlaySound();
        }
    }
        

    public void ActiveGate()
    {
        _gate.SetActive(true);
        _gatePortal.SetActive(true);
        _particles.SetActive(true);

    }
    private void PlaySound()
    {
        if (_playSound == false)
        {
            Instantiate(_allTapesCollectedSFX, transform.position, transform.rotation);
            _playSound = true;
        }
        
    }
    
    
    void OnTriggerEnter2D(Collider2D col)
    {
        
        Instantiate(_enterGateSFX, transform.position, transform.rotation);
        //sceneControl.LoadScene();
        
        //    PlayerDeath player = other.GetComponent<PlayerDeath>();
        //    if (player == null) return;
        //    player?.ChangeSpawnPoint();

    }
}
