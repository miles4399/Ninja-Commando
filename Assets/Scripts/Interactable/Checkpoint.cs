using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    

    [SerializeField] private GameObject[] _checkpoint;
    private int _index = -1;

    private void Start()
    {
        _checkpoint = GameObject.FindGameObjectsWithTag("CheckPoints");
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Keypad5))
        {
            _index++;
            // % divides and gives the remainder, allow for us to loop through checkpoints without going out of range
            transform.position = _checkpoint[_index % _checkpoint.Length].transform.position;
        }
    }
}
