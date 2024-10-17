using System;

namespace Core.Player
{
    public class PlayerData
    {
        private float baseSpeedRotate = 7;
        private float baseSpeed = 5f;
        private float baseGravity = -9.81f;
        private float baseJumpForce = 3000f;
        private float baseMass = 400;
        private float baseMinMass=300;
        private int level = 1;

        private float speedRotate;
        private float speed;
        private float gravity;
        private float jumpForce;
        private float mass;
        

        public float SpeedRotate { get; private set; }
        public float Speed { get; private set; }
        public float Gravity { get; private set; }
        public float JumpForce { get; private set; }
        public float BaseMinMass { get; private set; }
        public int Level { get; set; }

        public bool IsMine { get; set; }

        public float Mass
        {
            get => mass; set
            {
                mass = value;
                OnMassChanged(value);
            }
        }
        
        public Action<float> MassChanged;
        

        public void LevelUp(int level)
        {
            Level = level;
        }

        public void UpdateStats()
        {
            SpeedRotate = baseSpeedRotate + (Level * 1f);
            Speed = baseSpeed + (Level * 0.1f);
            Gravity = baseGravity;
            JumpForce = baseJumpForce + (Level * 100f);
            Mass = baseMass;
        }

        private void OnMassChanged(float newValue)
        {
            MassChanged?.Invoke(newValue);
        }
    }
}
