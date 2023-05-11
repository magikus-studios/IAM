using System;
using UnityEngine;

namespace IAM
{

	[CreateAssetMenu(fileName = "Game Stats", menuName = "Magikus/Tools/Managers/Game Stats")]
	public class GameStats_Object : ScriptableObject
	{
		private bool active;
		protected Action<bool> onActiveStateChange;
		protected Action onActiveStateSetTrue;
		protected Action onActiveStateSetFalse;

		public bool IsActive() { return active; }
		public void Activate() { SetActive(true); }
		public void Deactivate() { SetActive(false); }
		public void SetActive(bool state) 
		{
			if (active != state) { onActiveStateChange?.Invoke(state); }
			active = state; 
			if (active) { onActiveStateSetTrue?.Invoke(); }
			else { onActiveStateSetFalse?.Invoke(); }
		}
		public void SubOnActiveStateChange(Action<bool> action) { onActiveStateChange += action; }
		public void SubOnActiveStateSetTrue(Action action) { onActiveStateSetTrue += action; }
		public void SubOnActiveStateSetFalse(Action action) { onActiveStateSetFalse += action; }
		public void UnsubOnActiveStateChange(Action<bool> action) { onActiveStateChange -= action; }
		public void UnsubOnActiveStateSetTrue(Action action) { onActiveStateSetTrue -= action; }
		public void UnsubOnActiveStateSetFalse(Action action) { onActiveStateSetFalse -= action; }

		[field: Header("Game Stats")]
		[field: SerializeField] public int Score { get; protected set; } = 0;
		[field: SerializeField] public int Lives { get; protected set; } = 3;
		[field: SerializeField] public int Level { get; protected set; } = 1;
		[field: SerializeField] public float Progres { get; protected set; } = 0f;

		[field: Header("Reset Values")]
		[field: SerializeField] public int ResetScore { get; protected set; } = 0;
		[field: SerializeField] public int ResetLives { get; protected set; } = 3;
		[field: SerializeField] public int ResetLevel { get; protected set; } = 1;
		[field: SerializeField] public float ResetProgres { get; protected set; } = 0f;
		
		protected Action<int> _onScoreChange;
		protected Action<int> _onLivesChange;
		protected Action<int> _onLevelChange;
		protected Action<float> _onProgresChange;

        public void ResetAllValues()
        {
			Score = ResetScore;
			Lives = ResetLives;
			Level = ResetLevel;
			Progres = ResetProgres;
        }

        public void SetScore(int score) { this.Score = score; _onScoreChange?.Invoke(score); }
		public void SetLives(int lives) { this.Lives = lives; _onLivesChange?.Invoke(lives); }
		public void SetLevel(int level) { this.Level = level; _onLevelChange?.Invoke(level); }
		public void SetProgres(float progres) { this.Progres = progres; _onProgresChange?.Invoke(progres); }

		public void AddScore(int amount) { SetScore(Score + amount); }
		public void AddLives(int amount) { SetLives(Lives + amount); }
		public void AddLevel(int amount) { SetLevel(Level + amount); }
		public void AddProgres(float amount) { SetProgres(Progres + amount); }

		public void SubOnScoreChange(Action<int> action) { _onScoreChange += action; }
		public void SubOnLivesChange(Action<int> action) { _onLivesChange += action; }
		public void SubOnLevelChange(Action<int> action) { _onLevelChange += action; }
		public void SubOnProgresChange(Action<float> action) { _onProgresChange += action; }

		public void UnsubOnScoreChange(Action<int> action) { _onScoreChange -= action; }
		public void UnsubOnLivesChange(Action<int> action) { _onLivesChange -= action; }
		public void UnsubOnLevelChange(Action<int> action) { _onLevelChange -= action; }
		public void UnsubOnProgresChange(Action<float> action) { _onProgresChange -= action; }
	}
}
