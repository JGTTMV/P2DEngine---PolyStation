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
    public class myPlayer : myPhysicsCircle
    {
        public Image spriteName;
        public float speed = 0.6f; // Velocidad del jugador
        public myPlayer(float x, float y, float radius, Image spriteName)
            : base(x, y, radius, spriteName) // Fix: Pass null for the image parameter
        {
            this.x = x;
            this.y = y;
            this.radius = radius;
            this.spriteName = spriteName;
        }

        public override void Update(float deltaTime)
        {
        }

        public override void Draw(Graphics g, Vector position, Vector size)
        {
            if (image == null)
            {
                g.FillEllipse(brush, (float)position.X, (float)position.Y, (float)size.X, (float)size.Y);
            }
            else
            {
                g.DrawImage(image, (float)position.X, (float)position.Y, (float)size.X, (float)size.Y);
            }
        }
    }
}
