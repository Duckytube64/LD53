using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WalkingEnemy : MonoBehaviour
{

    public float speed;
    float last_direction_change = 0;
    Collider2D wall_detector;
    public Collider2D wall1, wall2;
    public bool bounce = false;

    void Start()
    {
        wall_detector = transform.GetChild(0).GetComponent<Collider2D>();
        FlipSprite();
        wall1.GetComponent<SpriteRenderer>().enabled = false;
        wall2.GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Rigidbody2D>().freezeRotation = true;
    }

    void Update()
    {
        transform.position += Time.deltaTime * new Vector3(speed, 0, 0);
        if ((wall_detector.IsTouching(wall1) || wall_detector.IsTouching(wall2)) && Time.time - last_direction_change > 0.5f)
        {
            speed *= -1;
            last_direction_change = Time.time;
            FlipSprite(); 
        }
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Knight") && bounce == false)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void FlipSprite()
    {
        if (speed < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
