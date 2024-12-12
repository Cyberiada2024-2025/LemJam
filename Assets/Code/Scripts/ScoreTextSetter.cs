using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreTextSetter : MonoBehaviour
{
    public static int scoreToSet = 0;

    // Start is called before the first frame update
    void Start()
    {
        if(scoreToSet != 0)
        {
            GetComponent<TMP_Text>().text = "Score: " + scoreToSet.ToString();
        }
    }

}
