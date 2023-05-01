using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum CameraMode { Free, WAverage }

public class FollowCamera : MonoBehaviour
{
    public Transform knight, frog;
    float camWeight = 1; // 0 = on knight, 1 = on frog, in between is in between :^)
    float camWeightTarget = 1, camLerpSpeed = 7, minSpeed = 0.001f;
    CameraMode mode = CameraMode.WAverage;

    private void Start()
    {
        camWeightTarget = camWeight;
    }

    void Update()
    {
        switch (mode)
        {
            case CameraMode.WAverage:
                if (camWeight != camWeightTarget)
                {
                    float diff = Mathf.Abs(camWeightTarget - camWeight);
                    float change = Mathf.Max(diff * Time.deltaTime * camLerpSpeed, minSpeed);
                    if (camWeight < camWeightTarget) camWeight += change;
                    else camWeight -= change;

                    if (Mathf.Abs(camWeight - camWeightTarget) < minSpeed) 
                        camWeight = camWeightTarget;
                }

                Vector3 newPos = (1 - camWeight) * knight.position + camWeight * frog.position;
                newPos.z = -10;
                transform.position = newPos;
                break;
        }
    }

    public void ChangeCameraMode(CameraMode mode)
    {
        this.mode = mode;
    }

    public void SetCameraWeightTarget(float target)
    {
        camWeightTarget = Mathf.Clamp01(target);
    }

    public void SetCameraWeight(float target)
    {
        camWeight = Mathf.Clamp01(target);
    }
}
