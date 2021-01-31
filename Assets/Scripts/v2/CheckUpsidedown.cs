using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CheckUpsidedown : MonoBehaviour
{
    [SerializeField]
    GameObject _kim;
    [SerializeField]
    GameObject boat;
    [SerializeField]
    GameObject Canvas;


    public GameObject scoreText;
    // Start is called before the first frame update
    void Start()
    {
        Canvas.SetActive(false);
    }

    public bool dead;

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
        if (!checkJoint && !dead)
        {
            Canvas.SetActive(true);
            dead = true;

            FindObjectOfType<AudioManager>().Play("Scream");
            scoreText.GetComponent<Text>().text = "SCORE: "+gameObject.GetComponent<Raft>().Score;

        }
        if (Input.GetKeyDown("space") && dead)
        {
            SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        }

    }
}
