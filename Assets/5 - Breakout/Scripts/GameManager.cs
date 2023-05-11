using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IAM.Breakout
{
    public class GameManager : MonoBehaviour
    {
        public void Play() { Time.timeScale = 1f; }
        public void Pause() { Time.timeScale = 0f; }
    }
}
