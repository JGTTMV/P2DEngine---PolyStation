using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using P2DEngine.GameObjects;
using P2DEngine.GameObjects.Collisions;
using P2DEngine.Games.SceneGame;
using P2DEngine.Managers;

namespace P2DEngine.Games
{
    public class PolyStation : myGame
    {
        // Ahora aquí en las clase de juego se deben crear y registrar las escenas correspondientes.
        public PolyStation(int width, int height, int FPS, myCamera c) : base(width, height, FPS, c)
        {
            Menu myMenu = new Menu(c);
            Pong p = new Pong(c);
            Arkanoid a = new Arkanoid(c);
            Agario a2 = new Agario(c);

            mySceneManager.Register(myMenu, "Menu");
            mySceneManager.Register(p, "Pong");
            mySceneManager.Register(a, "Arkanoid");
            mySceneManager.Register(a2, "Agario");


            mySceneManager.SetActive("Menu");
        }
    }
}
