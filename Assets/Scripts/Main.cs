using System.Collections;
using System.Collections.Generic;
using TarodevController;
using UnityEngine;

public class Main : MonoBehaviour
{
    [SerializeField]
    FollowCamera followCamera;
    [SerializeField]
    PlayerController knight, frog;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) ChangeToKnight();
        else if (Input.GetKeyDown(KeyCode.E)) ChangeToFrog();
    }

    void ChangeToKnight()
    {
        frog.deactivated = true;
        knight.deactivated = false;
        followCamera.SetCameraWeightTarget(0);
    }

    void ChangeToFrog()
    {
        knight.deactivated = true;
        frog.deactivated = false;
        followCamera.SetCameraWeightTarget(1);
    }
}
