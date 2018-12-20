using UnityEngine;

public class Movement : MonoBehaviour
{
    float speed = 5f;
    
    void Update()
    {
        // Move to the right
        if (Input.GetKey(KeyCode.D))
           transform.Translate(Vector3.right * Time.deltaTime * speed);

        // Move to the left
        if (Input.GetKey(KeyCode.A))
            transform.Translate(Vector3.left * Time.deltaTime * speed);

        // Move up
        if (Input.GetKey(KeyCode.W))
            transform.Translate(Vector3.up * Time.deltaTime * speed);

        // Move down
        if (Input.GetKey(KeyCode.S))
           transform.Translate(Vector3.down * Time.deltaTime * speed);
    }
}
