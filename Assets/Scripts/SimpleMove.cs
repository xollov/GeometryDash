using UnityEngine;

public class SimpleMove : MonoBehaviour
{
    public Rigidbody2D rb;
    public bool isGrounded;
    public bool isDead = false;
    public float jumpForce = 10f;
    public float flyForce = 1f;
    public float velocityCap = 5;
    public float speed = 2f;
    public GameObject particle;
    public GameObject renderer;
    public GameObject shipRenderer;
    public ModeType currentMode;
    Vector3 startPos;
    void Awake() 
    {
        SwitchMode(ModeType.Cube);
        startPos = transform.position;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        switch (currentMode) 
        {
            case ModeType.Cube:
                if (isGrounded && Input.GetMouseButton(0)) 
                {
                    rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                    isGrounded = false;
                }
                break;
            case ModeType.Fly:
                if(Input.GetMouseButton(0)) 
                {
                    rb.AddForce(Vector2.up * flyForce, ForceMode2D.Impulse);
                    if (rb.linearVelocity.y > velocityCap) 
                    {
                        rb.linearVelocity = new Vector2(rb.linearVelocity.x, velocityCap);
                    }
                    if (rb.linearVelocity.y < -velocityCap) 
                    {
                        rb.linearVelocity = new Vector2(rb.linearVelocity.x, -velocityCap);
                    }
                }
                break;
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
        if (other.TryGetComponent<Portal>(out var mode)) 
        {
           SwitchMode(mode.portalType); 
        }
    }
    void Respawn()
    {
        renderer.SetActive(true);
        transform.position = startPos;
        isDead = false;
    }
    void SwitchMode(ModeType mode)
    {
        currentMode = mode;
        shipRenderer.SetActive(mode == ModeType.Fly);
    }
}
