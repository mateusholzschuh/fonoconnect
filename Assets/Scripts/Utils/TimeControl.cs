using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeControl : MonoBehaviour {

    public static float timeScale        = 1;
    private const float timeScaleDefault = 1;

    void Update() {
        Time.timeScale = timeScale;
    }

    public static void PauseGame() {
        timeScale = 0;
    }

    public static void ResumeGame(){
        timeScale = timeScaleDefault;
    }
}
