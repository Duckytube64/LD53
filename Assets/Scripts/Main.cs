using System.Collections;
using System.Collections.Generic;
using TarodevController;
using UnityEngine;

public class Main : MonoBehaviour
{
    [SerializeField]
    FollowCamera followCamera;
    [SerializeField]
    PlayerController knightCon, frogCon;
    Transform knightT, frogT;
    Rigidbody2D knightR, frogR;
    CircleCollider2D frogCol;

    public float throwForce = 50;
    bool frogGrabbed = false, isKnight = true;
    bool thrown = false;

    // Start is called before the first frame update
    void Start()
    {
        knightT = knightCon.gameObject.transform;
        frogT = frogCon.gameObject.transform;
        frogR = frogCon.gameObject.GetComponent<Rigidbody2D>();
        frogCol = frogCon.transform.GetComponent<CircleCollider2D>();
        frogCol.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (isKnight)
                ChangeToFrog();
            else
                ChangeToKnight();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (frogGrabbed)
                DropFrog();
            else if (knightCon.isControlled && (knightT.position - frogT.position).magnitude < 0.75f)
                GrabFrog();
        }

        // Throw frog
        if (Input.GetMouseButtonDown(0) && frogGrabbed)
            ThrowFrog();
    }

    void ChangeToKnight()
    {
        frogCon.isControlled = false;
        knightCon.isControlled = true;
        followCamera.SetCameraWeightTarget(0);
        isKnight = true;
    }

    void ChangeToFrog()
    {
        if (frogGrabbed) return;
        knightCon.isControlled = false;
        frogCon.isControlled = true;
        followCamera.SetCameraWeightTarget(1);

        RBDisable();
        
        isKnight = false;
    }

    void RBEnable()
    {
        if (!thrown)
        {
            frogR.gravityScale = 2;
            frogR.drag = 1.5f;
            frogCon.enabled = false;
            frogCol.enabled = true;
        }
    }

    void RBDisable()
    {
        if (thrown)
        {
            frogR.gravityScale = 0;
            frogR.drag = 0;
            frogR.angularVelocity = 0;
            frogR.transform.rotation = Quaternion.Euler(Vector3.zero);
            frogR.velocity = Vector3.zero;
            frogCon.transform.Find("Visual").Find("SpriteHolder").rotation = Quaternion.Euler(Vector3.zero);
            frogCon.enabled = true;
            frogCol.enabled = false;
            thrown = false;
        }
    }

    void GrabFrog()
    {
        RBDisable();

        frogT.parent = knightT;
        frogT.localPosition = Vector3.zero;
        frogCon.isKinematic = true;
        frogGrabbed = true;
    }

    void DropFrog()
    {
        frogT.parent = GameObject.Find("Player characters").transform;
        frogCon.isKinematic = false;
        frogGrabbed = false;
    }

    void ThrowFrog()
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPosition.z = 0;
        Vector3 diffN = (worldPosition - knightT.position).normalized * throwForce;
        RBEnable();
        DropFrog();
        frogR.AddForce(diffN);

        thrown = true;
    }
}
