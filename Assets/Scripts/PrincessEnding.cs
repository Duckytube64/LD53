using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PrincessEnding : MonoBehaviour
{

    public GameObject prince, frog, text;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.name == "Frog")
        {
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            prince.GetComponent<SpriteRenderer>().enabled = true;
            Destroy(frog);
            text.GetComponent<Canvas>().enabled = true;
        }
    }
}
