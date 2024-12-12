using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using TMPro;

public class GameManager : MonoBehaviour
{
    private float StartingZ;
    public int MaxScore = 1000;
    public int PlayerScore = 0;

    public static GameManager Instance;

    public MothMovement player;
    public Slider EnergyBar;
    public Canvas PauseMenuCanvas;
    public CameraBoxScript cameraBox;
    public Transform landingPoint;
    public GameObject scoreLabel;

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
        PauseMenuCanvas.enabled = false;
    }
      public static void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        if(GameManager.Instance.PlayerScore > 0)
        { //GameObject.FindGameObjectWithTag("ScoreLabel")
           GameManager.Instance.scoreLabel.GetComponent<TextMeshProUGUI>().SetText("Score: "+ GameManager.Instance.PlayerScore.ToString());
        }
        Debug.Log(GameManager.Instance.PlayerScore);
        
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseMenu();
        }
    }

    public void PauseMenu()
    {
        PauseMenuCanvas.enabled = true;
        Time.timeScale = 0;
    }

    public void SetEnergy(float energy)
    {
        EnergyBar.value = energy;
    }

    public void setScore(float posZ)
    {
        PlayerScore = (int)(MaxScore * (posZ - StartingZ) / (landingPoint.transform.position.z - StartingZ));
    }

    public void OnExitButtonClicked()
    {
#if UNITY_EDITOR
UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
    public void OnResumeButtonClicked()
    {
        PauseMenuCanvas.enabled = false;
        Time.timeScale = 1;
       
    }
    
    public void OnRestartButtonClicked()
    {
        Time.timeScale = 1;
        PauseMenuCanvas.enabled = false;
    }

}
