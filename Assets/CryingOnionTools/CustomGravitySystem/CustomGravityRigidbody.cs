using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CustomGravityRigidbody : MonoBehaviour
{
   private Rigidbody body;
   private float floatDelay;

   private void Awake()
   {
      body = GetComponent<Rigidbody>();
      body.useGravity = false;
   }

   private void FixedUpdate()
   {
      if (body.IsSleeping())
      {
         floatDelay = 0;
         return;
      }

      if (body.velocity.sqrMagnitude < 0.0001f)
      {
         floatDelay += Time.fixedDeltaTime;
         if(floatDelay >= 1) return;
      }
      else
      {
         floatDelay = 0;
      }
      
      body.AddForce(CustomGravity.GetGravity(body.position), ForceMode.Acceleration);
   }
}