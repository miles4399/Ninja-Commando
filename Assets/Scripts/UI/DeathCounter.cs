using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeathCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _uIText;
    [SerializeField] private IntCounter _deathCountEvent;

    


    private void OnEnable()
    {
        _deathCountEvent.CountChanged += SetDeathDisplayer;
        
    }

    private void OnDisable()
    {
        _deathCountEvent.CountChanged -= SetDeathDisplayer;
    }


    void Update()
    {
        
    }

    private void SetDeathDisplayer(int deathCount)
    {
        _uIText.text = deathCount.ToString();
    }
}
