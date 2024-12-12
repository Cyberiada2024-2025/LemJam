using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private float StartingZ;
    public int MaxScore = 1000;

    public static GameManager Instance;

    public MothMovement player;
    public Slider EnergyBar;
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
            StartingZ = player.transform.position.z;
        }
        if (cameraBox == null)
        {
            cameraBox = GameObject.FindGameObjectWithTag("CameraBox").GetComponent<CameraBoxScript>();
        }
        if (landingPoint == null)
        {
            landingPoint = GameObject.FindGameObjectWithTag("LandingPoint").transform;
        }

        EnergyBar.value = player.GetCurrentEnergy();
    }
    public void SetEnergy(float energy)
    {
        EnergyBar.value = energy;
    }

    public int GetScore(float posZ)
    {
        return (int)(MaxScore * (posZ - StartingZ) / (landingPoint.transform.position.z - StartingZ));
    }

}
