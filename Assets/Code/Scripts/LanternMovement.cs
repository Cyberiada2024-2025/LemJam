using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using UnityEngine;

public class LampionMovement : MonoBehaviour
{
    private float time = 0.0f;


    [SerializeField]
    AnimationCurve curve;

    [SerializeField]
    float max_height = 10;

    [SerializeField]
    float max_time = 10;
    
    
    
    
 


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveLantern();
    }


    void MoveLantern()
    {
        time += Time.deltaTime;
        float position = max_height * curve.Evaluate(time / max_time);
        transform.position = new Vector3(transform.position.x, position, transform.position.z);
    }

}
