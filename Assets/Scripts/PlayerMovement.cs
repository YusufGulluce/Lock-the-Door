using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private SpriteRenderer sr;
    private int direction = -1;

    [Header("Animation")]

    [Header("Idle")]
    [SerializeField]
    private Sprite[] idleSprites0;
    [SerializeField]
    private Sprite[] idleSprites1;
    [SerializeField]
    private Sprite[] idleSprites2;
    [SerializeField]
    private Sprite[] idleSprites3;
    private Sprite[][] idleSprites;
    private Sprite[] currIdleSprites;


    [Header("Run")]
    [SerializeField]
    private Sprite[] runSprites0;
    [SerializeField]
    private Sprite[] runSprites1;
    [SerializeField]
    private Sprite[] runSprites2;
    [SerializeField]
    private Sprite[] runSprites3;
    private Sprite[][] runSprites;
    private Sprite[] currRunSprites;

    [Header("Turn")]
    [SerializeField]
    private Sprite[] turnLeftSprites;
    [SerializeField]
    private Sprite[] turnRightSprites;

    [Header("Animation Speed FPS")]
    [SerializeField]
    private float idleAnimSpeed;    // frame per second
    [SerializeField]
    private float runAnimSpeed;
    [SerializeField]
    private float turnAnimSpeed;
    public int turn;
    private int frameIndex;
    private float frameTimer;
    private float frameSpeed; // frame per second


    [Header("Movement")]
    [SerializeField]
    private float runForce;

    [SerializeField]
    private float frictionConstant;

    private KeyCode rightKey;
    private KeyCode leftKey;


    public bool gotKey;


    private bool isBreathing;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        sr = GetComponent<SpriteRenderer>();
        rightKey = KeyCode.RightArrow;
        leftKey = KeyCode.LeftArrow;

        gotKey = false;
        isBreathing = false;

        frameIndex = 0;
        frameTimer = 0f;

        idleSprites = new Sprite[4][];
        idleSprites[0] = idleSprites0;
        idleSprites[1] = idleSprites1;
        idleSprites[2] = idleSprites2;
        idleSprites[3] = idleSprites3;

        runSprites = new Sprite[4][];
        runSprites[0] = runSprites0;
        runSprites[1] = runSprites1;
        runSprites[2] = runSprites2;
        runSprites[3] = runSprites3;
    }


    void Update()
    {
        if (Input.GetKey(rightKey) || Input.GetKey(leftKey))
            HorizontalMovement();

        if (rb.velocity.magnitude > 0.01f)
            Breath();
        else
            SoundController.current.breathSound.volume = 0f;

        if (transform.position.y <= -20f)
        {
            transform.position = new Vector3(8.5f, .5f, -.5f);
            rb.velocity = Vector3.zero;
            int random = Random.Range(0, 3);
            switch(random)
            {
                case 0:
                    Controller.main.OpenInfo("You can not run, just lock the door.", 2f);
                    break;
                case 1:
                    Controller.main.OpenInfo("Escape will not be an option.", 2f);
                    break;
                case 2:
                    Controller.main.OpenInfo("You do not know anything but here. Where are you going?", 2f);
                    break;
                default:
                    Controller.main.OpenInfo("Just contuniue.", 2f);
                    break;
            }
        }

        if (Controller.stageIndex != 4)
            UpdateFrame();
    }

    
    private void HorizontalMovement()
    {

        int dir = 0;
        if (Input.GetKey(rightKey))
            dir++;
        else if (Input.GetKey(leftKey))
            dir--;

        if(direction != dir)
        {
            direction = dir;
            transform.localScale = new Vector3(-dir, 1, 1);
        }

        if (Controller.stageIndex != 4)
        {
            if (dir != 0 && !isBreathing)
            {
                isBreathing = true;
            }

            float currentSpeed = Vector3.Dot(transform.right, rb.velocity);
            float currForce = dir * runForce;
            currForce -= currentSpeed * currentSpeed * frictionConstant * dir;
            rb.velocity += currForce * Time.deltaTime * transform.right;
        }
        else
            rb.velocity = 5f * dir * transform.right;

    }
    private void Look(int dir)
    {

    }

    private void Breath()
    {
        SoundController.current.breathSound.volume = Mathf.Min(rb.velocity.magnitude, 1f) * .05f;
    }

    private void UpdateFrame()
    {
        frameTimer -= Time.deltaTime * frameSpeed;
        if (frameTimer <= 0f)
        {
            LookDirection();
            frameTimer = 1f;


            if (rb.velocity.magnitude > 0.03f)
            {
                frameSpeed = runAnimSpeed * currRunSprites.Length;
                frameIndex %= currRunSprites.Length;
                sr.sprite = currRunSprites[frameIndex];
            }
            else if (turn > 0)
            {
                frameSpeed = turnAnimSpeed * turnRightSprites.Length;
                frameIndex %= turnRightSprites.Length;
                sr.sprite = turnRightSprites[frameIndex];

                if (frameIndex >= turnRightSprites.Length - 1)
                {
                    ResetFrame();
                    turn = 0;
                }

            }
            else if (turn < 0)
            {
                frameSpeed = turnAnimSpeed * turnLeftSprites.Length;
                frameIndex %= turnLeftSprites.Length;
                sr.sprite = turnLeftSprites[frameIndex];

                if (frameIndex >= turnLeftSprites.Length - 1)
                {
                    ResetFrame();
                    turn = 0;
                }
            }
            else
            {
                frameSpeed = idleAnimSpeed * currIdleSprites.Length;
                frameIndex %= currIdleSprites.Length;
                sr.sprite = currIdleSprites[frameIndex];
            }

            frameIndex++;
        }
    }

    private void LookDirection()
    {
        int lookCase;   // 0 - düz, 1 - içeri, 2 - geri, 3 - dışarı
        Vector3 dadDir = Controller.main.dad.position - transform.position;

        float dotProduct = Vector3.Dot(transform.right, dadDir);
        if (dotProduct >= 0f)
            lookCase = 2;
        else
        {
            lookCase = 0;
            dotProduct *= -1;
        }

        float dP = Vector3.Dot(transform.forward, dadDir);
        if (dP > dotProduct)
            lookCase = 1;
        else if (-dP > dotProduct)
            lookCase = 3;

        currIdleSprites = idleSprites[lookCase];
        currRunSprites = runSprites[lookCase];
    }

    private void ResetFrame()
    {
        frameIndex = 0;
    }

    private void ImmidiateFrame()
    {
        frameTimer = 0f;
    }

    public void Turn(int turn)
    {

        if (direction != turn)
        {
            direction = turn;
            transform.localScale = new Vector3(-turn, 1, 1);
        }

        this.turn = turn;
        ResetFrame();
        ImmidiateFrame();
    }


}

