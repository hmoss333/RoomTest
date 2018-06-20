using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractGiveItem : InteractParent {

    public GameObject itemToGive;

    [Range(0f, 1f)]
    public float chanceToDropKey;

    bool haveChecked = false;

    public override void Start()
    {
        base.Start();
        chanceToDropKey = Random.value;
    }

    public override void Interact()
    {
        GiveItem();
        base.Interact();
    }

    public void GiveItem()
    {
        if (!GameManager.foundKey) //GameManager.step == 3)
        {
            float randNum = Random.value;

            if (randNum < chanceToDropKey && !haveChecked)
            {
                text = "I found a key in here!";
                GameManager.foundKey = true;
            }
            else
            {
                text = "Doesn't look like there's anything in this one";
            }

            haveChecked = true;
        }
    }
}
