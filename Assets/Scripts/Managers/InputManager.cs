using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    public InputController inputController;

    private void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        inputController = new InputController();
        inputController.Player.Enable();
        inputController.UI.Enable();
    }
}
