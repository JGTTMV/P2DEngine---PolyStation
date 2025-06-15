using P2DEngine.Games;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Forms;
using P2DEngine.Managers;
using P2DEngine.GameObjects;

namespace P2DEngine.Games.SceneGame
{
    public class Pong : myScene
    {
        private Image pongBackground;
        private Font gameFont;

        //No utilize las versiones de "myPhysics" para tener un control mas preciso sobre la fisica del juego
        private myBlock player;
        private myBlock enemy;
        private myCircle ball;

        //Sprites del juego
        private Image playerSprite;
        private Image enemySprite;
        private Image ballSprite;

        private float ballSpeedY = 400f; //Velocidad en el eje Y
        private float ballSpeedX = 400f; //Velocidad en el eje X
        private int playerScore; //Puntuacion del jugador
        private int enemyScore; //Puntuación del enemigo

        public Pong(myCamera camera) : base(camera)
        {
        }

        public override void Init()
        {
            gameObjects.Clear();

            //Carga la fuente del juego
            gameFont = myFontManager.Get("GameFont", 15);

            //Reinicia puntajes
            playerScore = 0;
            enemyScore = 0;

            //Carga los sprites
            pongBackground = myImageManager.Get("Pong_Background");
            playerSprite = myImageManager.Get("Pong_Player");
            enemySprite = myImageManager.Get("Pong_Enemy");
            ballSprite = myImageManager.Get("Pong_Ball");

            //Inicializa los objetos del juego
            player = new myBlock(10, 100, 25, 125, playerSprite);
            enemy = new myBlock(765, 100, 25, 125, enemySprite);
            ball = new myCircle(400, 300, 15, ballSprite);


        }

        public override void ProcessInput()
        {
            if (myInputManager.IsKeyPressed(Keys.W) || myInputManager.IsKeyPressed(Keys.Up))
            {
                if(player.y <= 0) //Se asegura de que el jugador no se salga de la pantalla
                    player.y = 0;
                else
                    player.y -= 10; //Mueve el jugador hacia arriba
            }
            if (myInputManager.IsKeyPressed(Keys.S) || myInputManager.IsKeyPressed(Keys.Down))
            {
                if(player.y >= 600 - player.sizeY)
                    player.y = 600 - player.sizeY; //Se asegura de que el jugador no se salga de la pantalla
                else
                    player.y += 10; //Mueve el jugador hacia abajo
            }
            if (myInputManager.IsKeyPressed(Keys.Enter)) //Cambiar al menu
            {
                ball.x = 400;
                ball.y = 300;
                player.y = 100;

                playerScore = 0;
                enemyScore = 0;

                myAudioManager.Stop(GlobalAudioState.PongMusicIndex);
                GlobalAudioState.PongMusicIndex = -1; // Resetea el índice

                mySceneManager.SetActive("Menu");
            }
        }

        public override void Render(Graphics g)
        {
            g.DrawImageUnscaled(pongBackground, 0, 0); //Dibuja el fondo del juego

            player.Draw(g, new Vector(player.x, player.y), new Vector(player.sizeX, player.sizeY));
            enemy.Draw(g, new Vector(enemy.x, enemy.y), new Vector(enemy.sizeX, enemy.sizeY));
            ball.Draw(g, new Vector(ball.x, ball.y), new Vector(ball.radius * 2, ball.radius * 2));

            //Dibuja los puntajes
            g.DrawString("Player Score: " + playerScore, gameFont, Brushes.White, new PointF(10, 10));
            g.DrawString("Enemy Score: " + enemyScore, gameFont, Brushes.White, new PointF(600, 10));

            //Indicaciones de como volver al menu
            g.DrawString("Presione 'Enter' para volver al menu", gameFont, Brushes.Black, new PointF(22, 570)); //Sombreado
            g.DrawString("Presione 'Enter' para volver al menu", gameFont, Brushes.White, new PointF(20, 570));
        }

        public override void Update(float deltaTime)
        {
            //Actualiza la posicion del balon
            ball.x += ballSpeedX * deltaTime;
            ball.y += ballSpeedY * deltaTime;

            //Comprueba colisiones con los bordes de la pantalla
            if (ball.y <= 0 || ball.y >= 600 - ball.radius * 2)
            {
                ballSpeedY = -ballSpeedY; //Invierte la direccion en el eje Y.
                myAudioManager.Play("Pong_Hit"); //Sonido de rebote.
            }
            //Comprueba las colisiones con los jugadores
            if (ball.x <= player.x + player.sizeX && ball.x >= player.x && 
                ball.y + ball.radius * 2 >= player.y && ball.y <= player.y + player.sizeY)
            {
                myAudioManager.Play("Pong_Hit");
                ballSpeedX = -ballSpeedX; //Invierte la direccion en el eje X
                ball.x = player.x + player.sizeX; //Ajusta la posicion del balon para evitar que se quede atrapado
            }
            else if (ball.x + ball.radius * 2 >= enemy.x && 
                     ball.x + ball.radius * 2 <= enemy.x + enemy.sizeX &&
                     ball.y + ball.radius * 2 >= enemy.y && 
                     ball.y <= enemy.y + enemy.sizeY)
            {
                myAudioManager.Play("Pong_Hit");
                ballSpeedX = -ballSpeedX;
                ball.x = enemy.x - ball.radius * 2;
            }
            //Comprueba si el balon sale por los lados (goles)
            if (ball.x < 0) //Lado del jugador
            {
                myAudioManager.Play("Score"); //Sonido de puntuacion.
                enemyScore++; //Aumenta la puntuacion del enemigo.
                Console.WriteLine("Enemy Score: " + enemyScore);
                //Reinicia la posicion del balon al centro de la pantalla.
                ball.x = 400;
                ball.y = 300;
                //Reiniciar las velocidades del balon
                ballSpeedX = 400f;
                ballSpeedY = 400f;
            }
            else if (ball.x > 800) //Lado del enemigo
            {
                myAudioManager.Play("Score");
                playerScore++; //Aumenta la puntuacion del jugador
                Console.WriteLine("Player Score: " + playerScore);

                //Reinicia la posición del balon al centro de la pantalla    
                ball.x = 400;
                ball.y = 300;
                
                //Invierte direccion hacia el enemigo
                ballSpeedX = 400f * -1;
                ballSpeedY = 400f * -1;
            }

            enemy.y = ball.y * 0.7f; //Mueve al enemigo hacia la posicion del balon.
            if (enemy.y < 0) //Se asegura de que el enemigo no se salga de la pantalla.
            {
                enemy.y = 0;
            }
            else if (enemy.y > 600 - enemy.sizeY)
            {
                enemy.y = 600 - enemy.sizeY;
            }
        }
    }
}
