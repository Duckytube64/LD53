using TarodevController;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Unity.Burst.Intrinsics.X86.Avx;
using UnityEngine.SceneManagement;

public struct HasItem
{
    public bool holdsItem;
    public GameObject heldItem;
}

public class Main : MonoBehaviour
{
    [SerializeField]
    FollowCamera followCamera;
    [SerializeField]
    PlayerController knightCon, frogCon;
    Transform knightT, frogT;
    Rigidbody2D frogR;
    CircleCollider2D frogCol;
    HasItem knightI, frogI;

    public float throwForce = 50;
    bool thrown = false;
    TrailRenderer trail;

    void Start()
    {
        knightI = new HasItem();
        frogI = new HasItem();
        knightT = knightCon.gameObject.transform;
        frogT = frogCon.gameObject.transform;
        frogR = frogCon.gameObject.GetComponent<Rigidbody2D>();
        frogCol = frogCon.transform.GetComponent<CircleCollider2D>();
        frogCol.enabled = false;
        frogCon.isFrog = true;

        trail = frogT.Find("Visual").GetComponent<TrailRenderer>();
        trail.enabled = false;
    }

    void Update()
    {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (knightCon.isControlled)
                ChangeToFrog();
            else
                ChangeToKnight();
        }

        ref HasItem hi = ref knightI;
        if (frogCon.isControlled) hi = ref frogI;

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

            if (Input.GetKeyDown(KeyCode.F))
        {
            if (knightCon.isControlled && hi.holdsItem && knightI.heldItem.name == "Frog")
                DropFrog();
            else if (hi.holdsItem)
                DropItem(ref hi);
            else
            {
                var objs = GameObject.FindGameObjectsWithTag("Pick up");
                float minDist = 1;
                GameObject obj = null;
                Vector3 pos;
                if (knightCon.isControlled) pos = knightT.position;
                else pos = frogT.position;

                for (int i = 0; i < objs.Length; i++)
                {
                    float dist = (pos - objs[i].transform.position).magnitude;
                    if (!(frogCon.isControlled && objs[i].name == "Frog"))
                        if (dist < 1 && minDist > dist)
                        {
                            obj = objs[i];
                            minDist = dist;
                        }
                }
                if (obj)
                {
                    if (obj.name == "Frog" && !frogI.holdsItem)
                        GrabFrog();
                    else
                        GrabItem(obj, ref hi);
                }
            }
        }

        // Throw stuff
        if (Input.GetMouseButtonDown(0) && knightCon.isControlled)
        {
            if (hi.holdsItem)
            {
                if (hi.heldItem.name == "Frog")
                    ThrowFrog();
                else
                    ThrowItem();
            }
        }

        void ChangeToKnight()
        {
            frogCon.isControlled = false;
            knightCon.isControlled = true;
            followCamera.SetCameraWeightTarget(0);
        }

        void ChangeToFrog()
        {
            if (knightI.holdsItem && knightI.heldItem.name == "Frog") return;
            knightCon.isControlled = false;
            frogCon.isControlled = true;
            followCamera.SetCameraWeightTarget(1);

            RBDisable();
            trail.enabled = false;
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

        void GrabItem(GameObject g, ref HasItem hi)
        {
            if (frogI.heldItem == g || knightI.heldItem == g) return;
            Rigidbody2D r = g.GetComponent<Rigidbody2D>();
            r.isKinematic = true;
            r.velocity = Vector3.zero;
            r.angularVelocity = 0;
            g.transform.rotation = Quaternion.Euler(Vector3.zero);

            if (knightCon.isControlled)
            {
                g.transform.parent = knightT.Find("Visual");
                g.transform.localPosition = new Vector3(0.2f, 0, 0);
            }
            else if (frogCon.isControlled)
            {
                g.transform.parent = frogT.Find("Visual");
                g.transform.localPosition = new Vector3(0, 0.2f, 0);
            }
            hi.holdsItem = true;
            hi.heldItem = g;
            g.GetComponent<TrailRenderer>().enabled = false;
        }

        void GrabFrog()
        {
            RBDisable();

            frogT.parent = knightT;
            frogT.localPosition = Vector3.zero;
            frogCon.isKinematic = true;
            knightI.holdsItem = true;
            knightI.heldItem = frogT.gameObject;
            trail.enabled = false;
        }

        void DropItem(ref HasItem hi)
        {
            hi.heldItem.transform.parent = null;
            hi.heldItem.GetComponent<Rigidbody2D>().isKinematic = false;
            hi.holdsItem = false;
            hi.heldItem.GetComponent<TrailRenderer>().enabled = false;
            hi.heldItem = null;
        }

        void DropFrog()
        {
            frogT.parent = GameObject.Find("Player characters").transform;
            frogCon.isKinematic = false;
            knightI.holdsItem = false;
            knightI.heldItem = null;
        }

        void ThrowItem()
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPosition.z = 0;
            Vector3 diffN = (worldPosition - knightT.position).normalized * throwForce;
            GameObject tmp = knightI.heldItem;
            DropItem(ref knightI);
            tmp.GetComponent<Rigidbody2D>().AddForce(diffN);

            thrown = true;
            tmp.GetComponent<TrailRenderer>().enabled = true;
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
            trail.enabled = true;
        }
    }
}
