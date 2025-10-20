using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Item
{
    public override void PlayerEnter(PlayerMovement player)
    {
        controller.OpenInfo("You have the key. Find the door.", 2.5f);
        player.gotKey = true;
        AudioSource.PlayClipAtPoint(controller.takeKeySound, transform.position + Vector3.down * 2f);
        Destroy(gameObject);
    }

    public override void PlayerExit(PlayerMovement player)
    {
    }

    public override void Pressed(PlayerMovement player, string context)
    {
    }
}
