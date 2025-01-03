using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyBeh : MonoBehaviour
{
    public int hp = 10;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "¤p§L")
        {
            hp--;
            Destroy(other.gameObject);
            if (hp <= 0 ) Destroy(this.gameObject);
        }
    }
}
