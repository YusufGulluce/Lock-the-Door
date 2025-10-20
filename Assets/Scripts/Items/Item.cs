using UnityEngine;
using System.Collections;

public abstract class Item : MonoBehaviour
{
    private PlayerMovement player;
    public Controller controller;
    private bool pressKey;
    public bool pressible;
    private string context;

    public void Set(Controller controller, bool pressKey, PlayerMovement player, string context)
    {
        this.controller = controller;
        this.pressKey = pressKey;
        this.player = player;
        this.context = context;
        pressible = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3) //Player
        {
            pressible = pressKey;
            PlayerEnter(player);

            if(Controller.stageIndex == 3)
            {
                Camera.main.orthographic = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 3) //Player
        {
            pressible = false;
            PlayerExit(player);

            if (Controller.stageIndex == 3)
            {
                Camera.main.orthographic = false;
            }
        }
    }

    private void Update()
    {
        if (pressible && Input.GetKeyDown(KeyCode.Space))
            Pressed(player, context);
    }

    public void OnDestroy()
    {
        if (Controller.stageIndex == 3)
        {
            if(Camera.main != null)
                Camera.main.orthographic = false;
        }
        if (controller.items.Contains(transform))
            controller.items.Remove(transform);
        if (controller.transforms2D.Contains(transform))
            controller.transforms2D.Remove(transform);
    }

    public abstract void Pressed(PlayerMovement player, string context);
    public abstract void PlayerEnter(PlayerMovement player);
    public abstract void PlayerExit(PlayerMovement player);
}
