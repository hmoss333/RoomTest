using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractGiveItem : InteractParent {

    public GameObject itemToGive;

    [Range(0f, 1f)]
    public float chanceToDropKey;

    public override void Interact()
    {
        base.Interact();

        GiveItem();
    }

    public void GiveItem()
    {
        if (GameManager.step == 3)
        {
            float randNum = Random.value;

            if (randNum < chanceToDropKey)
            {
                Debug.Log("Found objective item");
                GameManager.UpdateStep();
            }
        }
    }
}
