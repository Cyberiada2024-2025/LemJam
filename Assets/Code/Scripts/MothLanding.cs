using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngineInternal;

public class MothLanding : MonoBehaviour
{
    private bool landing = false;
    private bool isLanded = false;
    private Vector3 startPos;

    private float time = 0;
    [SerializeField]
    float maxDuration = 2;
    [SerializeField]
    float AfterLandingTime = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (landing)
        {
            FlyToEnd();
        }
        else if(isLanded)
        {
            Landing();
        }
    }
    void FlyToEnd()
    {
        time += Time.deltaTime;
        //Debug.Log("end");
        var rotation = Quaternion.LookRotation(GameManager.Instance.landingPoint.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, time/maxDuration);

        Vector3 newPosition = Vector3.Lerp(startPos, GameManager.Instance.landingPoint.position, time/maxDuration);
        transform.position = newPosition;

        if(time >= maxDuration)
        {
            time = 0;
            //Debug.Log("stop");
            isLanded = true;
            landing = false;
        }
    }

    void Landing()
    {
        time += Time.deltaTime;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, 0), (float)(time/AfterLandingTime));
        if(time > AfterLandingTime)
        {
            //Debug.Log("land");
            //isLanded=false;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            startPos = transform.position;
            landing = true;
            GameManager.Instance.player.enabled = false;
        }
    }
}
