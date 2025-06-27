using UnityEngine;

public class SimpleMove : MonoBehaviour
{
    public Rigidbody2D rb;
    public bool isGrounded;
    public bool isDead = false;
    public float jumpForce = 10f;
    public float speed = 2f;
    public GameObject particle;
    public GameObject renderer;
    Vector3 startPos;
    void Awake() 
    {
        startPos = transform.position;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
       if (isGrounded && Input.GetMouseButton(0)) 
       {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
       } 
    }
    void FixedUpdate() 
    {
        if (!isDead)
        {
            transform.Translate(Vector2.right * speed * Time.fixedDeltaTime);
        }
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }
    void OnTriggerEnter2D(Collider2D other) 
    {
        print("Trigger");
        if (other.gameObject.tag == "Obstacle")
        {
            renderer.SetActive(false);
            Instantiate(particle,transform.position, Quaternion.identity);
            isDead = true;
            Invoke("Respawn", 0.5f);
        }
        if (other.gameObject.tag == "Finish")
        {
            print("WIN");
        }
    }
    void Respawn()
    {
        renderer.SetActive(true);
        transform.position = startPos;
        isDead = false;
    }
}
