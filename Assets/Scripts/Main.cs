using System.Collections;
using System.Collections.Generic;
using TarodevController;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

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
    GameObject grabbedItem = null;

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
            else if (grabbedItem)
                DropItem();
            else
            {
                var objs = GameObject.FindGameObjectsWithTag("Pick up");
                float minDist = 1;
                GameObject obj = null;
                for (int i = 0; i < objs.Length; i++)
                {
                    float dist = (knightT.position - objs[i].transform.position).magnitude;
                    if (knightCon.isControlled && dist < 0.75 && minDist > dist)
                    {
                        obj = objs[i];
                        minDist = dist;
                    }
                }
                if (obj)
                {
                    if (obj.name == "Frog")
                        GrabFrog();
                    else
                        GrabItem(obj);
                }
            }
        }

        // Throw stuff
        if (Input.GetMouseButtonDown(0))
        {
            if (frogGrabbed)
                ThrowFrog();
            else if (grabbedItem && !frogCon.isControlled)
                ThrowItem();
        }
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

    void GrabItem(GameObject g)
    {
        g.GetComponent<Rigidbody2D>().isKinematic = true;

        g.transform.parent = knightT;
        g.transform.localPosition = Vector3.zero;
        grabbedItem = g;
    }

    void GrabFrog()
    {
        RBDisable();

        frogT.parent = knightT;
        frogT.localPosition = Vector3.zero;
        frogCon.isKinematic = true;
        frogGrabbed = true;
    }

    void DropItem()
    {
        if (!grabbedItem) return;

        grabbedItem.transform.parent = null;
        grabbedItem.GetComponent<Rigidbody2D>().isKinematic = false;
        grabbedItem = null;
    }

    void DropFrog()
    {
        frogT.parent = GameObject.Find("Player characters").transform;
        frogCon.isKinematic = false;
        frogGrabbed = false;
    }

    void ThrowItem()
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPosition.z = 0;
        Vector3 diffN = (worldPosition - knightT.position).normalized * throwForce;
        GameObject tmp = grabbedItem;
        DropItem();
        tmp.GetComponent<Rigidbody2D>().AddForce(diffN);

        thrown = true;
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
