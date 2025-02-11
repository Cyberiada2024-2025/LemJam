using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using UnityEngine;
using UnityEngine.UIElements;

public class LampionMovement : MonoBehaviour
{
    enum LampionState{
        PRESPAWN,
        RISING,
        FLOATING
    }
    private float start_position;
    private float time = 0.0f;
    private LampionState state = LampionState.PRESPAWN;

    [SerializeField]
    AnimationCurve curve;

    [SerializeField]
    public float max_height = 10;

    [SerializeField]
    float max_rising_time = 10;


    public float distance_to_activate = 10;

    [SerializeField]
    float floating_frequency = 1;


    [SerializeField]
    float floating_amplitude = 1;            


    // Start is called before the first frame update
    void Start()
    {
        start_position = transform.position.y;
        //state = LampionState.RISING;
        //death.startSize = sphere.radius;        
    }

    

    // Update is called once per frame
    void Update()
    {
        Transform kok = GameManager.Instance.player.transform;
        switch (state) { 
            case LampionState.PRESPAWN:
                if(transform.position.z - kok.position.z <= distance_to_activate)
                {
                    state = LampionState.RISING;
                }
                break;
            case LampionState.RISING:
                MoveLantern();

                break;
            case LampionState.FLOATING:
                LanterFloating();
                break;
        }

    }



    void MoveLantern()
    {
        time += Time.deltaTime;
        Vector3 new_position = new Vector3(transform.position.x, start_position + max_height * curve.Evaluate(time / max_rising_time), transform.position.z);
        transform.position = new_position;
        if (time / max_rising_time >= 1.0f)
        {
            this.state = LampionState.FLOATING;
            time = Time.time;
        }
        return;
    }


    void LanterFloating()
    {
        Vector3 new_position = new Vector3(transform.position.x, (float)(start_position + max_height + floating_amplitude*Math.Sin((Time.time - time)*floating_frequency)), transform.position.z);
        this.transform.position = new_position;
    }


}
