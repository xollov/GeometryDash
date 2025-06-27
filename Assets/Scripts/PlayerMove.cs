using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public Vector2 velocity = Vector2.zero;
    public float gravityScale = 9.8f;
    public float jumpVelocity = 20f;
    public bool isGrounded = false;
    public LayerMask playerLayer;
    public LayerMask obstacleLayer;
    // Update is called once per frame
    void Update() 
    {
        if (isGrounded)
        {
            if (Input.GetMouseButtonDown(0)) 
            {
                isGrounded = false;
                velocity.y = jumpVelocity;
            }
        }
    }
    void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Linecast(transform.position, (Vector2)transform.position- 3 * Vector2.up, playerLayer);
        if (hit) 
        {
            print(hit.transform);
            if (hit.transform.tag == "Ground")
                isGrounded = true;
        }
        if (isGrounded) 
        {
            velocity.y = 0f;
        }
        else 
        {
            velocity.y -= gravityScale * Time.fixedDeltaTime; 
            if (velocity.y < -9.8f) 
            {
                velocity.y = -9.8f;  
            }
        }
        transform.position = transform.position + (Vector3)velocity;
    }
}
