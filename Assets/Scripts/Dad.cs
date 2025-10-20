using UnityEngine;
using System.Collections;

public class Dad : MonoBehaviour
{
    private Transform player;
    private Controller controller;
    private StaticEffectController staticController;

    [SerializeField]
    private float speed;

    public void Set(Transform player, Controller controller)
    {
        this.player = player;
        this.controller = controller;

        staticController = GetComponent<StaticEffectController>();
        staticController.Set(player, controller);
    }

    private void Update()
    {
        Vector3 add = (player.position - transform.position).normalized * speed * Time.deltaTime + transform.position;
        transform.position = add;
        //transform.position = new Vector3(transform.position.x, 1, transform.position.z);

        if(!staticController.enabled && (transform.position - player.position).magnitude <= 20f)
            staticController.enabled = true;
        else if(staticController.enabled && (transform.position - player.position).magnitude > 20f)
            staticController.enabled = false;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            controller.Restart();
            enabled = false;
        }
    }
}
