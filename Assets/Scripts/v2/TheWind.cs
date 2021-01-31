using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheWind : MonoBehaviour
{
      public Rigidbody rb;
      public int clickForce = 500;
      private Plane plane = new Plane(Vector3.up, Vector3.zero);
  
      void Update () {
            
        if (Input.GetMouseButton(0))
         {
             var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
 
             float enter;
             if (plane.Raycast(ray, out enter))
             {
                 var hitPoint = ray.GetPoint(enter);               
                 var mouseDir = hitPoint - gameObject.transform.position;   
                 mouseDir = mouseDir.normalized;    
                 rb.AddForce(mouseDir * clickForce);

                 // Determine which direction to rotate towards
                Vector3 targetDirection = hitPoint - transform.position;

                // The step size is equal to speed times frame time.
                float singleStep = 0.01f * Time.deltaTime;

                // Rotate the forward vector towards the target direction by one step
                Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);

                // Draw a ray pointing at our target in
                Debug.DrawRay(transform.position, newDirection, Color.red);
                Debug.Log(Camera.main.ScreenToViewportPoint(Input.mousePosition));
                // rb.AddTorque(transform.up * );

                // Calculate a rotation a step closer to the target and applies rotation to this object
                //transform.rotation = Quaternion.LookRotation(newDirection);
             }
         }
        if(Input.GetKey(KeyCode.W)){
            rb.AddForce(transform.forward * clickForce);
        }
        if(Input.GetKey(KeyCode.D)){
            rb.AddTorque(transform.up * clickForce*2);
        }
        if(Input.GetKey(KeyCode.A)){
            rb.AddTorque(-transform.up * clickForce*2);
        }
        if (Input.GetKeyDown(KeyCode.Escape)){
            Application.Quit();
        }
      }


}

