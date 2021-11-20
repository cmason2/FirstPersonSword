using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
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
    bool hasSword = false;

    Camera cam;
    Camera sword_cam;
    [SerializeField] float regularFOV = 60;
    [SerializeField] float zoomedFOV = 40;

    [SerializeField] LayerMask mask;
    public float interact_distance = 2.0f;
    UnityEngine.UI.Text infoText;
    UnityEngine.UI.Text interactText;
    GameObject obj;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        //infoText = GameObject.Find("InformationText").GetComponent<UnityEngine.UI.Text>();
        //interactText = GameObject.Find("InteractText").GetComponent<UnityEngine.UI.Text>();
        cam = gameObject.GetComponent<Camera>();
        sword_cam = gameObject.GetComponentsInChildren<Camera>()[1];
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
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, interact_distance, mask))
        {
            obj = hitInfo.collider.gameObject;
            //interactText.text = obj.name + "\nPress 'E' to interact";
            if (Input.GetKeyDown("e"))
            {
                if (obj.name == "Key")
                {
                    Destroy(obj);
                    hasKey = true;
                    //infoText.text = "Key obtained";
                }
                //else if (obj.tag == "Switch")
                //{
                //    Animator anim = obj.GetComponent<Animator>();
                //    anim.SetBool("isOn", !anim.GetBool("isOn"));
                //    infoText.text = "Switch is " + (anim.GetBool("isOn") ? "ON" : "OFF");
                //}
                //else if (obj.name == "Torch")
                //{
                //    obj.transform.SetParent(gameObject.transform);
                //    obj.transform.localPosition = new Vector3(-0.36500001f, -0.324000001f, 0.493000001f);
                //    obj.transform.localRotation = Quaternion.identity;
                //    SetLayerRecursively(obj, 6);
                //    hasTorch = true;
                //    infoText.text = "Torch picked up\nPress 'F' to toggle the torch on and off";
                //}
                //else if (obj.name == "Sword")
                //{
                //    obj.transform.SetParent(gameObject.transform);
                //    obj.transform.localPosition = new Vector3(0.495999992f, -0.131000042f, 0.887000084f);
                //    obj.transform.localRotation = new Quaternion(-0.682276845f, -0.69443506f, -0.147758752f, 0.17442967f);
                //    SetLayerRecursively(obj, 6);
                //    hasSword = true;
                //    infoText.text = "Sword picked up\nPress Left mouse button to attack";
                //}
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
}
