namespace Core.Player
{
    public class PlayerData
    {
        private float baseSpeedRotate=5;
        private float baseSpeed = 12f;
        private float baseGravity = -9.81f;
        private float baseJumpForce = 3000f;

        private int level = 1;
        
        private float speedRotate;
        private float speed;
        private float gravity;
        private float jumpForce;
        
        public float SpeedRotate { get; private set; }
        public float Speed {get;private set; }
        public float Gravity { get; private set; }
        public float JumpForce { get; private set; }
        public int Level { get;  set; }
       

        public void LevelUp(int level)
        {
            Level = level;
        }

        public void UpdateStatsOnLevelUp()
        {
            SpeedRotate = baseSpeedRotate+(Level * 1f);
            Speed = baseSpeed + (Level * 0.1f); 
            Gravity = baseGravity;
            JumpForce = baseJumpForce + (Level * 100f); 
        }
    }
}
