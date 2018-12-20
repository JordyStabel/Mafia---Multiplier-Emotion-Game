using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

// Derived of networkbehaviour to get access to all networking stuff. 
// It also derives from Monobehaviour, so that can still be used aswell
public class Player : NetworkBehaviour
{
    public GameObject playerCamera;
    public Transform playerContainer;
    public Image playerGraphics;
    public Text playerNameText;

    [SyncVar]
    public string playerName = "Player";

    [SyncVar]
    public Color playerColor = Color.white;

    public void OnGUI()
    {
        if (isLocalPlayer == true)
        {
            // Some if this might need to get moved outside the if statement, otherwise not all players will see everything
            playerName = GUI.TextField(new Rect(25, Screen.height - 40, 100, 30), playerName);
            if (GUI.Button(new Rect(130, Screen.height - 40, 80, 30), "Change"))
            {
                CmdChangeName();
            }
        }
    }

    public void Start()
    {
        if (isLocalPlayer == true)
        {
            this.enabled = true;
            playerCamera.GetComponent<Camera>().enabled = true;
        }
        else
        {
            playerCamera.GetComponent<Camera>().enabled = false;
            this.enabled = false;
        }
        playerGraphics.GetComponent<Image>().color = playerColor;
        playerNameText.text = playerName;
        
    }

    public void OnEnable()
    {
        Vector3 randomPos = new Vector3(Random.Range(-100, 100), 0.5f, Random.Range(-100, 100));
        playerContainer.transform.Translate(randomPos);
    }

    public void Update()
    {
        // Check if the current client is the 'owner' of this object
        if (isLocalPlayer == true)
        {
            // Move to the right
            if (Input.GetKey(KeyCode.D))
                playerContainer.transform.Translate(Vector3.right * Time.deltaTime * 30f);

            // Move to the left
            if (Input.GetKey(KeyCode.A))
                playerContainer.transform.Translate(Vector3.left * Time.deltaTime * 30f);

            // Debug
            if (Input.GetKey(KeyCode.Space))
                Debug.Log("Space pressed");
        }
    }

    [Command]
    public void CmdChangeName()
    {
        playerNameText.text = playerName;
    }


    [Command]
    public void CmdFireBullet()
    {
        //GameObject _bullet = (GameObject)Instantiate(bullet, bulletFiringPoint.transform.position, Quaternion.identity);
        //_bullet.transform.rotation = bulletFiringPoint.transform.rotation;

        //// Spawn there gameObject and have authority over it 
        //NetworkServer.SpawnWithClientAuthority(_bullet, connectionToClient);
    }
}
