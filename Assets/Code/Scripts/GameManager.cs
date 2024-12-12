using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public MothMovement player;
    public CameraBoxScript cameraBox;
    public Transform landingPoint;

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<MothMovement>();
        }
        if (cameraBox == null)
        {
            cameraBox = GameObject.FindGameObjectWithTag("CameraBox").GetComponent<CameraBoxScript>();
        }
        if (landingPoint == null)
        {
            landingPoint = GameObject.FindGameObjectWithTag("LandingPoint").transform;
        }
    }


}
