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
    Transform knightT, frogT;

    bool frogGrabbed = false;

    // Start is called before the first frame update
    void Start()
    {
        knightT = knight.gameObject.transform;
        frogT = frog.gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) ChangeToKnight();
        else if (Input.GetKeyDown(KeyCode.E)) ChangeToFrog();

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (frogGrabbed)
                DropFrog();
            else if (knight.isControlled && (knightT.position - frogT.position).magnitude < 0.75f)
                GrabFrog();
        }
    }

    void ChangeToKnight()
    {
        frog.isControlled = false;
        knight.isControlled = true;
        followCamera.SetCameraWeightTarget(0);
    }

    void ChangeToFrog()
    {
        if (frogGrabbed) return;
        knight.isControlled = false;
        frog.isControlled = true;
        followCamera.SetCameraWeightTarget(1);
    }

    void GrabFrog()
    {
        frogT.parent = knightT;
        frogT.localPosition = Vector3.zero;
        frog.isKinematic = true;
        frogGrabbed = true;
    }

    void DropFrog()
    {
        frogT.parent = GameObject.Find("Player characters").transform;
        frog.isKinematic = false;
        frogGrabbed = false;
    }
}
