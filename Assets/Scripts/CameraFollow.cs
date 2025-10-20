using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    public float smooth;

    private Rigidbody rbFollow;
    private Vector3 offset;
    private Vector3 startOffset;
    // Start is called before the first frame update
    void Start()
    {
        rbFollow = GetComponentInParent<Rigidbody>();
        offset = Vector3.zero;
        startOffset = transform.localPosition;

        if (Controller.stageIndex == 3 || Controller.stageIndex == 4)
            smooth = 1000f;

        if (Controller.stageIndex == 4)
            GetComponent<Camera>().backgroundColor = new Color32(80,20,20,255);
    }

    // Update is called once per frame
    void Update()
    {
        //offset = startOffset - Vector3.Lerp(Vector3.zero, rbFollow.velocity, .5f) * smooth;
        transform.position -= rbFollow.velocity * smooth * Time.deltaTime; 
        transform.localPosition = Vector3.Lerp(startOffset, transform.localPosition, .01f);
    }
}
