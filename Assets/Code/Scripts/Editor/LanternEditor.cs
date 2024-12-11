using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;



[CustomEditor(typeof(LampionMovement))]
public class LanternEditor : Editor
{
    void OnSceneGUI()
    {
        // Get the chosen GameObject
        LampionMovement t = target as LampionMovement;

        if (t == null)
            return;

        // Grab the center of the parent
        Vector3 start = t.transform.position;
        Vector3 endPos = start + new Vector3(0, t.max_height, 0);
        Handles.DrawLine(start, endPos);
        Handles.DrawWireDisc(endPos, Vector3.forward, 2);
        Handles.DrawWireDisc(endPos, Vector3.right, 2);
        
    }
}


