using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class autocreateenemy : MonoBehaviour
{
    public float minSecond = 5.0f;
    public GameObject createGameObject;
    public float maxSecond = 10.0f;
    public GameObject targetTrace;
    private float timer;
    private float creatTime;



    // Start is called before the first frame update
    void Start()
    {
        if (targetTrace == null)
            targetTrace = GameObject.FindGameObjectWithTag("Player");
        timer = 0.0f;
        creatTime = Random.Range(minSecond, maxSecond);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > creatTime)
        {
            CreateObject();
            timer = 0.0f;
            creatTime = Random.Range(minSecond, maxSecond);

        }
    }


    void CreateObject()
    {
        Vector3 deltaVector = new Vector3(0.0f, 5.0f, 0.0f);

       
        GameObject newGameObject = Instantiate(
            createGameObject,
            transform.position - deltaVector,
            transform.rotation) as GameObject;
        

        if(newGameObject.GetComponent<EnemyTrace>() != null)
        {
            newGameObject.GetComponent<EnemyTrace>().target = targetTrace;
        }
            
    }
}
