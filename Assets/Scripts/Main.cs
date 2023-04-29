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
    public bool a = false, b = false;
    // Update is called once per frame
    void Update()
    {
        if (a) { ChangeToKnight(); a = false; }
        else if (b) { ChangeToFrog();b = false; }
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
