using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject key;
    public Sprite open;

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject == key)
        {
            transform.gameObject.layer = 7;
            transform.gameObject.GetComponent<SpriteRenderer>().sprite = open;
        }
    }
}
