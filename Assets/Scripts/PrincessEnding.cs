using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PrincessEnding : MonoBehaviour
{

    public GameObject prince, text;

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.name == "Frog")
        {
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            prince.GetComponent<SpriteRenderer>().enabled = true;
            Destroy(col.gameObject);
            text.GetComponent<Canvas>().enabled = true;
        }
    }
}
