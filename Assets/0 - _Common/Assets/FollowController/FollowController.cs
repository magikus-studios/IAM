using UnityEngine;

public class FollowController : MonoBehaviour
{
    [SerializeField] private Transform follower;
    [SerializeField] private Transform target;

    [SerializeField] private float _strength = 1f;
    [SerializeField] private float _lockRadius = 1f;
    [SerializeField] private bool _useUnscaledTime = false;
    [SerializeField] private bool _ignoreZaxis = true;

    private Vector3 _direction;

    private void Awake()
    {
        if(follower == null) { follower = transform; }
    }

    private void Update()
    {
        if (target == null) { return; }
        Follow();
    }

    private void Follow() 
    {
        if (_strength == 0f) 
        {
            if (_ignoreZaxis) 
            {
                follower.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, follower.transform.position.z);
                return;
            }
            follower.transform.position = target.transform.position; 
            return; 
        }
        _direction = target.transform.position - follower.transform.position;

        if(_direction.magnitude <= _lockRadius) { return; }

        if(_ignoreZaxis) { _direction = new Vector2(_direction.x, _direction.y); }

        follower.transform.position += _direction * _strength * ((_useUnscaledTime) ? Time.unscaledDeltaTime : Time.deltaTime);
    }
}
