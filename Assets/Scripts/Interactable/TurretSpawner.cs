using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _projectile;
    [SerializeField] private float _spawnTime;
    [SerializeField] private AudioSource _shootSFX;

    private void Start()
    {
        //A coroutine is a Unity method. Has to be called with StartCoroutine -> Won't work without it and won't display error without it
        //Tells us to run the function
        StartCoroutine(SpawnCo());
    }

    private IEnumerator SpawnCo()       //Co meaning coroutine, not a official naming convention
    {
        //always happen
        while (true)
        {
            //yield return means wait until this condition/time to happen before processing
            yield return new WaitForSeconds(_spawnTime);
            SpawnProjectile();
        }
    }

    private void SpawnProjectile()
    {
        Instantiate(_projectile, transform.position, transform.rotation);
        _shootSFX.Play();
    }
}
