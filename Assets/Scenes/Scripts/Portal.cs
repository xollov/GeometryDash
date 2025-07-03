using UnityEngine;

public enum PortalType {GameMode, Speed, Gravity}
public class Portal : MonoBehaviour
{
   public GameModes gameMode; 
   public Speed speed;
   public bool uprightGravity;
   public PortalType state;

   void OnTriggerEnter2D(Collider2D other)
   {
      if (other.TryGetComponent<Movement>(out var player))
      {
        print("Player enter portal"); 
        player.PortalChange(gameMode, speed, uprightGravity, state);
      }
   }
}
