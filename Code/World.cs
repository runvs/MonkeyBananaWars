using SFML.Graphics;
using System;
using JamUtilities;
using SFML.Window;
using JamUtilities.Particles;

namespace JamTemplate
{
    public class World
    {

        #region Fields

        Landscape _landscape;
        public Player _p1;
        public Player _p2;



        System.Collections.Generic.List<Banana> _bananaList;
        System.Collections.Generic.List<AreatricCloud> _cloudList;

        float _setUpTimer;
        float _setUpTimerMax = GameProperties.SetupTimerMax;

        Vector2f _windAcceleration;
        private float _windChangeTimer;

        #endregion Fields

        #region Methods

        public World()
        {
            InitGame();
        }

        public Vector2f GetWindAcceleration()
        {
            return _windAcceleration;
        }

        public void GetInput()
        {

            if (Keyboard.IsKeyPressed(Keyboard.Key.L))
            {
                _p1.IsDead = true;
            }

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

            if (_setUpTimer <= _setUpTimerMax)
            {
                
                _p1.Position = new Vector2f(_p1.Position.X,  (float)PennerDoubleAnimation.GetValue(PennerDoubleAnimation.EquationType.BounceEaseOut, _setUpTimer-0.1f, 0, GetHeightAtPosition(_p1.Position.X), _setUpTimerMax));
                _p2.Position = new Vector2f(_p2.Position.X, (float)PennerDoubleAnimation.GetValue(PennerDoubleAnimation.EquationType.BounceEaseOut, _setUpTimer, 0, GetHeightAtPosition(_p2.Position.X), _setUpTimerMax));
                //_p1.Sprite.Scale(1 + 0.5f * (float)PennerDoubleAnimation.GetValue(PennerDoubleAnimation.EquationType.BounceEaseOut, _setUpTimer - 0.1f, 0,1, _setUpTimerMax));
                _setUpTimer += deltaT;
                
            }
            else
            {
                _windChangeTimer -= deltaT;
                if (_windChangeTimer <= 0.0f)
                {
                    ChangeWindAcceleration();
                    _windChangeTimer += GameProperties.WindChangeTimer;
                }


                _p1.Position = new Vector2f(_p1.Position.X, GetHeightAtPosition(_p1.Position.X));
                _p2.Position = new Vector2f(_p2.Position.X, GetHeightAtPosition(_p2.Position.X));
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

                        CheckIfHitPlayer(b);

                    }

                    if (b.IsAlive)
                    {
                        newBananaList.Add(b);
                    }
                }
                _bananaList = newBananaList;
            }

            ParticleManager.Update(deltaT);
        }

        private void CheckIfHitPlayer(Banana b)
        {
            if (SFMLCollision.Collision.BoundingBoxTest(b.Sprite, _p1.Sprite.Sprite))
            {
                _p1.IsDead = true;
            }
            if (SFMLCollision.Collision.BoundingBoxTest(b.Sprite, _p2.Sprite.Sprite))
            {
                _p2.IsDead = true;
            }
        }

        public bool IsGameOver()
        {
            bool ret = false;
            if (_p1.IsDead || _p2.IsDead)
            {
                WorldScore = new Score(this);
                ret = true;
            }

            return ret;
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
            DrawWindStrengthHudInfo(rw);

            ScreenEffects.DrawFadeRadial(rw);
        }


        private void DrawWindStrengthHudInfo(RenderWindow rw)
        {
            Vector2f position = new Vector2f(400, 125);
            //SmartText.DrawText("Wind " + GetWindAcceleration().ToString(), TextAlignment.MID, position, rw);

            float xspan = GetWindAcceleration().X /GameProperties.MaxWindSpeedAcceleration * 175.0f;



            RectangleShape shape = new RectangleShape(new Vector2f(xspan , 28));
            shape.Origin = new Vector2f(0, 28 / 2);
            shape.FillColor = GameProperties.Color01;
            shape.Position = new Vector2f(400, 15+15);
            rw.Draw(shape);


            shape = new RectangleShape(new Vector2f(xspan/1.25f, 24));
            shape.Origin = new Vector2f(0, 24 / 2);
            shape.FillColor = GameProperties.Color02;
            shape.Position = new Vector2f(400, 15+15);
            rw.Draw(shape);


            shape = new RectangleShape(new Vector2f(xspan / 2.0f, 18));
            shape.Origin = new Vector2f(0, 18 / 2);
            shape.FillColor = GameProperties.Color03;
            shape.Position = new Vector2f(400, 15+15);
            rw.Draw(shape);

            shape = new RectangleShape(new Vector2f(xspan / 2.75f, 16));
            shape.Origin = new Vector2f(0, 16 / 2);
            shape.FillColor = GameProperties.Color04;
            shape.Position = new Vector2f(400, 15 + 15);
            rw.Draw(shape);


            shape = new RectangleShape(new Vector2f(2, 30));
            shape.FillColor = GameProperties.Color05;

            shape.Position = new Vector2f(400, 15);
            rw.Draw(shape);


        }

        public void SwitchActivePlayer()
        {
            _p1.IsPlayerActive = !_p1.IsPlayerActive;
            _p2.IsPlayerActive = !_p2.IsPlayerActive;
        }

        public void ChangeWindAcceleration()
        {
            _windAcceleration = RandomGenerator.GetRandomVector2fSquare(GameProperties.MaxWindSpeedAcceleration);
            _windAcceleration.Y = 0;
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


        public Score WorldScore { get; private set; }
    }
}
