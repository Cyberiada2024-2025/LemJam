using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(CameraBoxScript))]
public class MothPath : Editor
{
    void OnSceneGUI()
    {
        // Get the chosen GameObject
        CameraBoxScript t = target as CameraBoxScript;

        if (t == null || t.finish == null)
            return;


        // Grab the center of the parent
        Vector3 beggining = t.transform.position;
        Vector3 endPoint = t.finish.transform.position;

        int N = 30;
        for(int i = 0; i < N-1; i++)
        {
            float perc = (float)i / N;
            float perc2 = ((float)i+1) / N;
            Vector3 lineStart = Vector3.Lerp(beggining,endPoint,perc);
            Vector3 lineENd = Vector3.Lerp(beggining,endPoint,perc2);

            float startY = beggining.y + t.curve.Evaluate(perc) * t.height;
            float endY = beggining.y + t.curve.Evaluate(perc2) * t.height;

            lineStart.y = startY;
            lineENd.y = endY;

            Handles.DrawLine(lineStart, lineENd);
        }

    }
}
