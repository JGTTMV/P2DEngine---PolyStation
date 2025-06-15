using P2DEngine.Games;
using P2DEngine.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace P2DEngine
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Ancho y alto de la ventana.
            int windowWidth = 800;
            int windowHeight = 600;

            // Ancho y alto de la cámara.
            int camWidth = 800;
            int camHeight = 600;

            // Frames por segundo.
            int FPS = 60;

            //Cargando assets de los juegos
            myFontManager.Load("CourierPrime-Bold.ttf", "GameFont");
            myAudioManager.Load("Blood Machine - Switch.wav", "Scene_Change");
            myAudioManager.Load("Blood Machine - Beep.wav", "Scene_Load");
            myImageManager.Load("PolyStation_Background.png", "PolyStation_Background");

            //Cargando assets del menu

            //Cargando assets del Pong
            myImageManager.Load("Pong_Background.jpg", "Pong_Background");
            myAudioManager.Load("Shin Megami Tensei - Kichijoji PSX.mp3", "Pong_Music");
            myImageManager.Load("Pong_Player.png", "Pong_Player");
            myImageManager.Load("Pong_Enemy.png", "Pong_Enemy");
            myImageManager.Load("Pong_Ball.png", "Pong_Ball");
            myAudioManager.Load("Curtis - Point Score.wav", "Score");
            myAudioManager.Load("Cancel 2.wav", "Pong_Hit");

            //Cargando assets del Arkanoid
            myImageManager.Load("Arkanoid_Background.jpg", "Arkanoid_Background");
            myImageManager.Load("Arkanoid_Player.png", "Arkanoid_Player");
            myImageManager.Load("Arkanoid_Ball.png", "Arkanoid_Ball");
            myImageManager.Load("Arkanoid_Brick1.png", "Brick1");
            myImageManager.Load("Arkanoid_Brick2.png", "Brick2");
            myImageManager.Load("Arkanoid_Brick3.png", "Brick3");
            myAudioManager.Load("Shin Megami Tensei_ Devil Summoner OST - Battle theme.mp3", "Arkanoid_Music");
            myAudioManager.Load("Cruch.wav", "Cruch");
            myAudioManager.Load("Boop.wav", "Boop");
            myAudioManager.Load("Healing - Moving Blood.wav", "GameOver");

            //Cargando assets del Agario
            //myImageManager.Load("Agario_Background.jpg", "Agario_Background");
            myImageManager.Load("Agario_Player.png", "Agario_Player");
            myImageManager.Load("Agario_Block.png", "Agario_Block");
            myImageManager.Load("Agario_Enemy.png", "Agario_Enemy");
            myImageManager.Load("Agario_Food.png", "Agario_Food");

            PolyStation game = new PolyStation(windowWidth, windowHeight, FPS, new myCamera(0, 0, camWidth, camHeight, 
                (float)windowWidth/(float)camWidth));

            game.Start();
            
            // Esto es propio de WinForms, es básicamente para que la ventana fluya.
            Application.Run();
        }
    }
}
