using SFML.Graphics;
using System;
using JamUtilities;

namespace JamTemplate
{
    public class World
    {

        #region Fields

        Landscape _landscape;
        Player _p1;
        Player _p2;

        System.Collections.Generic.List<Banana> _bananaList;

        #endregion Fields

        #region Methods

        public World()
        {
            InitGame();
        }

        public void GetInput()
        {
            _p1.GetInput();
            _p2.GetInput();
        }

        public void Update(float deltaT)
        {
            _p1.Update(deltaT);
            _p2.Update(deltaT);

            System.Collections.Generic.List<Banana> newBananaList = new System.Collections.Generic.List<Banana>();
            foreach (var b in _bananaList)
            {
                b.Update(deltaT);

                if (b.Position.Y >= GetHeightAtPosition(b.Position.X))
                {
                    b.IsAlive = false;
                    _landscape.DamageLandscape(b.Position.X);
                }

                if (b.IsAlive)
                {
                    newBananaList.Add(b);
                }
            }
            _bananaList = newBananaList;

        }

        public void Draw(RenderWindow rw)
        {
            rw.Clear(GameProperties.Color10);
            ScreenEffects.DrawFadeUp(rw);
            _landscape.Draw(rw);

            _p1.Draw(rw);
            _p2.Draw(rw);

            foreach (var b in _bananaList)
            {
                b.Draw(rw);
            }


            _p1.DrawPlayerShotProperties(rw);
            _p2.DrawPlayerShotProperties(rw);
        }

        public void SwitchActivePlayer()
        {
            _p1.IsPlayerActive = !_p1.IsPlayerActive;
            _p2.IsPlayerActive = !_p2.IsPlayerActive;
        }

        public float GetHeightAtPosition(float xval)
        {
            return 600.0f - _landscape.GetHeightAtPosition(xval);
        }

        private void InitGame()
        {
            _landscape = new Landscape();
            _p1 = new Player(this, 1);
            _p2 = new Player(this, 2);
            _p1.IsPlayerActive = true;

            _bananaList = new System.Collections.Generic.List<Banana>();
        }

        public void AddBanana (Banana b)
        {
            if (b != null)
            {
                _bananaList.Add(b);
            }
        }

        #endregion Methods

    }
}
