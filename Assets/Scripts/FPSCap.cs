using UnityEngine;

public class FPSCap : MonoBehaviour {
    private void Start() {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 120;
    }
}
