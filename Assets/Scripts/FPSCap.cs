using UnityEngine;

public class FPSCap : MonoBehaviour {
    private void Start() {
        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = 120;
    }
}
