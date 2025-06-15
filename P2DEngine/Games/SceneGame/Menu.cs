using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using P2DEngine.Managers;
using System.Windows.Forms;

namespace P2DEngine.Games.SceneGame
{
    public class Menu : myScene
    {
        private const float volume = 1.0f;
        public Menu(myCamera camera) : base(camera)
        {
        }
        public override void Init()
        {
            // Aquí podrías inicializar el menú, como cargar imágenes, sonidos, etc.
        }
        public override void ProcessInput()
        {
            if (myInputManager.IsKeyPressed(Keys.D1))
            {
                if (GlobalAudioState.PongMusicIndex != -1)
                {
                    myAudioManager.Stop(GlobalAudioState.PongMusicIndex);
                    GlobalAudioState.PongMusicIndex = -1;
                }
                GlobalAudioState.PongMusicIndex = myAudioManager.Play("Pong_Music", volume);

                mySceneManager.SetActive("Pong");
            }
            if (myInputManager.IsKeyPressed(Keys.D2))
            {
                if (GlobalAudioState.ArkanoidMusicIndex != -1)
                {
                    myAudioManager.Stop(GlobalAudioState.ArkanoidMusicIndex);
                    GlobalAudioState.ArkanoidMusicIndex = -1;
                }
                GlobalAudioState.ArkanoidMusicIndex = myAudioManager.Play("Arkanoid_Music", volume);

                mySceneManager.SetActive("Arkanoid");
            }
            if(myInputManager.IsKeyPressed(Keys.D3))
            {  
              /*if (GlobalAudioState.AgarioMusicIndex != -1)
                {
                    myAudioManager.Stop(GlobalAudioState.AgarioMusicIndex);
                    GlobalAudioState.AgarioMusicIndex = -1;
                }
                GlobalAudioState.AgarioMusicIndex = myAudioManager.Play("Agario_Music", volume);*/

                mySceneManager.SetActive("Agario");
            }
        }
        public override void Render(Graphics g)
        {
            // Aquí podrías dibujar el menú en la pantalla.
            g.DrawImage(myImageManager.Get("PolyStation_Background"), 0, 0, 800, 600);
        }
        public override void Update(float deltaTime)
        {
            // Aquí podrías actualizar cualquier lógica del menú si es necesario.
        }
    }

    public static class GlobalAudioState
    {
        public static int PongMusicIndex = -1;
        public static int ArkanoidMusicIndex = -1;
        // ...otros índices...
    }
}
