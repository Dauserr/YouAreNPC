using UnityEngine;

public class JohnWalking : MonoBehaviour
{
    public float speed = 5f;  // movement speed

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

   
}
