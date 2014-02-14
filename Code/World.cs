using SFML.Graphics;
using System;
using JamUtilities;

namespace JamTemplate
{
    class World
    {

        #region Fields

        Landscape _landscape;

        #endregion Fields

        #region Methods

        public World()
        {
            InitGame();
        }

        public void GetInput()
        {

        }

        public void Update(float deltaT)
        {

        }

        public void Draw(RenderWindow rw)
        {
            rw.Clear(GameProperties.Color10);
            ScreenEffects.DrawFadeUp(rw);
            _landscape.Draw(rw);
        }

        private void InitGame()
        {
            _landscape = new Landscape();
        }

        #endregion Methods

    }
}
