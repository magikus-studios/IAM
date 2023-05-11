using UnityEngine;
using UnityEngine.Events;

namespace IAM.Snake
{
	public class Fruit : MonoBehaviour
	{
		[field: SerializeField] public int Points { get; private set; } = 10;
		[SerializeField] private UnityEvent<string> _beforeSpawn;

		public void Spawn(Vector2Int position) 
		{
			_beforeSpawn?.Invoke($"+{Points}");
			
			transform.position = new Vector3(position.x, position.y);		
		}
		public void Respawn(Vector2Int position)
		{
			transform.position = new Vector3(position.x, position.y);
		}
	}
}
