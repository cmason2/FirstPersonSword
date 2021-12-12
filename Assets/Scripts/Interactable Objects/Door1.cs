using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door1 : MonoBehaviour
{
    public bool isLocked = false;
    public bool isOpen = false;
    public bool isMoving = false;

    Animation anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animation>();
    }


    public void DoorOpen(bool open) 
    {
        isMoving = true;
        if (!open)
        {
            isMoving = true;
            anim.Play();
            
            isMoving = false;
            isOpen = !isOpen;
        }
    }
}
