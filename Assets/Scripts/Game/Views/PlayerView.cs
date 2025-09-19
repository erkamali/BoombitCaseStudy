using Game.Constants;
using UnityEngine;

namespace Game.Views
{
    public class PlayerView : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float _moveSpeed   = 5f;
        [SerializeField] private float _deadZone    = 0.1f; // Minimum joystick input to register movement
        
        [Header("References")]
        [SerializeField] private Animator _animator;
        
        // Input variables
        private Vector2 _inputVector;
        private float   _currentSpeed;
        
        // Direction vectors for our 4 animations
        private readonly Vector2[] directions = {
            Vector2.up,     // 0: Forward
            Vector2.right,  // 1: Backward  
            Vector2.down,   // 2: Right
            Vector2.left    // 3: Left
        };
        
        void Start()
        {
            if (_animator == null)
            {
                _animator = GetComponent<Animator>();
            }
        }
        
        void Update()
        {
            HandleInput();
            UpdateAnimator();
            MovePlayer();
        }
        
        private void HandleInput()
        {
            _inputVector.x = Input.GetAxis("Horizontal");
            _inputVector.y = Input.GetAxis("Vertical");
            
            _currentSpeed = _inputVector.magnitude;
            if (_currentSpeed < _deadZone)
            {
                _currentSpeed = 0f;
                _inputVector = Vector2.zero;
            }
            else
            {
                _inputVector = _inputVector.normalized;
            }
        }
        
        private void UpdateAnimator()
        {
            _animator.SetFloat(PlayerMovementConstants.SPEED_PARAM,     _currentSpeed);
            _animator.SetFloat(PlayerMovementConstants.INPUT_X_PARAM,   _inputVector.x);
            _animator.SetFloat(PlayerMovementConstants.INPUT_Y_PARAM,   _inputVector.y);
            
            if (_currentSpeed > _deadZone)
            {
                int closestDirection = GetClosestDirection(_inputVector);
                _animator.SetInteger(PlayerMovementConstants.DIRECTION_PARAM, closestDirection);
            }
        }
        
        private int GetClosestDirection(Vector2 input)
        {
            float dotProduct = -1f;
            int closestDirection = 0;
            
            // Find which direction vector is closest to our input
            for (int i = 0; i < directions.Length; i++)
            {
                float dot = Vector2.Dot(input, directions[i]);
                if (dot > dotProduct)
                {
                    dotProduct = dot;
                    closestDirection = i;
                }
            }
            
            return closestDirection;
        }
        
        private void MovePlayer()
        {
            // Move the player based on input
            if (_currentSpeed > _deadZone)
            {
                Vector3 movement = new Vector3(_inputVector.x, 0, _inputVector.y) * _moveSpeed * Time.deltaTime;
                transform.Translate(movement, Space.World);
            }
        }
        
        // Optional: Visualize direction vectors in Scene view
        private void OnDrawGizmos()
        {
            if (_currentSpeed > _deadZone)
            {
                Gizmos.color = Color.red;
                Vector3 inputDir = new Vector3(_inputVector.x, 0, _inputVector.y);
                Gizmos.DrawRay(transform.position, inputDir * 2f);
                
                int closest = GetClosestDirection(_inputVector);
                Gizmos.color = Color.green;
                Vector3 closestDir = new Vector3(directions[closest].x, 0, directions[closest].y);
                Gizmos.DrawRay(transform.position, closestDir * 1.5f);
            }
        }
    }
}