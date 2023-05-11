using UnityEngine;

namespace IAM
{
	[AddComponentMenu("Magikus/Tools/Managers/GameStats")]
	public class GameStats_Component : MonoBehaviour
	{
		public GameStats_Object Stats;
		public LabelController Lives;
		public LabelController Level;
		public LabelController Score;
		public RectTransform HorizontalProgresBar;
		public RectTransform VerticalProgresBar;

        private void OnEnable()
        {
			if (Stats == null) { return; }
			Stats.SubOnLivesChange(SetLives);
			Stats.SubOnLevelChange(SetLevel);
			Stats.SubOnScoreChange(SetScore);
			Stats.SubOnProgresChange(SetProgresBar);
			Stats.SubOnActiveStateSetTrue(SetAll);
			SetAll();
		}
        private void OnDisable()
        {
			if (Stats == null) { return; }
			Stats.UnsubOnLivesChange(SetLives);
			Stats.UnsubOnLevelChange(SetLevel);
			Stats.UnsubOnScoreChange(SetScore);
			Stats.UnsubOnProgresChange(SetProgresBar);
			Stats.UnsubOnActiveStateSetTrue(SetAll);
		}
        public void SetAll()
        {
			SetLives(Stats.Lives);
			SetLevel(Stats.Level);
			SetScore(Stats.Score);
			SetProgresBar(Stats.Progres);
		}
		public void SetLives(int amount) 
		{
			if (Stats == null || Lives == null) { return; }
			Lives.SetLabel(amount);
		}
		public void SetLevel(int amount) 
		{
			if (Stats == null || Level == null) { return; }
			Level.SetLabel(amount);
		}
		public void SetScore(int amount)
		{
			if (Stats == null || Score == null) { return; }
			Score.SetLabel(amount);
		}
		public void SetProgresBar(float amount) 
		{
			if (Stats == null) { return; }
			if (HorizontalProgresBar != null) { HorizontalProgresBar.localScale = new Vector3(Mathf.Clamp01(amount), HorizontalProgresBar.localScale.y, HorizontalProgresBar.localScale.z); }
			if (VerticalProgresBar != null) { VerticalProgresBar.localScale = new Vector3(VerticalProgresBar.localScale.x, Mathf.Clamp01(amount), VerticalProgresBar.localScale.z); }
		}

		private void Reset()
		{
			Canvas canvas = gameObject.GetComponent<Canvas>();
			if (canvas == null) { return; }
			canvas.worldCamera = Camera.main;
		}
	}
}
