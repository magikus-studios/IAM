using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace IAM.Breakout
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private List<Brick> _brickList;

        [SerializeField] private UnityEvent _onLevelCompleted;

        public int BrickAmount 
        { get 
            {
                int amount = 0;
                foreach (Brick brick in _brickList)
                {
                    if (!brick.IsUnbreakable) { amount++; }
                }
                return amount;
            }
        }
        public int BrickLeft
        {
            get
            {
                int amount = 0;
                foreach (Brick brick in _brickList)
                {
                    if (!brick.IsBroken && !brick.IsUnbreakable) { amount++; }
                }
                return amount;
            }
        }
        public void CheckBricks() 
        {
            if (BrickLeft != 0) { return; }
            _onLevelCompleted?.Invoke();
        }
        public void ResetLevel() 
        {
            foreach (Brick brick in _brickList) { brick.Spawn(); }
        }
    }
}