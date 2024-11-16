using UnityEngine;

public class FPSCap : MonoBehaviour {
    private void Awake() {
        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = 120;
    }
}
