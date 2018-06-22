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
        //Placeholder; interacting with the main door should load the outdoors area
        switch (GameManager.step)
        {
            case 1:
                tc.DisplayText(step0);
                break;
            case 2:
                tc.DisplayText(step1);
                break;
            case 3:
                GameManager.UpdateStep();
                //tc.DisplayText(step2);
                break;
            case 4:
                //tc.DisplayText(step3);
                break;
            case 5:
                tc.DisplayText(step4);
                break;
            default:
                Debug.Log("Something went wrong");
                break;
        }
    }
}
