using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroLerp : MonoBehaviour
{
    [SerializeField] Transform endPoint;

    [SerializeField] float speed;

    Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        direction = (endPoint.position - transform.position).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
        if(Vector3.Distance( transform.position, endPoint.position ) < 2f)
        {
            // move to other scene
        }
    }
}
