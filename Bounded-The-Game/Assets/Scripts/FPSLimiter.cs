using UnityEngine;

public class FPSLimiter : MonoBehaviour
{
    public int targetFPS = 60; // Set your desired FPS here

    void Awake()
    {
        Application.targetFrameRate = targetFPS;
        QualitySettings.vSyncCount = 0; 
    }
}