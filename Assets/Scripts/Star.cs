using UnityEngine;

public class Star : MonoBehaviour
{
   void OnTriggerEnter2D(Collider2D other)
   {
     if (other.GetComponent<Movement>())
        {
            gameObject.SetActive(false);
        }
   }
}
