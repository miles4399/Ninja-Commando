using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearingPlatform : MonoBehaviour
{
    [SerializeField] private float _destructionDelay;
    [SerializeField] private float _respawnTime = 3f;
    [SerializeField] private GameObject _platform;
    [SerializeField] private GameObject _disappearSFX;

    public void StartTimer(float timerValue)
    {
        StartCoroutine(PlatformDisableTimer(timerValue));
        Instantiate(_disappearSFX, transform.position, transform.rotation);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator PlatformDisableTimer(float timerValue)
    {
        yield return new WaitForSeconds(timerValue);
        _platform.SetActive(false);
        yield return new WaitForSeconds(_respawnTime);
        _platform.SetActive(true);
    }


}
