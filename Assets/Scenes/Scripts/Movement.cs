using UnityEngine;
using System.Collections;

public enum Speed {Slow, Normal, Fast, Faster, Fastest};
public enum GameModes {Cube, Ship, UFO, Wheel, Spider, Wave, Robot};
/*
Note:   gravity scale  = 12.41067f
        jump force     = 26.6581f
        rotation speed = 452.425218f
*/
public class Movement : MonoBehaviour
{
    #region public
    public GameModes startGameMode;
    public Speed currentSpeed;
    public Transform cubeRenderer;
    public float groundCheckRadius;
    public LayerMask groundLayer;
    public LayerMask obstacleLayer;
    public GameObject particle;
    #endregion

    #region private
    GameModes currentGameMode;
    Vector3 startPosition;
    Rigidbody2D rb;
    float[] speedValues = {8.6f, 10.4f,12.96f,15.6f,19.27f};

    int gravity = 1;
    bool isDead;

    #endregion
    void Start()
    {
       rb = GetComponent<Rigidbody2D>(); 
       currentGameMode = startGameMode;
       startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead) return;


        if (TouchingWall() || ObstacleTouch())
        {
            print("Wall hit");
            StartCoroutine("Respawn");
        }

        transform.position += Vector3.right * speedValues[(int)currentSpeed] * Time.deltaTime;    

        Invoke(currentGameMode.ToString(), 0f); 
    }
    bool Grounded() 
    {
        int gravityMultiplier = (int) (Mathf.Abs(rb.gravityScale) / rb.gravityScale);
        return Physics2D.OverlapBox(transform.position + Vector3.up * -gravityMultiplier * 0.5f, Vector2.right * 1.1f + Vector2.up * groundCheckRadius, 0, groundLayer);
    }
    bool TouchingWall()
    {
        return Physics2D.OverlapBox(transform.position + (Vector3)Vector2.right * 0.55f, Vector2.up * 0.8f + Vector2.right * groundCheckRadius, 0, groundLayer);
    }
    bool ObstacleTouch()
    {
        return Physics2D.OverlapBox(transform.position, Vector2.up * 0.85f + Vector2.right * 0.85f, 0, obstacleLayer);
    }
    void LimitYVelocity(float limit)
    {
       var value = rb.linearVelocity;
       if (limit < 0)
       {
            limit *= -1;
       }
       value.y = Mathf.Clamp(value.y, -limit, limit);
       rb.linearVelocity = value;
    }
    void RoundRotation()
    {
        Vector3 rotation = cubeRenderer.rotation.eulerAngles;
        rotation.z = Mathf.Round(cubeRenderer.rotation.z / 90) * 90;
        cubeRenderer.rotation = Quaternion.Euler(rotation);
    }
    void Cube()
    {
        float initialVelocity = 26.6581f;
        float rotationSpeed = 409.1f;
        float gravityValue = 12.34f;
        if (Grounded())
        {
            RoundRotation();     
            if (Input.GetMouseButton(0))
            {
                rb.linearVelocity = Vector2.up * initialVelocity * gravity;  
                print("Jump");
            }
        }
        else 
        {
            cubeRenderer.Rotate(Vector3.back, rotationSpeed * Time.deltaTime);
        }
        rb.gravityScale = gravityValue * gravity;
    }
    void Ship()
    {
        float gravityValue = -4.314969f;
        float velocityLimit = 24.2f;
        LimitYVelocity(velocityLimit);
        cubeRenderer.rotation = Quaternion.Euler(0,0, rb.linearVelocity.y * 2);
        // if player holds then gravity scale is set -4.31..., so with upright gravity player goes up
        // if he doesnt gold then gravity scale set to positive 4.31... and layer goes down
        rb.gravityScale = gravity * gravityValue * (Input.GetMouseButton(0) ? 1 : -1);
        if (Grounded())
        {
            RoundRotation();
        }
        
    }
    void UFO()
    {
        float initialVelocity = 10.841f;
        float gravityValue = 4.1483f;
        float velocityLimit = 10.841f;
        LimitYVelocity(velocityLimit);
        if (Input.GetMouseButtonDown(0))
        {
            print("UFO click");
            rb.linearVelocity = Vector2.up * initialVelocity * gravity;
        }
        rb.gravityScale = gravityValue * gravity;
    }
    void Wheel()
    {
        float gravityValue = 6.2f;
        rb.gravityScale = gravityValue * gravity;
        if (Grounded())
        {
            if (Input.GetMouseButton(0))
            {
                rb.gravityScale = -rb.gravityScale;
                gravity = -gravity;
            }
        }
        
    }
    void Spider()
    {
        float initialVelocity = 238f;
        float gravityValue = 6.2f; 
        float velocityLimit = 238f;

        LimitYVelocity(velocityLimit);
        rb.gravityScale = gravityValue * gravity;
        if (Grounded())
        {
            RoundRotation();
            if(Input.GetMouseButtonDown(0))
            { 
                rb.linearVelocity = Vector2.up * initialVelocity * gravity;
                rb.gravityScale = -rb.gravityScale;
                gravity = -gravity;
            }
        }
    }
    void Wave()
    {
        rb.gravityScale = 0;
        rb.linearVelocity = new Vector2(0, speedValues[(int)currentSpeed] * (Input.GetMouseButton(0) ? 1 : -1) * gravity);

    }

    bool firstTouch = false;
    float robotXStart = -1;
    float roboJumpThreshold = 3;
    void Robot()
    {
        float initialVelocity = 10.4f;
        float gravityValue = 8.62f;


        bool isGrounded = Grounded();
        bool isMouseHeld = Input.GetMouseButton(0);        

        if (isGrounded)
        {
            firstTouch = false;
        }
        if (isMouseHeld && !firstTouch && isGrounded)
        {
            firstTouch = true;
            robotXStart = transform.position.x;
        }
        if (isMouseHeld)
        {
            if (Mathf.Abs(transform.position.x - robotXStart) <= roboJumpThreshold)
            {
                rb.linearVelocity = Vector2.up * initialVelocity * gravity;
            }
        }

        rb.gravityScale = gravityValue * gravity;

    }
    
    public void PortalChange(GameModes gameMode, Speed speed, bool uprightGravity, PortalType state)
    {
        switch (state)
        {
            case PortalType.GameMode:
                currentGameMode = gameMode;
                break;
            case PortalType.Speed:
                currentSpeed = speed;
                break;
            case PortalType.Gravity:
                rb.gravityScale = Mathf.Abs(rb.gravityScale) * (uprightGravity? 1 : -1);
                gravity = uprightGravity? 1 : -1;
                break;
        }
    }
    IEnumerator Respawn()
    {
        isDead = true;
        Destroy(Instantiate(particle, transform.position, Quaternion.identity), 0.5f);
        yield return new WaitForSeconds(0.4f);
        transform.position = startPosition;
        currentGameMode = startGameMode;
        isDead = false;
    }
}
