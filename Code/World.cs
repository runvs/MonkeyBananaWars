using SFML.Graphics;
using System;
using JamUtilities;
using SFML.Window;

namespace JamTemplate
{
    public class World
    {

        #region Fields

        Landscape _landscape;
        Player _p1;
        Player _p2;



        System.Collections.Generic.List<Banana> _bananaList;
        System.Collections.Generic.List<AreatricCloud> _cloudList;

        #endregion Fields

        #region Methods

        public World()
        {
            InitGame();
        }

        public void GetInput()
        {
            if (_p1.IsPlayerActive)
            {
                _p1.GetInput();
            }
            else
            {
                _p2.GetInput();
            }
        }

        public void Update(float deltaT)
        {
            if (_p1.IsPlayerActive)
            {
                _p1.Update(deltaT);
            }
            else
            {
                _p2.Update(deltaT);
            }
            

            System.Collections.Generic.List<Banana> newBananaList = new System.Collections.Generic.List<Banana>();
            foreach (var b in _bananaList)
            {
                b.Update(deltaT);

                if (b.Position.Y >= GetHeightAtPosition(b.Position.X))
                {
                    b.IsAlive = false;
                    _landscape.DamageLandscape(b.Position.X);
                    ParticleManager.SpawnMultipleDebris(b.Position, 170, GameProperties.Color02, 5, 0.25f);
                    ParticleManager.SpawnSmokeCloud(b.Position, 17.5f, 5.0f, GameProperties.Color09);
                }

                if (b.IsAlive)
                {
                    newBananaList.Add(b);
                }
            }
            _bananaList = newBananaList;

            ParticleManager.Update(deltaT);
        }

        public void Draw(RenderWindow rw)
        {
            rw.Clear(GameProperties.Color10);

            foreach (var ac in _cloudList)
            {
                ac.Draw(rw);
            }

            ScreenEffects.DrawFadeUp(rw);
            


            _landscape.Draw(rw);

            _p1.Draw(rw);
            _p2.Draw(rw);



            foreach (var b in _bananaList)
            {
                b.Draw(rw);
            }



            ParticleManager.Draw(rw);



            _p1.DrawPlayerShotProperties(rw);
            _p2.DrawPlayerShotProperties(rw);

            ScreenEffects.DrawFadeRadial(rw);


        }

        public void SwitchActivePlayer()
        {
            _p1.IsPlayerActive = !_p1.IsPlayerActive;
            _p2.IsPlayerActive = !_p2.IsPlayerActive;

            Console.WriteLine(_p1.IsPlayerActive + " " + _p2.IsPlayerActive);
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
             
            _cloudList = new System.Collections.Generic.List<AreatricCloud>();
            for (int i = 0; i != 15; i++ )
            {
                AreatricCloud ac = new AreatricCloud(RandomGenerator.GetRandomVector2f(new Vector2f(0, 800), new Vector2f(0, 600)), GameProperties.Color08);
                _cloudList.Add(ac);
            }
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
