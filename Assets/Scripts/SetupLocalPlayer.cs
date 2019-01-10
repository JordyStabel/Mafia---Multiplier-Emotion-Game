using UnityEngine;
using UnityEngine.Networking;

public class SetupLocalPlayer : NetworkBehaviour
{

    void Start()
    {
        if (isLocalPlayer)
        {
            GetComponent<Movement>().enabled = true;
            GameObject test = this.transform.Find("Canvas/EmotionBar/Slider").gameObject;
            test.GetComponent<Affdex2Slider>().enabled = true;
        }
        else
        {
            GetComponent<Movement>().enabled = false;
        }
    }
}
