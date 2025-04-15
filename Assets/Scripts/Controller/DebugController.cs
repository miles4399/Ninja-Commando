using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugController : MonoBehaviour
{
    [SerializeField] private string _soilderTag = "Soilder";
    [SerializeField] private string _droneTag = "Drone";
    [SerializeField] private string _tapeTag = "Tape";


    private GameObject[] _soilder;
    private GameObject[] _drone;
    private GameObject[] _tape;




    void Start()
    {
        _soilder = GameObject.FindGameObjectsWithTag(_soilderTag);
        _drone = GameObject.FindGameObjectsWithTag(_droneTag);
        _tape = GameObject.FindGameObjectsWithTag(_tapeTag);




    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Keypad1))
        {
            for (int i = 0; i < _drone.Length; i++)
            {
                Destroy(_drone[i]);
            }
            for (int i = 0; i < _soilder.Length; i++)
            {
                Destroy(_soilder[i]);
            }
        }

        if(Input.GetKeyDown(KeyCode.Keypad2))
        {
            for (int i = 0; i < _tape.Length; i++)
            {
                Destroy(_tape[i]);
                Gate.tapeCount = Gate.totalTape;
            }
        }

        if(Input.GetKeyDown(KeyCode.Keypad3))
        {
            PlayerDeath player = GetComponent<PlayerDeath>();
            player.ToggleVulnerable();
        }

        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            PlayerDeath player = GetComponent<PlayerDeath>();
            player.ReturnCheckPoint();
        }
    }
}
