using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckUpsidedown : MonoBehaviour
{
    [SerializeField]
    public GameObject _kim;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Dot(transform.up, Vector3.down) > 0)
        {
            Debug.Log("Destroy joint");
            Destroy(_kim.GetComponentInChildren<FixedJoint>());
        }
            
    }
}
