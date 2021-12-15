using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float mouse_sens = 100f;
    float x_rotation = 0f;

    GameObject fadeOverlay;

    //Player health
    [SerializeField] private int hp = 100;
    public int HP
    {
        get { return hp; }
        set { hp = value; }
    }
    HealthBar playerHealthBar;

    //Combat Stuff
    [SerializeField] LayerMask enemyLayer;
    public bool isInvulnerable = false;
    [SerializeField] float invulnerabilityTime = 1.5f;
    public float timeSinceDamageTaken = 0f;
    [SerializeField] float attackDelay = 0.75f;
    float timeSinceAttack = 0f;
    float attackDistance = 1f;
    GameObject targetedEnemy;
    Enemy targetedEnemyScript;
    Animator swordAnimator;

    //Inventory
    [SerializeField] bool hasSword = false;
    int numKeys = 0;
    public Image[] keysUI;
    bool hasTorch = false;
    [SerializeField] GameObject sword;
    [SerializeField] GameObject torch;

    Camera cam;
    Camera sword_cam;
    [SerializeField] float regularFOV = 60;
    [SerializeField] float zoomedFOV = 40;

    GameObject heldObject = null;

    //Object interaction
    [SerializeField] LayerMask interactLayer;
    public float interactDistance = 2.0f;
    TextMeshProUGUI infoText;
    TextMeshProUGUI interactText;
    TextMeshProUGUI objectiveText;
    [SerializeField] float textDisplayTime = 3f;
    [SerializeField] float textFadeTime = 1f;
    GameObject informationTextObject;
    GameObject targetedObject;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        playerHealthBar = GameObject.Find("Health Bar").GetComponent<HealthBar>();
        playerHealthBar.SetMaxHealth(hp);
        UpdateKeyUI();
        informationTextObject = GameObject.Find("InformationText");
        infoText = informationTextObject.GetComponent<TextMeshProUGUI>();
        infoText.text = "";
        interactText = GameObject.Find("InteractionText").GetComponent<TextMeshProUGUI>();
        objectiveText = GameObject.Find("Objective").GetComponent<TextMeshProUGUI>();
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        if (sceneName == "Maze")
        {
            objectiveText.text = "Pick up the torch\nCollect 3 keys\nUnlock the exit door";
        }
        else if(sceneName == "spike")
        {
            objectiveText.text = "Pick up the sword\nActivate the switch";
        }
        else if (sceneName == "Boss")
        {
            objectiveText.text = "Defeat the boss";
        }
        cam = gameObject.GetComponent<Camera>();
        sword_cam = gameObject.GetComponentsInChildren<Camera>()[1];
        enemyLayer = LayerMask.GetMask("Enemy");
        interactLayer = LayerMask.GetMask("Interactable Objects");
        timeSinceDamageTaken = invulnerabilityTime;
        timeSinceAttack = attackDelay;
        //Check if player has already picked up items and equip
        if (hasSword)
        {
            EquipItem(sword, new Vector3(0.495999992f, -0.131000042f, 0.887000084f), new Quaternion(-0.682276845f, -0.69443506f, -0.147758752f, 0.17442967f));
            swordAnimator = GetComponentInChildren<Animator>();
        }
        if (hasTorch)
        {
            EquipItem(torch, new Vector3(-0.36500001f, -0.324000001f, 0.493000001f), Quaternion.identity);
        }
        fadeOverlay = GameObject.Find("FadeOverlay");
        StartCoroutine(fadeOverlay.GetComponent<SceneFade>().Fade(true, 2f));
    }

    private void Update()
    {
        timeSinceDamageTaken += Time.deltaTime;
        if(timeSinceDamageTaken >= invulnerabilityTime)
        {
            //Can be attacked
            isInvulnerable = false;
        }
        else
        {
            isInvulnerable = true;
        }

        AdjustCamera();
        
        //Can't attack/interact when holding something
        if(heldObject == null)
        {
            CheckInteraction();
            Attack();
        }
        else
        {
            interactText.text = "Press 'F' to drop";
        }
        Zoom();
        CheckTorchStatus();
        DropItem();
    }

    private void FixedUpdate()
    {
        
    }

    void AdjustCamera()
    {
        float mouse_x = Input.GetAxis("Mouse X") * mouse_sens * Time.deltaTime;
        float mouse_y = Input.GetAxis("Mouse Y") * mouse_sens * Time.deltaTime;

        GameObject.FindWithTag("Player").transform.Rotate(Vector3.up * mouse_x);

        x_rotation -= mouse_y;
        x_rotation = Mathf.Clamp(x_rotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(x_rotation, 0f, 0f);
    }

    void CheckInteraction()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, interactDistance, interactLayer))
        {
            targetedObject = hitInfo.collider.gameObject;
            //Debug.Log("In range of " + targetedObject.name);
            interactText.text = targetedObject.name + "\nPress 'E' to interact";
            if (Input.GetKeyDown("e"))
            {
                if(targetedObject.tag == "chest")
                {
                    targetedObject.GetComponent<Animator>().SetTrigger("activate");
                    
                }
                else if (targetedObject.tag == "cupbord")
                {
                    targetedObject.GetComponent<Animator>().SetTrigger("opendoor");
                }
                else if (targetedObject.tag == "lever")
                {
                    targetedObject.GetComponentInChildren<Animator>().SetTrigger("pull");
                    targetedObject.GetComponentInChildren<Switch>().MoveSpikesDown();
                    Destroy(GameObject.Find("Blockexit").GetComponent<Spike_switch>());
                }
                else if (targetedObject.tag == "Key")
                {
                    Destroy(targetedObject);
                    numKeys++;
                    UpdateKeyUI();
                    StartCoroutine(informationTextObject.GetComponent<TextFadeInOut>().DisplayTextFade("Key picked up", textDisplayTime, textFadeTime));
                }
                else if (targetedObject.name == "Torch")
                {
                    Destroy(targetedObject);
                    hasTorch = true;
                    EquipItem(torch, new Vector3(-0.36500001f, -0.324000001f, 0.493000001f), Quaternion.identity);
                    StartCoroutine(informationTextObject.GetComponent<TextFadeInOut>().DisplayTextFade("Torch equipped, Press 'F' to toggle the torch on and off", textDisplayTime, textFadeTime));
                }
                else if (targetedObject.name == "Sword")
                {
                    Destroy(targetedObject);
                    hasSword = true;
                    EquipItem(sword, new Vector3(0.495999992f, -0.131000042f, 0.887000084f), new Quaternion(-0.682276845f, -0.69443506f, -0.147758752f, 0.17442967f));
                    swordAnimator = GetComponentInChildren<Animator>();
                    StartCoroutine(informationTextObject.GetComponent<TextFadeInOut>().DisplayTextFade("Sword equipped\nPress Left mouse button to attack", textDisplayTime, textFadeTime));
                }
                else if (targetedObject.tag == "Door")
                {
                    Door door = targetedObject.GetComponent<Door>();
                    if (door.isLocked)
                    {
                        if (numKeys > 0)
                        {
                            door.isLocked = false;
                            numKeys--;
                            UpdateKeyUI();
                            StartCoroutine(informationTextObject.GetComponent<TextFadeInOut>().DisplayTextFade("You unlocked the door with the key", textDisplayTime, textFadeTime));
                        }
                        else
                        {
                            StartCoroutine(informationTextObject.GetComponent<TextFadeInOut>().DisplayTextFade("Door is locked, find a key to unlock it", textDisplayTime, textFadeTime));
                        }
                    }
                    else if (!door.isMoving)
                    {
                        if(door.isOpen)
                        {
                            StartCoroutine(door.RotateDoor(false, 1f));
                        }
                        else
                        {
                            StartCoroutine(door.RotateDoor(true, 1f));
                        }
                    }
                }
                else if (targetedObject.tag == "Door1")
                {
                    Door1 door = targetedObject.GetComponent<Door1>();
                    if (door.isLocked)
                    {
                        if (numKeys == 3)
                        {
                            door.isLocked = false;
                            numKeys = 0;
                            UpdateKeyUI();
                            StartCoroutine(informationTextObject.GetComponent<TextFadeInOut>().DisplayTextFade("You unlocked the door with your keys", textDisplayTime, textFadeTime));
                        }
                        else
                        {
                            StartCoroutine(informationTextObject.GetComponent<TextFadeInOut>().DisplayTextFade("Door is locked, collect all three keys to unlock it", textDisplayTime, textFadeTime));
                        }
                    }
                    else if (!door.isMoving)
                    { 
                        int health = hp;
                        Debug.Log(health);
                        if(!door.isOpen)
                        {
                            door.DoorOpen(false);
                        }
                        StartCoroutine(fadeOverlay.GetComponent<SceneFade>().Fade(false, 1f));
                        Invoke("SceneChangeNext",1f);
                        //Debug.Log(hp);
                        
                        hp = health;
                    }
                }
                else if (targetedObject.name == "Dead Spider")
                {
                    HoldItem(targetedObject);
                    heldObject = targetedObject;
                }
            }            
        }
        else
        {
            interactText.text = "";
        }
    }

    void Attack()
    {
        timeSinceAttack += Time.deltaTime;
        if(Input.GetMouseButton(0) && hasSword && timeSinceAttack >= attackDelay)
        {
            swordAnimator.SetTrigger("click");
            timeSinceAttack = 0f;
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, attackDistance, enemyLayer))
            {
                targetedEnemy = hitInfo.collider.gameObject;
                targetedEnemyScript = targetedEnemy.GetComponent<Enemy>();
                if (!targetedEnemyScript.isInvulnerable)
                {
                    targetedEnemyScript.TakeDamage(10);
                    if (targetedEnemy.tag == "DeflectBall")
                    {
                        targetedEnemy.GetComponent<DeflectProjectile>().direction = transform.forward; //Deflect where the player is looking.
                    }
                }
            }
        }
        
    }
    void Zoom()
    {
        if (Input.GetMouseButton(1))
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, zoomedFOV, Time.deltaTime * 5);
            sword_cam.fieldOfView = Mathf.Lerp(sword_cam.fieldOfView, zoomedFOV, Time.deltaTime * 5); ;
        }
        else
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, regularFOV, Time.deltaTime * 5);
            sword_cam.fieldOfView = Mathf.Lerp(sword_cam.fieldOfView, regularFOV, Time.deltaTime * 5);
        }
    }
    public static void SetLayerRecursively(GameObject go, int layerNumber) //https://forum.unity.com/threads/change-gameobject-layer-at-run-time-wont-apply-to-child.10091/ post #5
    {
        foreach (Transform trans in go.GetComponentsInChildren<Transform>(true))
        {
            trans.gameObject.layer = layerNumber;
        }
    }
    void CheckTorchStatus()
    {
        if (hasTorch && Input.GetKeyDown("f"))
        {
            GetComponentInChildren<Light>().enabled = !GetComponentInChildren<Light>().enabled;
        }
    }

    void EquipItem(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        GameObject equippedItem = Instantiate(prefab, transform);
        equippedItem.transform.localPosition = position;
        equippedItem.transform.localRotation = rotation;
        if(equippedItem.GetComponent<BoxCollider>() != null)
        {
            Destroy(equippedItem.GetComponent<BoxCollider>()); //Remove colliders from objects once equipped
        }
        SetLayerRecursively(equippedItem, 6);
    }

    void HoldItem(GameObject item)
    {
        if (item.GetComponent<Rigidbody>())
            item.GetComponent<Rigidbody>().isKinematic = true; //Prevent forces and collisions
        item.transform.SetParent(transform);
        item.transform.localPosition = new Vector3(0f, -0f, 2f);
        SetLayerRecursively(item, 6);
    }

    void DropItem()
    {
        if (heldObject != null && Input.GetKeyDown("f"))
        {
            heldObject.transform.SetParent(null);
            heldObject.GetComponent<Rigidbody>().isKinematic = false;
            SetLayerRecursively(heldObject, 7);
            heldObject = null;
        }
    }

    void CheckHealth()
    {
        if(hp <= 0)
        {
            Debug.Log("Player is dead, what a shame!");
            GetComponentInParent<PlayerMovement>().enabled = false;
            StartCoroutine(fadeOverlay.GetComponent<SceneFade>().Fade(false, 1f));
            Invoke("SceneChangeGameOver", 1f);
        }
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        timeSinceDamageTaken = 0f;
        playerHealthBar.SetHealth(hp);
        CheckHealth();
    }

    private void UpdateKeyUI()
    {
        for(int i = 0; i < keysUI.Length; i++)
        {
            if(i < numKeys)
            {
                keysUI[i].enabled = true;
            }
            else
            {
                keysUI[i].enabled = false;
            }
        }
    }

    public void SceneChangeNext()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void SceneChangeGameOver()
    {
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("GameOver");
    }
}
