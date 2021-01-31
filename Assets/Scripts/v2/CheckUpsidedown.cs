using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckUpsidedown : MonoBehaviour
{
    [SerializeField]
    GameObject _kim;
    [SerializeField]
    GameObject boat;
    [SerializeField]
    GameObject Canvas;
    // Start is called before the first frame update
    void Start()
    {
        Canvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Dot(transform.up, Vector3.down) > -0.3f)
        {
            Debug.Log("Destroy joint");
            Destroy(_kim.GetComponentInChildren<FixedJoint>());
            Canvas.SetActive(true);

        }
        var checkJoint = _kim.GetComponentInChildren<FixedJoint>();
        if (!checkJoint)
        {
            Canvas.SetActive(true);

            if (Input.GetKeyDown("space"))
            {
                SceneManager.LoadScene("Menu", LoadSceneMode.Single);
            }


        }

    }
}
