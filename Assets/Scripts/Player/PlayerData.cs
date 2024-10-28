using System;

namespace Core.Player
{
    public class PlayerData
    {
        private float _baseSpeedRotate = 7;
        private float _baseSpeed = 5f;
        private float _baseGravity = -9.81f;
        private float _baseJumpForce = 3000f;
        private float _baseMass = 400;
        private float _baseMinMass = 300;

        private float _speedRotate;
        private float _speed;
        private float _gravity;
        private float _jumpForce;
        private float _mass;
        private bool _isActive;


        public float SpeedRotate { get; private set; }
        public float Speed { get; private set; }
        public float Gravity { get; private set; }
        public float JumpForce { get; private set; }
        public float BaseMinMass { get; private set; }

        public bool IsMine { get; set; }
       
        public bool IsActive 
        {
            get=> _isActive;
            set  {
                _isActive=value;
                UnityEngine.Debug.Log("IsActive "+_isActive); 
            }
        }

        public float Mass
        {
            get => _mass; 
            set
            {
                _mass = value;
                OnMassChanged(value);
            }
        }
        
        public Action<float> MassChanged;
        
        public void UpdateStats()
        {
            Speed = _baseSpeed;
            SpeedRotate = _baseSpeedRotate;
            Gravity = _baseGravity;
            JumpForce = _baseJumpForce;
            Mass = _baseMass;
        }

        private void OnMassChanged(float newValue)
        {
            MassChanged?.Invoke(newValue);
        }
    }
}
