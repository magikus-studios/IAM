using UnityEngine;
using UnityEngine.Events;

namespace IAM.Pong
{
	public class ScoreSurface : MonoBehaviour
	{
		[field: SerializeField] public int Points { get; private set; }
		[field: SerializeField] public UnityEvent<int> OnScore { get; private set; }
		[field: SerializeField] public UnityEvent<string> OnScoreLabel { get; private set; }
    }
}
