using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxScrolling : MonoBehaviour
{
    Transform cam;
    Vector3 camStartPosition;
    float distance;
    Material mat;
    float speed = 0.02f;

    float farthestBack;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;
        camStartPosition = cam.position;
        mat = GetComponent<Renderer>().material;

    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = new Vector3(cam.position.x, cam.position.y, 100);
        distance = cam.position.x - camStartPosition.x;
        mat.SetTextureOffset("_MainTex" , new Vector2(distance,0) * speed);
    }
}
