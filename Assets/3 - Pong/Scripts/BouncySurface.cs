using UnityEngine;

namespace IAM.Pong
{
	public class BouncySurface : MonoBehaviour
	{
		[field: SerializeField] public float BounceStreangth { get; private set; }
		[SerializeField] private float _max;
		[SerializeField] private float _min;
		[SerializeField] private float _default;

		public void SetBounceStreangth(float streangth) { BounceStreangth = streangth; }
		public void SetMaxBounceStreangth() { BounceStreangth = _max; }
		public void SetMinBounceStreangth() { BounceStreangth = _min; }
		public void ResetBounceStreangth() { BounceStreangth = _default; }
    }
}
