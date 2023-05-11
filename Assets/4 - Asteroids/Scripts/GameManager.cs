using UnityEngine;

namespace IAM.Asteroids
{
    public class GameManager : MonoBehaviour
    {
        public void Play() { Time.timeScale = 1f; }
        public void Pause() { Time.timeScale = 0f; }
    }
}