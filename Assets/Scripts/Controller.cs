using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Controller : MonoBehaviour
{
    // Errors
    //private bool error30degrees; 2
    //private bool errorNoClip; 2
    //private bool errorInstantTurn; 2
    //private bool errorGameOver; 3
    //private bool errorSpawnOut; 3
    //private bool errorNoSound; 3
    //private bool errorCameraModeChange 3;
    //private bool errorBadCamSmooth; 3 4
    //private bool errorNoAnimation; 4
    //private bool errorBadMovement; 4
    //private bool errrorSameImage; 4

    public static Controller main;
    // Stage
    public static int stageIndex = 1;

    // Character
    public PlayerMovement character;

    // Dad
    public Transform dad;


    // Turning 
    private KeyCode turnLeftKey;
    private KeyCode turnRightKey;
    private float turning;
    private float turnAngle;

    [SerializeField]
    private float turnAnglePS;

    [SerializeField]
    public List<Transform> transforms2D;
    [SerializeField]
    public List<Rigidbody> rigidbodies2D;

    // Item
    [SerializeField]
    public List<Transform> items;
    [SerializeField]
    private float itemJumpFreq;
    private float itemJumpTimer;

    // Scroll UI
    [SerializeField]
    private GameObject scrollUI;
    [SerializeField]
    private Text scrollContext;

    // Information UI
    [SerializeField]
    private Text information;
    private float informationTimer;

    // Night Sign UI
    [SerializeField]
    private Image nightSignUI;

    [SerializeField]
    private Text nightSignText;
    private float nightSignTimer;
    private bool canSleep;

    // Restart UI
    [SerializeField]
    private GameObject restartUI;


    // Prefabs
    [SerializeField]
    private GameObject maze;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject father;
    [SerializeField]
    private GameObject key;
    [SerializeField]
    private GameObject scroll;
    [SerializeField]
    private GameObject wall;
    [SerializeField]
    private GameObject door;

    [SerializeField]
    private Color wallColor;
    [SerializeField]
    private Sprite wallSprite;

    // Scroll Contexts
    private string[][] scrolls;

    [SerializeField]
    private string[] stage0Scrolls;
    [SerializeField]
    private string[] stage1Scrolls;
    [SerializeField]
    private string[] stage2Scrolls;
    [SerializeField]
    private string[] stage3Scrolls;

    // Static UI
    public RawImage staticUI;

    // Sound
    [SerializeField]
    public SoundController soundController;
    public AudioClip takeKeySound;
    public AudioClip takePaperSound;

    private void Start()
    {
        main = this;

        restartUI.SetActive(false);
        turnLeftKey = KeyCode.Q;
        turnRightKey = KeyCode.E;
        turnAngle = 90f;

        transforms2D = new List<Transform>();
        rigidbodies2D = new List<Rigidbody>();
        items = new List<Transform>();

        turning = 0f;
        itemJumpTimer = 0f;
        informationTimer = 0f;
        scrolls = new string[4][];
        scrolls[0] = stage0Scrolls;
        scrolls[1] = stage1Scrolls;
        scrolls[2] = stage2Scrolls;
        scrolls[3] = stage3Scrolls;

        nightSignTimer = 3f;
        nightSignText.text = "Night " + stageIndex;

        canSleep = false;

        SetMaze();

        dad = Instantiate(father).transform;
        transforms2D.Add(dad);

        dad.position = new Vector3(8, -4, -80);
        dad.GetComponent<Dad>().Set(character.transform, this);



        //Stage 2
        if(stageIndex == 1)
        {
            OpenInfo("Movement: A - left, D - right, Q - turn left, E - turn right. And he is behind you.", 10f);
        }
        if(stageIndex == 2)
        {
            turnAngle = 45f;
        }
        if(stageIndex == 3)
        {
            nightSignTimer = 0f;
            nightSignUI.color = new Color(0f, 0f, 0f, .1f);
            nightSignText.color = new Color(1, 1, 1, .1f);
            character.transform.position = new Vector3(8.5f, 0.5f, -30.5f);
            SoundController.current.musicSound.enabled = false;
        }
    }

    private void Update()
    {
        if (Input.GetKey(turnRightKey) || Input.GetKey(turnLeftKey))
            TurnInputs();
        else if (turning != 0f)
            Turning();

        JumpingItems();

        if (informationTimer > 0f)
            InfoFade();

        if (scrollUI.active == true && Input.GetKeyDown(KeyCode.Space))
            CloseScroll();

        if (nightSignTimer > 0f)
            ShowNightSign();

        if (canSleep && Input.GetKeyDown(KeyCode.Space))
            NextNight();
    }

    // TURN MECHANIC
    private void TurnInputs()
    {
        if (Input.GetKeyDown(turnLeftKey))
        {
            character.Turn(-1);
            turning += turnAngle;
        }
        else if (Input.GetKeyDown(turnRightKey))
        {
            character.Turn(1);
            turning -= turnAngle;
        }


        //foreach (Rigidbody rb in rigidbodies2D)
        //    rb.velocity = Vector3.zero;
    }

    private void Turning()
    {
        // Stage 2
        if(stageIndex == 2 && turning != 0f)
        {
            float turn = -turning;
            foreach (Transform t in transforms2D)
                t.Rotate(Vector3.up * turn);
            turning = 0f;
        }


        else if (turning > .1f)
        {
            float turn = -turnAnglePS * Time.deltaTime;
            turning += turn;

            foreach(Transform t in transforms2D)
                t.Rotate(Vector3.up * turn);
        }
        else if (turning < -.1f)
        {
            float turn = turnAnglePS * Time.deltaTime;
            turning += turn;

            foreach (Transform t in transforms2D)
                t.Rotate(Vector3.up * turn);
        }
        else
        {
            float turn = -turning;
            foreach (Transform t in transforms2D)
                t.Rotate(Vector3.up * turn);
            turning = 0f;
        }
    }

    // ITEM JUMPING
    private void JumpingItems()
    {
        foreach(Transform t in items)
        {
            itemJumpTimer += Time.deltaTime;
            if (itemJumpTimer >= 2 / itemJumpFreq)
                itemJumpTimer -= 2 / itemJumpFreq;
            t.transform.position += .0005f * Mathf.Cos(itemJumpFreq * itemJumpTimer * Mathf.PI) * Vector3.up;
        }
    }

    // SCROLL UI
    public void OpenScroll(string text)
    {
        scrollContext.text = text;
        scrollUI.SetActive(true);
    }

    public void CloseScroll()
    {
        scrollUI.SetActive(false);
    }


    // INFO UI
    public void OpenInfo(string text, float timer)
    {
        information.enabled = true;
        information.text = text;
        informationTimer = timer;
        information.color = Color.white;
    }

    public void CloseInfo()
    {
        if(information != null)
            information.enabled = false;
        informationTimer = 0f;
    }

    private void InfoFade()
    {
        informationTimer -= Time.deltaTime;

        if (informationTimer <= 0f)
            CloseInfo();
        else
            information.color = new Color(1, 1, 1, Mathf.Min(1, informationTimer));

    }

    // MAZE
    private void SetMaze()
    {
        Transform walls = Instantiate(maze, -Vector3.up * 5f, transform.rotation, null).transform;

        for (int k = 0; k < walls.childCount; ++k)
        {
            Transform wallParent = walls.GetChild(k);

            for (int i = 0; i < wallParent.childCount; ++i)
            {
                Transform child = wallParent.GetChild(i);

                if (child.GetComponent<SpriteRenderer>())
                    SetWallCollider(child.GetComponent<SpriteRenderer>());

                for (int j = 0; j < child.childCount; ++j)
                {
                    if (child.GetChild(j).GetComponent<SpriteRenderer>())
                        SetWallCollider(child.GetChild(j).GetComponent<SpriteRenderer>());
                }
            }
        }

        Set3Doors();

        SetPlayer();
        SetItems();
        SetDoor();
    }

    private void SetWallCollider(SpriteRenderer wall)
    {
        wall.sprite = wallSprite;
        wall.color = wallColor;
        if (stageIndex == 4)
            wall.color = Color.red;



        if (stageIndex != 2)
        {
            BoxCollider collider = wall.gameObject.AddComponent<BoxCollider>();
            collider.size = (Vector3)wall.size + Vector3.forward * .2f;
        }

        
    }

    private void Set3Doors()
    {
        int index = Random.Range(0, 3);

        GameObject obj = Instantiate(wall);
        obj.transform.position = new Vector3(8, -5f, 14.5f);
        obj.transform.Rotate(Vector3.up * 90);
        SetWallCollider(obj.GetComponent<SpriteRenderer>());

        GameObject obj2 = Instantiate(wall);
        obj2.transform.position = new Vector3(8.5f, -5f, 14);
        SetWallCollider(obj2.GetComponent<SpriteRenderer>());

        GameObject obj3 = Instantiate(wall);
        obj3.transform.position = new Vector3(9, -5f, 14.5f);
        obj3.transform.Rotate(Vector3.up * 90);
        SetWallCollider(obj3.GetComponent<SpriteRenderer>());

        switch (index)
        {
            case 0:
                Destroy(obj);
                break;
            case 1:
                Destroy(obj2);
                break;
            case 2:
                Destroy(obj3);
                break;
            default:
                Destroy(obj);
                break;
        }
    }

    private void SetItems()
    {
        int keyIndex = Random.Range(0, 9);
        for(int i = 0; i < 9; ++i)
        {
            int sectionX = i % 3;
            int sectionZ = i / 3;
            GameObject obj;
            int x = Random.Range(0, 6);
            int z = Random.Range(0, 5);
            if (i != keyIndex)
                obj = Instantiate(scroll);
            else
                obj = Instantiate(key);
            obj.transform.position = new Vector3(.5f + x + sectionX * 6, -5.1f, .5f + z + sectionZ * 5);
            if (i != keyIndex)
                obj.GetComponent<Item>().Set(this, true, character, scrolls[stageIndex - 1][i]);
            else
                obj.GetComponent<Item>().Set(this, false, character, "");
            transforms2D.Add(obj.transform);
            items.Add(obj.transform);
        }

    }

    private void SetPlayer()
    {
        character = Instantiate(player).GetComponent<PlayerMovement>();
        character.transform.position = new Vector3(8.5f, -5.5f, -0.5f);

        transforms2D.Add(character.transform);
        rigidbodies2D.Add(character.GetComponent<Rigidbody>());
    }

    private void SetDoor()
    {
        GameObject obj = Instantiate(door);
        obj.transform.position = new Vector3(8.5f, -5f, 15f);
        obj.GetComponent<Door>().Set(this);
    }


    // Night Methods
    private void ShowNightSign()
    {
        nightSignTimer -= Time.deltaTime;

        nightSignUI.color = new Color(0f, 0f, 0f, Mathf.Min(1, nightSignTimer));
        nightSignText.color = new Color(1, 1, 1, Mathf.Min(1, nightSignTimer));

        if (nightSignTimer <= 0.1f)
        {
            Destroy(nightSignUI.gameObject);
            nightSignTimer = 0f;
        }
            
    }

    public void Restart()
    {
        Debug.Log("restarted condution");
        character.enabled = false;
        restartUI.SetActive(true);
        stageIndex = 0;
    }

    public void NextNight()
    {

        Debug.Log("next night");
        stageIndex++;

        if(stageIndex < 5)
            SceneManager.LoadScene(1);
        else
            SceneManager.LoadScene(2);
    }

    public void CanSleep()
    {
        OpenInfo("Door is locked. Press space to sleep.", 0f);
        canSleep = true;
    }
}


