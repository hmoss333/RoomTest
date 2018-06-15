using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractGiveItem : InteractParent {

    public GameObject itemToGive;

    [Range(0f, 1f)]
    public float chanceToDropKey;

    public override void Start()
    {
        base.Start();
        chanceToDropKey = Random.Range(0, 1.0f);
    }

    public override void Interact()
    {
        GiveItem();
        base.Interact();
    }

    public void GiveItem()
    {
        if (GameManager.step == 3)
        {
            float randNum = Random.value;

            if (randNum < chanceToDropKey) //Need to fix; player can just keep re-trying same object to find a key (or maybe keep it but make the chance very low)
            {
                text = "I found a key in here!";
                GameManager.UpdateStep();
            }
            else
            {
                text = "Doesn't look like there's anything in this one";
            }
        }
    }
}
