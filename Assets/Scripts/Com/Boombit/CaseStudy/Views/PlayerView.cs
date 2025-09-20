using UnityEngine;
using Com.Boombit.CaseStudy.Constants;

namespace Com.Boombit.CaseStudy.Views
{
    public class PlayerView : MonoBehaviour
    {
        //  MEMBERS
        //      Editor
        [Header("Movement Settings")]
        [SerializeField] private float _moveSpeed       = 5f;
        [SerializeField] private float _deadZone        = 0.1f;
        [SerializeField] private float _rotationSpeed   = 180f;
        
        [Header("References")]
        [SerializeField] private Animator _animator;
        
        //      Private
        private Vector2 _movementVector;
        private Vector2 _rotationVector;
        private float   _currentSpeed;
        
        private readonly Vector2[] directions = {
            Vector2.up,     // 0: Forward
            Vector2.right,  // 1: Right  
            Vector2.down,   // 2: Backward
            Vector2.left    // 3: Left
        };
        
        //  METHODS
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
            // Movement input
            _movementVector.x = Input.GetAxis("Horizontal");
            _movementVector.y = Input.GetAxis("Vertical");

            // Rotation input
            _rotationVector.x = Input.GetAxis("RightStickX");
            _rotationVector.y = Input.GetAxis("RightStickY");

            if (Mathf.Abs(_rotationVector.x) < 0.1f)
            {
                if (Input.GetKey(KeyCode.Q))
                    _rotationVector.x = -1f;
                else if (Input.GetKey(KeyCode.E))
                    _rotationVector.x = 1f;
                else
                    _rotationVector.x = 0f;
            }
            
            _currentSpeed = _movementVector.magnitude;
            if (_currentSpeed < _deadZone)
            {
                _currentSpeed = 0f;
                _movementVector = Vector2.zero;
            }
            else
            {
                _movementVector = _movementVector.normalized;
            }

            // Attack input
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
            {
                _animator.SetTrigger(PlayerAnimationParameters.ATTACK);
            }
        }
        
        private void UpdateAnimator()
        {
            _animator.SetFloat(PlayerAnimationParameters.SPEED,     _currentSpeed);
            _animator.SetFloat(PlayerAnimationParameters.INPUT_X,   _movementVector.x);
            _animator.SetFloat(PlayerAnimationParameters.INPUT_Y,   _movementVector.y);
            
            if (_currentSpeed > _deadZone)
            {
                int closestDirection = GetClosestDirection(_movementVector);
                _animator.SetInteger(PlayerAnimationParameters.DIRECTION, closestDirection);
            }
        }
        
        private int GetClosestDirection(Vector2 input)
        {
            float dotProduct = -1f;
            int closestDirection = 0;
            
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
            // Rotation
            if (Mathf.Abs(_rotationVector.x) > _deadZone)
            {
                float rotationAmount = _rotationVector.x * _rotationSpeed * Time.deltaTime;
                transform.Rotate(0, rotationAmount, 0);
            }
            
            // Movement
            if (_currentSpeed > _deadZone)
            {
                Vector3 localMovement = new Vector3(_movementVector.x, 0, _movementVector.y);
                Vector3 worldMovement = transform.TransformDirection(localMovement) * _moveSpeed * Time.deltaTime;
                transform.Translate(worldMovement, Space.World);
            }
        }

        private Vector2 GetDirectionVector(PlayerMovementDirections movementDirection)
        {
            return directions[(int)movementDirection];
        }
    }
}