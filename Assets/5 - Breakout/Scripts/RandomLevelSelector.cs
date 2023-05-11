using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace IAM.Breakout
{
    public class RandomLevelSelector : MonoBehaviour
    {
        [SerializeField] private List<Level> _LevelList;
        [SerializeField] private UnityEvent<int> _onBricksUpdate;

        private int _currentLevelIndex = 0;
        
        public void SelectRandomLevel()
        {
            for (int i = 0; i < _LevelList.Count; i++) { _LevelList[i].gameObject.SetActive(false); }

            _currentLevelIndex = _LevelList.RandomIndex(_currentLevelIndex);
            _LevelList[_currentLevelIndex].ResetLevel();
            _LevelList[_currentLevelIndex].gameObject.SetActive(true);
            CheckLevel();
        }
        public void CheckLevel() 
        {
            _LevelList[_currentLevelIndex].CheckBricks();
            _onBricksUpdate?.Invoke(_LevelList[_currentLevelIndex].BrickLeft);
        }
    }
}