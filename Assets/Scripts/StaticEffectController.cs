using UnityEngine;
using System.Collections;

public class StaticEffectController : MonoBehaviour
{
    private Controller controller;
    private SoundController soundController;
    private Transform player;
    private float maxDistance;

    public void Set(Transform player, Controller controller)
    {
        this.player = player;
        this.controller = controller;
        this.soundController = controller.soundController;
    }

    private void OnEnable()
    {
        controller.staticUI.enabled = true;
        soundController.staticSound.enabled = true;

        maxDistance = (player.position - transform.position).magnitude;
        controller.staticUI.color = new Color(1f, 1f, 1f, 0f);
    }

    private void Update()
    {
        float distance = (player.position - transform.position).magnitude;
        float staticVolume = (maxDistance - distance) / maxDistance;
        staticVolume *= staticVolume;


        controller.staticUI.color = new Color(1f, 1f, 1f, staticVolume);
        soundController.staticSound.volume = staticVolume * .8f;
        soundController.musicSound.volume = (1f - staticVolume) * .5f;

    }

    private void OnDisable()
    {
        maxDistance = 128f;

        if(soundController.musicSound != null)
            soundController.musicSound.volume = 1f;

        if(controller.staticUI != null)
            controller.staticUI.enabled = false;

        if (soundController.staticSound != null)
            soundController.staticSound.enabled = false;
    }

}
