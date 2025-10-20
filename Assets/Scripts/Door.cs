using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
    private Controller controller;
    private AudioSource knockSource;
    private Transform player;
    private bool canOpen;
    public void Set(Controller controller)
    {
        this.controller = controller;
        knockSource = controller.soundController.doorSound;
        player = controller.character.transform;
        canOpen = false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 3)
        {
            if(collision.gameObject.GetComponent<PlayerMovement>().gotKey)
            {
                controller.OpenInfo("Press space to lock the door.", 0f);
                canOpen = true;
            }
            else
            {
                controller.OpenInfo("You need to find the key to lock the door.", 0f);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == 3)
        {
            controller.CloseInfo();
            canOpen = false;
        }
    }

    private void Update()
    {
        if (canOpen && Input.GetKeyDown(KeyCode.Space))
        {
            OnDestroy();
            controller.CanSleep();
            canOpen = false;
        }
        Knock();


    }

    private void OnDestroy()
    {
        controller.CloseInfo();
    }

    private void Knock()
    {
        float distance = (player.position - transform.position).magnitude;
        float volume = (10f - distance) * .1f;
        if(volume > 0f)
        {
            volume *= volume;
            knockSource.volume = volume;
        }
    }
}
