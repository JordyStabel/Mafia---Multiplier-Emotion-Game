using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SpritePlayer : NetworkBehaviour
{
    float speed = 5f;
    public Text playerNameText;
    public Slider slider;

    [SyncVar]
    public string playerName = "Player";

    [SyncVar]
    public Color playerColor = Color.white;

    // The Alldex2Slider instance
    public Affdex2Slider Affdex2Slider { get { return Affdex2Slider.Instance; } }

    [SyncVar]
    private float emotionValue = 0.5f;

    private float timeLeft = 1f;

    public void OnGUI()
    {
        if (isLocalPlayer == true)
        {
            // Some if this might need to get moved outside the if statement, otherwise not all players will see everything
            playerName = GUI.TextField(new Rect(25, Screen.height - 40, 100, 30), playerName);
            if (GUI.Button(new Rect(130, Screen.height - 40, 80, 30), "Change"))
            {
                if (isLocalPlayer)
                    CmdChangeName(playerName);
                else if (isServer)
                    RpcChangeName(playerName);
            }
        }
    }

    void OnEnable()
    {
        transform.position = new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), 0f);
        SpriteRenderer render = GetComponent<SpriteRenderer>();
        render.color = playerColor;

        //this.transform.parent = GameObject.Find("BarContainer").transform;
        this.transform.SetParent(GameObject.Find("BarContainer").transform);
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start");
        CmdChangeName(playerName);

        SpriteRenderer render = GetComponent<SpriteRenderer>();
        render.color = playerColor;
    }

    // Update is called once per frame
    void Update()
    {
        //playerNameText.transform.position = transform.position;

        timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {
            emotionValue = Affdex2Slider.EmotionValue;
            CmdChangeEmotion(emotionValue);

            //CmdChangeName(Affdex2Slider.EmotionValue.ToString());
            timeLeft = 1f;
        }

        
        //Affdex2Slider.EmotionValue = 0.5f;

        #region Replaced with Movement script

        //// Move to the right
        //if (Input.GetKey(KeyCode.D))
        //    transform.Translate(Vector3.right * Time.deltaTime * speed);

        //// Move to the left
        //if (Input.GetKey(KeyCode.A))
        //    transform.Translate(Vector3.left * Time.deltaTime * speed);

        //// Move up
        //if (Input.GetKey(KeyCode.W))
        //    transform.Translate(Vector3.up * Time.deltaTime * speed);

        //// Move down
        //if (Input.GetKey(KeyCode.S))
        //    transform.Translate(Vector3.down * Time.deltaTime * speed);

        #endregion
    }

    [Command]
    public void CmdChangeEmotion(float value)
    {
        emotionValue = value;
        slider.value = value;
        //Affdex2Slider.EmotionValue = emotionValue;
        // Change slider

        RpcChangeEmotion(value);
    }

    [ClientRpc]
    public void RpcChangeEmotion(float value)
    {
        emotionValue = value;
        slider.value = value;
        //Affdex2Slider.EmotionValue = emotionValue;
        // Change slider
    }

    [Command]
    public void CmdChangeName(string name)
    {
        playerName = name;
        playerNameText.text = playerName;

        RpcChangeName(playerName);
    }

    [ClientRpc]
    public void RpcChangeName(string name)
    {
        playerName = name;
        playerNameText.text = playerName;
    }
}
