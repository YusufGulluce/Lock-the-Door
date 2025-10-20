using UnityEngine;
using System.Collections;

public class Scroll : Item
{
    public override void PlayerEnter(PlayerMovement player)
    {
        controller.OpenInfo("Press space to read it.", 0f);
    }

    public override void PlayerExit(PlayerMovement player)
    {
        controller.CloseInfo();
    }

    public override void Pressed(PlayerMovement player, string context)
    {
        AudioSource.PlayClipAtPoint(controller.takePaperSound, transform.position + Vector3.down * 2f);
        controller.OpenScroll(context);
        controller.CloseInfo();
        Destroy(gameObject);
    }

}
