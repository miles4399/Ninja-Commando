using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{

    public void OnTriggerEnter2D(Collider2D other)
    {

        PlayerDeath player = other.GetComponent<PlayerDeath>();
        if (player != null) player?.Death();

        SelfDistruct();

    }

    public void SelfDistruct()
    {
        Destroy(this.gameObject);
    }

}
