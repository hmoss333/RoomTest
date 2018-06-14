using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainDoor : InteractParent{

    public string step0;
    public string step1;
    public string step2;
    public string step3;
    public string step4;

    public override void Interact()
    {
        //base.Interact();

        switch (GameManager.step)
        {
            case 1:
                Debug.Log(step0);
                TextController.textToDisplay = step0;
                break;
            case 2:
                Debug.Log(step1);
                TextController.textToDisplay = step1;
                break;
            case 3:
                Debug.Log(step2);
                TextController.textToDisplay = step2;
                break;
            case 4:
                Debug.Log(step3);
                TextController.textToDisplay = step3;
                break;
            case 5:
                Debug.Log(step4);
                TextController.textToDisplay = step4;
                break;
            default:
                Debug.Log("Something went wrong");
                break;
        }

        tc.DisplayText();
    }
}
