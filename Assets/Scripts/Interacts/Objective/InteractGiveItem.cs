using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractGiveItem : InteractParent {

    public GameObject itemToGive;

    [Range(0f, 1f)]
    public float chanceToDropKey;

    private void Start()
    {

    }

    public override void Interact()
    {
        base.Interact();
        //DisplayText();

        GiveItem(itemToGive);
    }

    public void GiveItem(GameObject item)
    {
        if (GameManager.step == 2)
        {
            float randNum = Random.value;

            if (randNum < chanceToDropKey)
            {
                Debug.Log("Found " + item.name + ". Maybe this will come in handy.");
                GameManager.UpdateStep();
            }
        }
    }
}
