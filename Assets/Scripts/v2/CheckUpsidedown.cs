using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckUpsidedown : MonoBehaviour
{
    [SerializeField]
    GameObject _kim;
    [SerializeField]
    GameObject boat;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Vector3.Dot(transform.up, Vector3.down));
        if (Vector3.Dot(transform.up, Vector3.down) > -0.3f)
        {
            Debug.Log("Destroy joint");
            Destroy(_kim.GetComponentInChildren<FixedJoint>());
        }
            
    }
}
