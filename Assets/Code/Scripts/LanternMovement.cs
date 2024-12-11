using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using UnityEngine;

public class LampionMovement : MonoBehaviour
{
    enum LampionState{
        PRESPAWN,
        RISING,
        FLOATING
    }
    private float time = 0.0f;
    private LampionState state = LampionState.PRESPAWN;

    [SerializeField]
    AnimationCurve curve;

    [SerializeField]
    float max_height = 10;

    [SerializeField]
    float max_time = 10;






    // Start is called before the first frame update
    void Start()
    {
        
        //state = LampionState.RISING;
    }



    // Update is called once per frame
    void Update()
    {

        switch (state)
        {
            case LampionState.PRESPAWN:

                break;
            case LampionState.RISING:
                MoveLantern();

                break;
            case LampionState.FLOATING:

                break;
        }

    }



    void MoveLantern()
    {
        time += Time.deltaTime;
        float position = max_height * curve.Evaluate(time / max_time);
        transform.position = new Vector3(transform.position.x, position, transform.position.z);
        if (time / max_time >= 1.0f)
        {
            this.state = LampionState.FLOATING;
        }
        return;
    }

}
