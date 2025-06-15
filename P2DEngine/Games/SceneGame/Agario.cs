using P2DEngine.GameObjects;
using P2DEngine.Games;
using P2DEngine.Games.SceneGame;
using P2DEngine.Managers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Forms;

namespace P2DEngine.Games.SceneGame
{
    public class Agario : myScene     
    {
        private myPlayer player;
        private Image playerSprite;
        private int enemyCount = 50;
        private List<myEnemy> enemies;
        private List<myPhysicsBlock> walls;
        private Image wallSprite;

        private float playerDistance = 0.0f;
        float playerSpeed = 0.6f;
        float baseStep = 30;

        private int pistaX = 100;
        private int pistaY = 100;
        private int pistaAncho = 1200;
        private int pistaAlto = 1200;
        private int grosorBorde = 20;

        private int foodCount;
        private int foodSpeciaACount;
        private int foodSpeciaBCount;
        private int foodSpeciaCCount;

        public Agario(myCamera camera) : base(camera)
        {
        }
        public override void Init()
        {
            gameObjects.Clear();

            playerSprite = myImageManager.Get("Agario_Player");
            player = new myPlayer(pistaX + pistaAncho / 2, pistaY + pistaAlto / 2, 10, playerSprite);
            Instantiate(player);

            // Ahora crea la cámara y asígnale el jugador como objetivo
            currentCamera = new myCamera(0, 0, 800, 600, 1.0f);
            currentCamera.SetTargetPlayer(player);

            walls = new List<myPhysicsBlock>();
            wallSprite = myImageManager.Get("Agario_Block");


            walls.Add(new myPhysicsBlock(100, 100, pistaAncho, 15, wallSprite));
            walls.Add(new myPhysicsBlock(100, 100, 15, pistaAlto, wallSprite));
            walls.Add(new myPhysicsBlock(pistaAncho + 85, 100, 15, pistaAlto, wallSprite));
            walls.Add(new myPhysicsBlock(100, pistaAlto + 85, pistaAncho, 15, wallSprite));

            foreach (var wall in walls)
            {
                Instantiate(wall);
            }
        }
        public void ResetGame()
        {
        }
        public override void Update(float deltaTime)
        {
            // Centra la cámara en el jugador
            if (player != null && currentCamera != null)
            {
                currentCamera.x = player.x + player.radius - currentCamera.width / 2;
                currentCamera.y = player.y + player.radius - currentCamera.height / 2;
            }

            // Verifica colisión con cada pared y resuelve
            foreach (var wall in walls)
            {
                if (player.IsColliding(wall))
                {
                    // Calcula la diferencia de posición
                    float overlapLeft = (player.x + player.sizeX) - wall.x;
                    float overlapRight = (wall.x + wall.sizeX) - player.x;
                    float overlapTop = (player.y + player.sizeY) - wall.y;
                    float overlapBottom = (wall.y + wall.sizeY) - player.y;

                    // Encuentra la menor superposición (eje de resolución)
                    float minOverlapX = Math.Min(overlapLeft, overlapRight);
                    float minOverlapY = Math.Min(overlapTop, overlapBottom);

                    if (minOverlapX < minOverlapY)
                    {
                        // Resolver en X
                        if (overlapLeft < overlapRight)
                            player.x = wall.x - player.sizeX;
                        else
                            player.x = wall.x + wall.sizeX;
                    }
                    else
                    {
                        // Resolver en Y
                        if (overlapTop < overlapBottom)
                            player.y = wall.y - player.sizeY;
                        else
                            player.y = wall.y + wall.sizeY;
                    }

                    // Actualiza el collider del jugador después de moverlo
                    player.CreateCollider(player.sizeX, player.sizeY);
                }
            }
        }

        public override void ProcessInput()
        {
            bool moved = false;
            if (myInputManager.IsKeyPressed(Keys.W) || myInputManager.IsKeyPressed(Keys.Up))
            {
                player.y -= (int)(baseStep * player.speed);
                moved = true;
            }
            if (myInputManager.IsKeyPressed(Keys.S) || myInputManager.IsKeyPressed(Keys.Down))
            {
                player.y += (int)(baseStep * player.speed);
                moved = true;
            }
            if (myInputManager.IsKeyPressed(Keys.A) || myInputManager.IsKeyPressed(Keys.Left))
            {
                player.x -= (int)(baseStep * player.speed);
                moved = true;
            }
            if (myInputManager.IsKeyPressed(Keys.D) || myInputManager.IsKeyPressed(Keys.Right))
            {
                player.x += (int)(baseStep * player.speed);
                moved = true;
            }

            if (moved)
                player.CreateCollider(player.sizeX, player.sizeY);

            if (myInputManager.IsKeyPressed(Keys.Enter))
            {
                ResetGame();
                mySceneManager.SetActive("Menu");
            }
        }

        public override void Render(Graphics g)
        {
            g.Clear(Color.Black);

            //Dibuja objetos del juego
            foreach (var gameObject in gameObjects)
            {
                gameObject.Draw(g,
                    currentCamera.GetViewPosition(gameObject.x, gameObject.y),
                    currentCamera.GetViewSize(gameObject.sizeX, gameObject.sizeY));
            }
        }
    }
}
