using UnityEngine;

public class lightFlicker : MonoBehaviour
{
   
    
    //maybe keep a track of how long it should take until the light goes off
    private float flickerWait = 3;
    
    //the flicker should only be for a split second
    private float flickerTime = 0.8f;
    
    //need an interval between the flicker
    private float flickerInterval;
    
    //keep a timer for it
    private float flickerTimer;
    
    //have a boolean to dim the light
    private bool dimLight;
    
    
    public Light myLight;

    private void Update()
    {
        if (dimLight)
        {
            myLight.intensity = 2;
        }
        else
        {
            myLight.intensity = 5;
        }
        flickerTimer += Time.deltaTime;
        //start the clock to get the flicker going
        if (flickerTimer > flickerInterval)
        {
            flickerLight();
        }

        

    }

 
    
//method to actually flicker the light
    void flickerLight()
    {
        dimLight = !dimLight;
        //when the light is enabled, start the clock
        if (dimLight == false)
        {
            flickerInterval = Random.Range(0, flickerWait);
        }

        else
        {
            //start to flicker a light for a split second
            flickerInterval = Random.Range(0, flickerTime);
        }

        flickerTimer = 0;
    }
    
}
