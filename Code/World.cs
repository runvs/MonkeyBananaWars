using SFML.Graphics;
using System;
using JamUtilities;
using SFML.Window;
using JamUtilities.Particles;
using SFML.Audio;

namespace JamTemplate
{
    public class World
    {

        #region Fields

        Landscape _landscape;
        public Player _p1;
        public Player _p2;

        private float _flashTimer = 0.0f;

        private Music _rainSample1;
        private Music _rainSample2;

        private SoundBuffer _flashSoundBuffer;
        private Sound _flashSound;

        private SoundBuffer _hitSoundBuffer;
        private Sound _hitSound;

        System.Collections.Generic.List<Banana> _bananaList;
        System.Collections.Generic.List<AreatricCloud> _cloudList;

        System.Collections.Generic.List<RectangleShape> _flashShapeList;

        float _setUpTimer;
        float _setUpTimerMax = GameProperties.SetupTimerMax;

        Vector2f _windAcceleration;
        private float _windChangeTimer;

        private AccelerationArea _rainAccelerationArea;
        private float _totalTime;

        #endregion Fields

        #region Methods

        public World()
        {
            LoadSounds();
            InitGame();
        }

        private void LoadSounds()
        {
            try
            {
                _rainSample1 = new Music("../SFX/rain.ogg");
                _rainSample2 = new Music("../SFX/rain.ogg");

                _flashSoundBuffer = new SoundBuffer("../SFX/flash.ogg");
                _flashSound = new Sound(_flashSoundBuffer);
                _flashSound.Volume = 35.0f;

                _hitSoundBuffer = new SoundBuffer("../SFX/hit.wav");
                _hitSound = new Sound(_hitSoundBuffer);

            }
            catch (SFML.LoadingFailedException e)
            {
                Console.WriteLine("Error loading the rain sounds.\n" + e.ToString() );
            }
            _rainSample1.Loop = true;
            _rainSample2.Loop = true;
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

            _flashTimer -= deltaT;
            if (_flashTimer <= 0.0f)
            {
                Flash();
                _flashTimer += GameProperties.FlashTimerMax;   
            }

            float  newVal = (float) (150.0f - PennerDoubleAnimation.GetValue(PennerDoubleAnimation.EquationType.QuintEaseIn, GameProperties.FlashTimerMax - _flashTimer, 0, 255, GameProperties.FlashTimerMax)); ;

            if (newVal <= 30.0f)
            {
                _flashShapeList.Clear();
            }

            //Console.WriteLine(newVal);
            foreach (var s in _flashShapeList)
            {
                Color col = s.FillColor;
                col.A = (byte)newVal;
                s.FillColor = col;
            }

            _totalTime += deltaT;

            if (_rainSample2.Status != SoundStatus.Playing && _totalTime >= 45 )
            { 
                _rainSample2.Play(); 
            }

            ScreenEffects.Update(deltaT);
            SpriteTrail.Update(deltaT);


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

                        
                        DamageLandScape(b.Position);

                        _hitSound.Play();

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

        private void DamageLandScape(Vector2f pos)
        {
            _landscape.DamageLandscape(pos.X);
            ParticleManager.SpawnMultipleDebris(pos , 170, GameProperties.Color02, 5, 0.25f);
            ParticleManager.SpawnSmokeCloud(pos, 17.5f, 5.0f, GameProperties.Color06);
        }

        private void Flash()
        {
            _flashSound.Play();
            Color col = GameProperties.Color03;
            col.A = 125;

            ScreenEffects.ScreenFlash(col, 0.5f);

            Vector2f startingPos = RandomGenerator.GetRandomVector2fInRect(new FloatRect(0, -10, 800, 10));

            Timing.Pause(0.09f);

            bool end = false;
            for (float y = 0; y <= 600;  )
            {
                Vector2f endingPos = new Vector2f(startingPos.X + RandomGenerator.Random.Next(-25, 25), y);
                if (endingPos.Y >= GetHeightAtPosition(endingPos.X))
                {
                    endingPos.Y = GetHeightAtPosition(endingPos.X);
                    DamageLandScape(endingPos);
                    end = true;
                }
                RectangleShape shape;
                LineCreator.CreateLine(out shape, startingPos, endingPos);
                _flashShapeList.Add(shape);
                startingPos = endingPos;
                y += 100;


                if (y > 300)
                {
                    y -= 20;
                    endingPos = new Vector2f(startingPos.X + RandomGenerator.Random.Next(-GameProperties.FlashMaxXDistance, GameProperties.FlashMaxXDistance), y);
                    if (endingPos.Y >= GetHeightAtPosition(endingPos.X))
                    {
                        endingPos.Y = GetHeightAtPosition(endingPos.X);
                        DamageLandScape(endingPos);
                        end = true;
                    }
                    LineCreator.CreateLine(out shape, startingPos, endingPos);
                    _flashShapeList.Add(shape);

                }
                if (y > 500)
                {
                    y -= 20;
                }

                if (end)
                {
                    return;
                }
                
            }


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


            SpriteTrail.Draw(rw);
            foreach (var b in _bananaList)
            {
                b.Draw(rw);
            }
            


            ParticleManager.Draw(rw);



            _p1.DrawPlayerShotProperties(rw);
            _p2.DrawPlayerShotProperties(rw);
            _p1.DrawPlayerAimingLine(rw);
            _p2.DrawPlayerAimingLine(rw);
            DrawWindStrengthHudInfo(rw);

            foreach (var s in _flashShapeList)
            {
                rw.Draw(s);
            }


            ScreenEffects.DrawFadeRadial(rw);
            ScreenEffects.Draw(rw);
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
            _rainAccelerationArea.Acceleration = _windAcceleration*10.0f;
        }

        public float GetHeightAtPosition(float xval)
        {
            return 600.0f - _landscape.GetHeightAtPosition(xval);
        }

        private void InitGame()
        {

            _rainSample1.Play();

            ParticleManager.ResetParticleSystem();
            ScreenEffects.ResetScreenEffects();

            _landscape = new Landscape();
            _p1 = new Player(this, 1);
            _p2 = new Player(this, 2);
            _p1.IsPlayerActive = true;
            
            _bananaList = new System.Collections.Generic.List<Banana>();
            _rainAccelerationArea = new AccelerationArea(new FloatRect(-500, 0, 1500, 600), new Vector2f(0, 0));
            _flashShapeList = new System.Collections.Generic.List<RectangleShape>(); 
            ParticleManager.AddAccelerationArea(_rainAccelerationArea);

             
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
