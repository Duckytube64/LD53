using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConnect : MonoBehaviour
{
    public Rigidbody2D rb;
    public HingeJoint2D hj;
    public bool attached = false;
    public float pushForce = 10f;

    // Start is called before the first frame update
    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        hj = gameObject.GetComponent<HingeJoint2D>();
        hj.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
    }

    void CheckInput()
    {
        Vector3 go = new Vector3(1, 0, 0);
        Vector3 goback = new Vector3(-1, 0, 0);
        if (this.tag == "Frog")
        {
            if (Input.GetKey("d"))
            {
                if (attached)
                {
                    rb.transform.position += new Vector3(1, 0, 0);
                    //rb.AddRelativeForce(go * pushForce);
                }
            }
            if (Input.GetKey("a"))
            {
                if (attached)
                {
                    rb.AddRelativeForce(goback * pushForce);
                }
            }
        }
    }
    public void Attach(Rigidbody2D ropeBone)
    {
        //ropeBone.gameObject.GetComponent<RopeSegment>().isPlayerAttached = true;
        hj.connectedBody = ropeBone;
        hj.enabled = true;
        attached = true;
        //attachedTo = ropeBone.gameObject.transform.parent;
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        UnityEngine.Debug.Log(col);
        Attach(col.gameObject.GetComponent<Rigidbody2D>());

    }
}
