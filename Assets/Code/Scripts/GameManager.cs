using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public MothMovement player;
    public Slider EnergyBar;
    public CameraBoxScript cameraBox;

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
        EnergyBar.value = player.GetCurrentEnergy();
    }
    public void SetEnergy(float energy)
    {
        EnergyBar.value = energy;
    }

}
