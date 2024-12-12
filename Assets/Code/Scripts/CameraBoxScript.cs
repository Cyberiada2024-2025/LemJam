using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBoxScript : MonoBehaviour
{
    private float cur_position = 0.0f;
    private float start_position = 0.0f;
    private bool is_started = false;

    [SerializeField]
    public AnimationCurve curve;
    [SerializeField]
    public Transform finish;
    [SerializeField]
    public float height = 20.0f;

    public float BoundaryX = 10.0f;
    public float LerpSpeed = 2.0f;

    [SerializeField]
    CinemachineVirtualCamera startCamera;


    // Start is called before the first frame update
    void Start()
    {
        start_position = transform.position.z;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (is_started)
        {
            cur_position += GameManager.Instance.player.ForwardSpeed * Time.deltaTime;

            if (cur_position < finish.position.z)
            {
                float pos_y = height * curve.Evaluate(cur_position / (finish.position.z - start_position));

                float target_x = GameManager.Instance.player.transform.position.x;
                if (target_x > BoundaryX) {
                    target_x = BoundaryX;
                } else if (target_x < -BoundaryX) {
                    target_x = -BoundaryX;
                }

                Vector3 new_position = new Vector3(Mathf.Lerp(transform.position.x, target_x, LerpSpeed * Time.deltaTime),
                    pos_y,
                    transform.position.z + GameManager.Instance.player.ForwardSpeed * Time.deltaTime
                );
                transform.position = new_position;
            }
        }
        //transform.position.z + forwardSpeed * Time.deltaTime
    }


    public void TurnOn()
    {
        is_started = true;
        startCamera.enabled = false;
    }

}
