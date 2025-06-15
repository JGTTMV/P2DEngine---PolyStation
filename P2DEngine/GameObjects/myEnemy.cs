using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using P2DEngine.GameObjects.Collisions;

namespace P2DEngine.GameObjects
{
    public class myEnemy : myPhysicsCircle
    {
        public Image spriteName;
        public float speed = 0.6f; // Velocidad del enemigo
        private float directionX;
        private float directionY;
        private Random rand = new Random();
        private float moveTimer = 0f;
        private float changeDirectionInterval;

        public myEnemy(float x, float y, float radius, float mass, Image spriteName)
            : base(x, y, radius, spriteName) // Fix: Pass null for the image parameter
        {
            this.x = x;
            this.y = y;
            this.radius = radius;

            // Make enemies faster - increase base speed by reducing divisor further
            speed = Math.Max((0.6f) * 2, (12.0f) * 2 / (sizeX + sizeY));

            // Initialize random movement direction
            InitializeRandomDirection();

            // Shorter intervals between direction changes for more dynamic movement
            changeDirectionInterval = (float)(rand.NextDouble() * 2.5 + 1.5);
        }

        private void InitializeRandomDirection()
        {
            directionX = (float)(rand.NextDouble() * 2 - 1); // Random value between -1 and 1
            directionY = (float)(rand.NextDouble() * 2 - 1);
            NormalizeDirection();
        }
        public void ReverseXDirection()
        {
            directionX = -directionX;
        }

        public void ReverseYDirection()
        {
            directionY = -directionY;
        }

        private void NormalizeDirection()
        {
            float magnitude = (float)Math.Sqrt(directionX * directionX + directionY * directionY);
            if (magnitude > 0)
            {
                directionX /= magnitude;
                directionY /= magnitude;
            }
        }
        public override void Update(float deltaTime)
        {
            // Occasionally change direction randomly
            moveTimer += deltaTime;
            if (moveTimer >= changeDirectionInterval)
            {
                // 30% chance to change direction
                if (rand.NextDouble() < 0.3)
                {
                    InitializeRandomDirection();
                }
                moveTimer = 0;
            }

            // Calculate current speed based on size - increased speed multiplier to 45 for faster movement
            float currentSpeed = Math.Max(0.6f, 12.0f / (sizeX + sizeY)) * speed;

            // Move enemy based on direction and speed with increased base movement rate
            x += directionX * currentSpeed * 45 * deltaTime;
            y += directionY * currentSpeed * 45 * deltaTime;
        }
        public override void Draw(Graphics g, Vector position, Vector size)
        {
        }
    }
}
