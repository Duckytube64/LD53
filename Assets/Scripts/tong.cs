using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tong : MonoBehaviour
{
    public Rigidbody2D hook;
    public GameObject[] prefabTongSegs;
    public int numLinks;
    // Start is called before the first frame update
    void Start()
    {
        GenerateTong();
    }

    void GenerateTong()
    {
        Rigidbody2D prevBod = hook;
        for(int i = 0; i < numLinks; i++)
        {
            GameObject newSeg;
            if (i == (numLinks - 1))
            {
                newSeg = Instantiate(prefabTongSegs[1]);
            }
            else
            {
                newSeg = Instantiate(prefabTongSegs[0]);
            }
            newSeg.transform.parent = transform;
            newSeg.transform.position = transform.position;
            HingeJoint2D hj = newSeg.GetComponent<HingeJoint2D>();
            hj.connectedBody = prevBod;

            prevBod = newSeg.GetComponent<Rigidbody2D>();
            
        }
        
    }
}
