using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float mouse_sens = 100f;
    float x_rotation = 0f;

    [SerializeField] private int hp = 100;
    public int HP
    {
        get { return hp; }
        set { hp = value; }
    }

    bool hasKey = false;
    bool hasTorch = false;
    [SerializeField] bool hasSword = false;

    Camera cam;
    Camera sword_cam;
    [SerializeField] float regularFOV = 60;
    [SerializeField] float zoomedFOV = 40;

    [SerializeField] LayerMask mask;
    [SerializeField] GameObject sword;
    [SerializeField] GameObject torch; 
    public float interact_distance = 2.0f;
    UnityEngine.UI.Text infoText;
    UnityEngine.UI.Text interactText;
    GameObject obj;

    [SerializeField] float attackDelay = 0.5f;
    float timeSinceAttack = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        //infoText = GameObject.Find("InformationText").GetComponent<UnityEngine.UI.Text>();
        //interactText = GameObject.Find("InteractText").GetComponent<UnityEngine.UI.Text>();
        cam = gameObject.GetComponent<Camera>();
        sword_cam = gameObject.GetComponentsInChildren<Camera>()[1];
        //Check if player has already picked up items and equip
        if(hasSword)
            equipItem(sword, new Vector3(0.495999992f, -0.131000042f, 0.887000084f), new Quaternion(-0.682276845f, -0.69443506f, -0.147758752f, 0.17442967f));
    }

    void Update()
    {
        AdjustCamera();
        Zoom();
        CheckInteraction();
        CheckTorchStatus();
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
        timeSinceAttack += Time.deltaTime;
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, interact_distance, mask))
        {
            obj = hitInfo.collider.gameObject;
            Debug.Log("In range of " + obj.name);
            //interactText.text = obj.name + "\nPress 'E' to interact";
            if (Input.GetKeyDown("e"))
            {
                if (obj.tag == "Key")
                {
                    Destroy(obj);
                    hasKey = true;
                    Debug.Log("Key picked up");
                    //infoText.text = "Key obtained";
                }
                //else if (obj.tag == "Switch")
                //{
                //    Animator anim = obj.GetComponent<Animator>();
                //    anim.SetBool("isOn", !anim.GetBool("isOn"));
                //    infoText.text = "Switch is " + (anim.GetBool("isOn") ? "ON" : "OFF");
                //}
                else if (obj.name == "Torch")
                {
                    Destroy(obj);
                    hasTorch = true;
                    equipItem(torch, new Vector3(-0.36500001f, -0.324000001f, 0.493000001f), Quaternion.identity);
                    Debug.Log("Torch picked up, Press 'F' to toggle the torch on and off");
                    //infoText.text = "Torch picked up\nPress 'F' to toggle the torch on and off";
                }
                else if (obj.name == "Sword")
                {
                    Destroy(obj);
                    hasSword = true;
                    equipItem(sword, new Vector3(0.495999992f, -0.131000042f, 0.887000084f), new Quaternion(-0.682276845f, -0.69443506f, -0.147758752f, 0.17442967f));
                    Debug.Log("Sword picked up\nPress Left mouse button to attack");
                    //infoText.text = "Sword picked up\nPress Left mouse button to attack";
                }
                //else if (obj.tag == "LockedDoor")
                //{
                //    if (hasKey)
                //    {
                //       obj.tag = "UnlockedDoor";
                //        infoText.text = "You unlocked the door with the key";
                //    }
                //    else
                //    {
                //        infoText.text = "Door is locked";
                //    }
                //}
                //else if (obj.tag == "UnlockedDoor")
                //{
                //    if (obj.GetComponent<Door>().isOpen)
                //    {
                //        infoText.text = "Door closed";
                //        obj.transform.Rotate(0, 0, 90);
                //        obj.GetComponent<Door>().isOpen = false;
                //    }
                //    else
                //    {
                //        infoText.text = "Door opened";
                //        obj.transform.Rotate(0, 0, -90);
                //        obj.GetComponent<Door>().isOpen = true;
                //    }
                //} 
            }
            //Combat Stuff
            else if (hasSword && timeSinceAttack >= attackDelay && Input.GetMouseButton(0) && obj.tag == "Enemy")
            {
                obj.GetComponent<Enemy>().HP -= 10;
                timeSinceAttack = 0f;
            }
        }
        else
        {
            //interactText.text = "";
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

    void equipItem(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        GameObject equippedItem = Instantiate(prefab, transform);
        equippedItem.transform.localPosition = position;
        equippedItem.transform.localRotation = rotation;
        SetLayerRecursively(equippedItem, 6);
    }
}
