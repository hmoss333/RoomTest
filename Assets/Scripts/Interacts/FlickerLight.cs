using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickerLight : MonoBehaviour {

    Light lightObj;
    
    // Use this for initialization
	void Start () {
        lightObj = GetComponent<Light>();
        int randNum = Random.Range(0, 2);

        if (randNum == 0)
            StartCoroutine(Flicker(lightObj));
	}

    IEnumerator Flicker(Light lightToFlicker)
    {
        float randomTime = Random.Range(0, 3);

        if (lightToFlicker.enabled)
            lightToFlicker.enabled = false;
        else
            lightToFlicker.enabled = true;

        yield return new WaitForSeconds(randomTime);

        StartCoroutine(Flicker(lightToFlicker));
    }
}
