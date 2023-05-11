using UnityEngine;

namespace IAM.Pong
{
	public class AIPaddle : Paddle
	{
        [SerializeField] private Rigidbody2D _ball;
        
        private void Update() 
        {
            if (_isPaused) { return; }
            TrackBall(); 
        }

        private void TrackBall()
        {
            if (_ball.velocity.x < 0f) 
            {
                CenterPaddle();
                return;
            }

            if (_ball.position.y > transform.position.y) { MoveUp(); }
            else { MoveDown(); }
        }
        private void CenterPaddle() 
        {
            if (transform.position.y > 0f) { MoveDown(); }
            else { MoveUp(); }
        }
    }
}
