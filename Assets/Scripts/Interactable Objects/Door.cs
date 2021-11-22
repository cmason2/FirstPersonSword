using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isLocked = false;
    public bool isOpen = false;
    public bool isMoving = false;

    public IEnumerator RotateDoor(bool open, float rotateTime) //https://answers.unity.com/questions/672456/rotate-an-object-a-set-angle-over-time-c.html
    {
        isMoving = true;
        Quaternion fromAngle = transform.rotation;
        Vector3 rotateAmount;
        if (open)
        {
            rotateAmount = new Vector3(0, -90, 0);
        }
        else
        {
            rotateAmount = new Vector3(0, 90, 0);
        }
        Quaternion toAngle = Quaternion.Euler(transform.eulerAngles + rotateAmount);
        for (var t = 0f; t < 1; t += Time.deltaTime / rotateTime)
        {
            transform.rotation = Quaternion.Lerp(fromAngle, toAngle, t);
            yield return null;
        }
        transform.rotation = toAngle;
        isOpen = !isOpen;
        isMoving = false;
    }
}
