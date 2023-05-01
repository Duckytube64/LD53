using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Intro : MonoBehaviour
{
    public AnimationCurve FadeCurve = new AnimationCurve(new Keyframe(0, 1), new Keyframe(0.6f, 0.7f, -1.8f, -1.2f), new Keyframe(1, 0));

    private float _alpha = 1;
    private Texture2D _texture;
    private bool _done;
    private float _time;

    GameObject main;
    string[] texts = new string[] { "My name is Prince Jonathoad, \nand I have turned into a frog!",
                                    "I can only roll around and can't \neven leave this pond...", 
                                    "Can you take me to the princess?\nShe can turn me back!",
                                    "Pick me up with F and \ncontrol me with Q",
                                    "Be careful but you can throw me \nwith your mouse ",
                                    "Bruh", "On my day off too..."};
    int textIndex = 0;
    [SerializeField] TMP_Text tmp;

    private void Start()
    {
        main = GameObject.Find("Main");
    }

    bool introDone = false;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (textIndex == texts.Length)
                introDone = true;
            else
            {
                tmp.text = texts[textIndex];
                textIndex++;
                // Move camera to knight
                if (textIndex == 6)
                    GetComponent<FollowCamera>().SetCameraWeightTarget(0);
            }
        }

        if (introDone || Input.GetKey(KeyCode.F))
        {
            GetComponent<FollowCamera>().SetCameraWeightTarget(0);
            Vector3 kpos = GameObject.Find("Knight").transform.position;
            kpos.z = -10;
            transform.position = kpos; 
            main.GetComponent<Main>().enabled = true;
            transform.Find("Textbox").gameObject.SetActive(false);
            GetComponent<Camera>().orthographicSize = 8;
            enabled = false;
        }
    }

    public void Reset()
    {
        _done = false;
        _alpha = 1;
        _time = 0;
    }

    [RuntimeInitializeOnLoadMethod]
    public void RedoFade()
    {
        Reset();
    }

    public void OnGUI()
    {
        if (_done) return;
        if (_texture == null) _texture = new Texture2D(1, 1);

        _texture.SetPixel(0, 0, new Color(0, 0, 0, _alpha));
        _texture.Apply();

        _time += Time.deltaTime;
        _alpha = FadeCurve.Evaluate(_time);
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), _texture);

        if (_alpha <= 0) _done = true;
    }
}