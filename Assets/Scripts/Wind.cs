using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    private Vector3 targetPosition;
    private Plane plane;
    public float moveSpeed = 3f;
    public float turnSpeed = 3f;
    public float angleLimit = 5f;
    public float height = 1f;
    public bool waitForRotation = true;
    private void Start()
    {
        targetPosition = transform.position;
        plane = new Plane(Vector3.up, new Vector3(0, height, 0));
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (plane.Raycast(ray, out float distance))
            {
                targetPosition = ray.GetPoint(distance);
            }
        }
        if (targetPosition != transform.position)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetPosition - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
            if (!waitForRotation || Quaternion.Angle(targetRotation, transform.rotation) < angleLimit)
            {
                float distanceToDestination = Vector3.Distance(transform.position, targetPosition);
                if (distanceToDestination > (Time.deltaTime * moveSpeed))
                {
                    transform.position += (targetPosition - transform.position).normalized * moveSpeed * Time.deltaTime;
                }
                else
                {
                    transform.position = targetPosition;
                }
            }
        }
    }


}
