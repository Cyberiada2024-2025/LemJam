using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBoxScript : MonoBehaviour
{
    private float cur_position = 0.0f;
    private float start_position = 0.0f;
    private bool is_started = false;

    [SerializeField]
    AnimationCurve curve;
    [SerializeField]
    Transform finish;
    [SerializeField]
    float height = 20.0f;


    // Start is called before the first frame update
    void Start()
    {
        start_position = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        if (is_started)
        {
            cur_position += GameManager.Instance.player.ForwardSpeed * Time.deltaTime;

            if (cur_position < finish.position.z)
            {
                float pos_y = height * curve.Evaluate(cur_position / (finish.position.z - start_position));
                Vector3 new_position = new Vector3(transform.position.x, pos_y, transform.position.z + GameManager.Instance.player.ForwardSpeed * Time.deltaTime);
                transform.position = new_position;
            }
        }
        //transform.position.z + forwardSpeed * Time.deltaTime
    }


    public void TurnOn()
    {
        is_started = true;
    }

}
