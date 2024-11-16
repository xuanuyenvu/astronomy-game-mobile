using System;
using UnityEngine;

namespace config {
    [Serializable]
    public class AnimationSpeedConfig {
        [SerializeField]
        [Tooltip("Animation speed in degrees per second")]
        public float rotation = 60f;

        [SerializeField]
        public float position = 500f;
        
        [SerializeField]
        public float releasePosition = 1500f;

        [SerializeField]
        public float zoom = 0.3f;
    }
}