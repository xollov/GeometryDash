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
    public GameObject[] renderers;
    public ModeType startMode;
    public ModeType currentMode;
    Vector3 startPos;
    void Awake() 
    {
        rb = GetComponent<Rigidbody2D>();
        SwitchMode(startMode);
        startPos = transform.position;
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
            case ModeType.Wheel:
                if(isGrounded && Input.GetMouseButtonDown(0))
                {
                    rb.gravityScale = -rb.gravityScale;
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
        if (other.gameObject.tag == "Obstacle" && !isDead)
        {
            foreach (var r in renderers)r.SetActive(false);
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
        SwitchMode(startMode);
        transform.position = startPos;
        isDead = false;
    }
    void SwitchMode(ModeType mode)
    {
        currentMode = mode;
        if (rb.gravityScale < 0)
        {
            rb.gravityScale = -rb.gravityScale;
        }
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].SetActive((int)mode == i);
        }
        switch(mode)
        {
            case ModeType.Cube:
                break;
            case ModeType.Fly:
                renderers[0].SetActive(true);
               break; 
            case ModeType.Wheel:
                break;
        } 
    }
}
