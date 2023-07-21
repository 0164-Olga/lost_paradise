using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemHeart : MonoBehaviour
{
    public int healthValue;

    private void OnTriggerEnter2D (Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<player>().IncreaseLife(healthValue);
            Destroy(gameObject);
        }
    }
}
