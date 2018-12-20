using UnityEngine.Networking;

public class SetupLocalPlayer : NetworkBehaviour
{
    void Start()
    {
        if (isLocalPlayer)
            GetComponent<SpritePlayer>().enabled = true;
        else
            GetComponent<SpritePlayer>().enabled = false;
    }
}
