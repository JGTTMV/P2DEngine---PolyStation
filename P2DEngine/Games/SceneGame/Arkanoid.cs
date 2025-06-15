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

namespace P2DEngine
{
    public class Arkanoid : myScene
    {
        private Image arkanoidBackground;
        private Font gameFont;
        private Font GameOverFont;

        private myPhysicsBlock player;
        private myPhysicsCircle ball;
        private List<myPhysicsBlock> bricks;
        private List<myPhysicsBlock> bricksToRemove;
        private Image playerSprite;
        private Image ballSprite;
        private Image Brick1;
        private Image Brick2;
        private Image Brick3;

        private float ballSpeedY = 400f; //Velocidad en el eje Y
        private float ballSpeedX = 400f; //Velocidad en el eje X
        private int score = 0; //Puntuacion del jugador
        private int vidas = 3; //Vidas del jugador
        private bool isGameOver = false;

        private float playerCenter;
        private float brickCenter;
        private float ballCenter;

        public Arkanoid(myCamera camera) : base(camera)
        {
        }

        public override void Init()
        {
            gameObjects.Clear();

            isGameOver = false;

            gameFont = myFontManager.Get("GameFont", 15);

            arkanoidBackground = myImageManager.Get("Arkanoid_Background");

            playerSprite = myImageManager.Get("Arkanoid_Player"); // Cargar el sprite del jugador.
            ballSprite = myImageManager.Get("Arkanoid_Ball"); // Cargar el sprite de la bola.
            Brick1 = myImageManager.Get("Brick1");
            Brick2 = myImageManager.Get("Brick2");
            Brick3 = myImageManager.Get("Brick3");

            player = new myPhysicsBlock(350, 545, 125, 25, playerSprite); // Crear el bloque del jugador.
            ball = new myPhysicsCircle(400, 300, 15, ballSprite); // Crear la bola del juego.

            CreateBricks();

            Instantiate(player); // Añadir el bloque del jugador a la escena.
            Instantiate(ball); // Añadir la bola a la escena.
        }

        private void CreateBricks()
        {
            bricks = new List<myPhysicsBlock>();

            if (bricks != null)
                bricks.Clear();

            for (int i = 0; i < 8; i++)
            {
                bricks.Add(new myPhysicsBlock(100 * i, 40, 100, 30, Brick3));
                bricks.Add(new myPhysicsBlock(100 * i, 70, 100, 30, Brick3));
                bricks.Add(new myPhysicsBlock(100 * i, 100, 100, 30, Brick2));
                bricks.Add(new myPhysicsBlock(100 * i, 130, 100, 30, Brick2));
                bricks.Add(new myPhysicsBlock(100 * i, 160, 100, 30, Brick1));
                bricks.Add(new myPhysicsBlock(100 * i, 190, 100, 30, Brick1));
            }
            foreach (var brick in bricks)
            {
                Instantiate(brick);
            }
        }

        private void ResetGame()
        {
            isGameOver = false;

            if(bricks != null)
                bricks.Clear();

            ball.x = 400;
            ball.y = 300;

            player.x = 350;
            player.y = 545;

            score = 0;
            vidas = 3;

            CreateBricks();
        }

        // Primera parte del GameLoop: Procesar inputs.
        public override void ProcessInput()
        {
            if((myInputManager.IsKeyPressed(Keys.Left) && !isGameOver) || (myInputManager.IsKeyPressed(Keys.A) && !isGameOver)) // Mover el bloque del jugador a la izquierda.
            {
                if(player.x <= 0) // Asegurarse de que el jugador no se salga de la pantalla.
                    player.x = 0; // Si está en el borde izquierdo, no lo movemos más.
                else
                    player.x -= 10; // Mover 10 píxeles a la izquierda.
            }
            if ((myInputManager.IsKeyPressed(Keys.Right) && !isGameOver) || (myInputManager.IsKeyPressed(Keys.D) && !isGameOver)) // Mover el bloque del jugador a la derecha.
            {
                if(player.x >= 800 - player.sizeX) // Asegurarse de que el jugador no se salga de la pantalla.
                    player.x = 800 - player.sizeX; // Si está en el borde derecho, no lo movemos más.
                else
                    player.x += 10; // Mover 10 píxeles a la derecha.
            }
            if (myInputManager.IsKeyPressed(Keys.R) && isGameOver)
                ResetGame();

            if (myInputManager.IsKeyPressed(Keys.Enter)) // Cambiar a la escena 1.
            {
                ResetGame();

                myAudioManager.Stop(GlobalAudioState.ArkanoidMusicIndex);
                GlobalAudioState.ArkanoidMusicIndex = -1; // Resetea el índice

                mySceneManager.SetActive("Menu");
            }
        }

        public override void Update(float deltaTime)
        {
            if (isGameOver) 
                return;

            //Lista para eliminar cada ladrillo
            bricksToRemove = new List<myPhysicsBlock>();

            //Actualiza la posicion del balon
            ball.x += ballSpeedX * deltaTime;
            ball.y += ballSpeedY * deltaTime;

            //Comprueba colisiones con los bordes de la pantalla
            if (ball.y <= 0)
            {
                ballSpeedY = -ballSpeedY; //Invierte la direccion en el eje Y.
                myAudioManager.Play("Pong_Hit"); //Sonido de rebote.
            }
            if (ball.y >= 600 - ball.radius * 2)
            {
                Random rand = new Random();

                myAudioManager.Play("Boop");
                ball.x = 400;
                ball.y = 300;

                ballSpeedX = Math.Abs(ballSpeedX) * (rand.Next(0, 2) == 0 ? 1 : -1);

                vidas--;
            }
            if (ball.x <= 0 || ball.x >= 800 - ball.radius * 2)
            {
                ballSpeedX = -ballSpeedX; //Invierte la direccion en el eje Y.
                myAudioManager.Play("Pong_Hit"); //Sonido de rebote.
            }
            //Comprueba las colisiones con los jugadores
            if (ball.IsColliding(player)) //Dado el angulo horizontal del jugador, es mas facil usar esta funcion
            {
                myAudioManager.Play("Pong_Hit");

                playerCenter = player.x + player.sizeX / 2;
                ballCenter = ball.x + ball.radius;

                ballSpeedY = -ballSpeedY; //Invierte la direccion en el eje Y
                ball.y = player.y - ball.radius * 2 - 1; //Ajusta la posicion del balon para evitar que se quede atrapado
            }
            if(vidas <= 0 || bricks.Count() == 0)
            {
                myAudioManager.Play("GameOver");
                isGameOver = true;
                ball.x = 900;
            }

            foreach (var brick in bricks)
            {
                if (ball.IsColliding(brick))
                {
                    myAudioManager.Play("Cruch", 0.5f);

                    score += 10;

                    brickCenter = brick.x + brick.sizeX / 2;
                    ballCenter = ball.x + ball.radius;

                    ballSpeedY = -ballSpeedY;
                    ball.y = brick.y + ball.radius * 2 + 1;

                    bricksToRemove.Add(brick);
                }
            }

            foreach (var brick in bricksToRemove)
            {
                Destroy(brick);
                bricks.Remove(brick);
            }
        }

        public override void Render(Graphics g)
        {
            g.DrawImageUnscaled(arkanoidBackground, 0, 0); // Dibuja el fondo del juego.

            player.Draw(g, new Vector(player.x, player.y), new Vector(player.sizeX, player.sizeY)); // Dibuja el bloque del jugador.
            ball.Draw(g, new Vector(ball.x, ball.y), new Vector(ball.sizeX, ball.sizeY)); // Dibuja la bola.

            foreach(var brick in bricks)
            {
                brick.Draw(g, new Vector(brick.x, brick.y), new Vector(brick.sizeX, brick.sizeY));
            }

            GameOverFont = myFontManager.Get("GameFont", 20);

            if (isGameOver)
            {
                g.DrawString("FIN DEL JUEGO - Puntaje final: " + score, GameOverFont, Brushes.Red, new PointF(120, 250));
                g.DrawString("Presione 'R' para reiniciar", GameOverFont, Brushes.Red, new PointF(150, 300));
            }
            else
            {
                g.DrawString("Vidas: " + vidas, gameFont, Brushes.White, new PointF(18, 10));
                g.DrawString("Puntaje: " + score, gameFont, Brushes.White, new PointF(150, 10));
            }

            //Indicaciones de como volver al menu
            g.DrawString("Presione 'Enter' para volver al menu", gameFont, Brushes.Black, new PointF(20, 578)); //Sombreado
            g.DrawString("Presione 'Enter' para volver al menu", gameFont, Brushes.White, new PointF(18, 578));
        }
    }
}
