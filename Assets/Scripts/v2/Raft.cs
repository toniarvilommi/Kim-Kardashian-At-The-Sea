using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raft : MonoBehaviour
{
    float defaultValue = 0f;
    GameObject Waves;
    GameObject Poiju;
    bool enterArea = false;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Saakeli"); 
        enterArea = true;
        //Random values for poiju
        xPos = Random.Range(-respawnRange, respawnRange);
        zPos = Random.Range(-respawnRange, respawnRange);

        Debug.Log("Random range is:" + xPos + " " + zPos);
    }


    private float xPos;
    private float zPos;

    public float respawnRange = 14.5f;
    private void Start()
    {
        


        Waves = GameObject.Find("Waves");
        Poiju = GameObject.Find("poju");
        defaultValue = Waves.GetComponent<Waves>().Octaves[0].height;
        Debug.Log("Default value =" + defaultValue);
    }

    public float changePerSecond = 0.9f;

    private void Update()
    {
        


        //Jos menee arealle niin meri rauhottuu graduaalisesti
        if (enterArea)
        {
            

            Poiju.transform.position = new Vector3(xPos, 0, zPos);

            Waves.GetComponent<Waves>().Octaves[0].height -= changePerSecond * Time.deltaTime ;
            
            //kun menee alle default valuen niin meri yltyy taas
            if (Waves.GetComponent<Waves>().Octaves[0].height < defaultValue)
            {
                enterArea = false;
            }


        }
        
    }

}
