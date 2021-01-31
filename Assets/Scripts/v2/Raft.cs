using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raft : MonoBehaviour
{
    float defaultValue = 0f;
    GameObject Waves;
    bool enterArea = false;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Saakeli"); 
        enterArea = true;
    }

    private void Start()
    {

        Waves = GameObject.Find("Waves");
        defaultValue = Waves.GetComponent<Waves>().Octaves[0].height;
        Debug.Log("Default value =" + defaultValue);
    }

    public float changePerSecond = 0.9f;

    private void Update()
    {
        //Jos menee arealle niin meri rauhottuu graduaalisesti
        if (enterArea)
        {
            Waves.GetComponent<Waves>().Octaves[0].height -= changePerSecond * Time.deltaTime ;
            
            //kun menee alle default valuen niin meri yltyy taas
            if (Waves.GetComponent<Waves>().Octaves[0].height < defaultValue)
            {
                enterArea = false;
            }

        }
        
    }

}
