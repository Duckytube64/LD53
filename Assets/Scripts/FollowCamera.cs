using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform knight, frog;
    public float knightWeight = 1, frogWeight = 1;

    // Update is called once per frame
    void Update()
    {
        float totWeight = knightWeight + frogWeight;
        Vector3 targetPos = (knight.position * knightWeight + frog.position * frogWeight);
        targetPos /= totWeight;
        targetPos.z = -10;
        transform.position = targetPos;
    }
}
