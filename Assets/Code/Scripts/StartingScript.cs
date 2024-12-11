using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingScript : MonoBehaviour
{
    [SerializeField]
    GameObject StartingUI;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space))
        {
            GameManager.Instance.cameraBox.TurnOn();
            GameManager.Instance.player.TurnOn();
            StartingUI.SetActive(false);
        }
    }
}
