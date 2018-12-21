using UnityEngine;
using UnityEngine.Networking;

public class SetupLocalPlayer : NetworkBehaviour
{

    void Start()
    {
        if (isLocalPlayer)
            GetComponent<Movement>().enabled = true;
        else
            GetComponent<Movement>().enabled = false;
    }
}
