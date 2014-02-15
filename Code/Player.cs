using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using JamUtilities;

namespace JamTemplate
{
    public class Player
    {

        #region Fields

        public int _playerNumber;
        public string PlayerName { get; private set; }
        private SmartSprite _sprite;

        public Vector2f Position { get; set; }

        Dictionary<Keyboard.Key, Action> _actionMap;
        private float _inputTimer = 0.0f; // time between two successive movement commands
        private World _world;
        public bool IsPlayerActive { get { return _isPlayerActive; } set { _isPlayerActive = value; _inputTimer += 0.25f; if (value == true) _remainingShots = 2; } }
        private bool _isPlayerActive;

        private float _shootAngle =45.0f;
        private float _shootStrength = 200.0f;
        private int _remainingShots = 2;

        public Sprite Sprite { get { return _sprite.Sprite; } }

        #endregion Fields

        #region Methods

        public Player(World world, int number)
        {
            IsPlayerActive = false;
            _world = world;
            _playerNumber = number;

            _actionMap = new Dictionary<Keyboard.Key, Action>();
            SetupActionMap();

            try
            {
                LoadGraphics();
            }
            catch (SFML.LoadingFailedException e)
            {
                System.Console.Out.WriteLine("Error loading player Graphics.");
                System.Console.Out.WriteLine(e.ToString());
            }

            SetPosition();


        }

        private void SetPosition()
        {
            int lower = 0;
            int upper = 800;
            if(_playerNumber == 1)
            {
                lower = 75;
                upper = 250;
            }
            if (_playerNumber == 2)
            {
                lower = 550;
                upper = 725;
            }

            float posX = RandomGenerator.Random.Next(lower, upper);
            
            Position = new Vector2f(posX, 0);
        }

        private void SetPlayerNumberDependendProperties()
        {
            PlayerName = "Player" + _playerNumber.ToString();
        }

        public void GetInput()
        {
            if (IsPlayerActive)
            {
                if (_inputTimer <= 0.0f)
                {
                    MapInputToActions();
                }
            }
        }


        private void Shoot()
        {

            float angle = 0.0f;
            Vector2f vel = new Vector2f();
            if (_playerNumber == 1)
            {
                angle = (float)(_shootAngle * Math.PI / 180.0f);
                //Console.WriteLine(_shootAngle);
                vel = new Vector2f((float)Math.Cos(angle), -(float)Math.Sin(angle)) * _shootStrength;
            }
            else if (_playerNumber == 2)
            {

                angle = -(float)((180 - _shootAngle) * Math.PI / 180.0f);
                //Console.WriteLine(180 - _shootAngle);
                vel = new Vector2f((float)Math.Cos(angle), (float)Math.Sin(angle)) * _shootStrength;
            }
             
            Banana b = new Banana(_world, Position + new Vector2f(0, -45.0f), vel );

            _world.AddBanana(b);
            _remainingShots--;
        }

        private void IncreaseShootStrength()
        {
            _shootStrength += GameProperties.ShootStrengthIncrement;
            if (_shootStrength >= 500)
            {
                _shootStrength = 500;
            }
        }
        private void DecreaseShootStrength()
        {
            _shootStrength -= GameProperties.ShootStrengthIncrement;
            if (_shootStrength <= 10)
            {
                _shootStrength = 10;
            }
        }

        private void IncreaseShootAngle()
        {
            _shootAngle += GameProperties.ShootAngleIncrement * ((_playerNumber==1)? 1 : -1);
            CheckShootAngle();
        }


        private void DecreaseShootAngle()
        {
            _shootAngle -= GameProperties.ShootAngleIncrement * ((_playerNumber == 1) ? 1 : -1); ;
            CheckShootAngle();  
        }

        private void CheckShootAngle()
        {
            if (_shootAngle <= 0)
            {
                _shootAngle = 0;
            }
            if (_shootAngle >= 90)
            {
                _shootAngle = 90;
            }
        }

        public void Update(float deltaT)
        {
            if (_inputTimer >= 0.0f)
            {
                _inputTimer -= deltaT;
            }
            _sprite.Update(deltaT);

            if (_remainingShots <= 0)
            {
                _world.SwitchActivePlayer();
            }
        }

        public void Draw(SFML.Graphics.RenderWindow rw)
        {
            _sprite.Position = Position;
            _sprite.Draw(rw);
        }

        public void DrawPlayerShotProperties(RenderWindow rw)
        {
            if (IsPlayerActive)
            {
                Vector2f position = new Vector2f();
                if (_playerNumber == 1)
                {
                    position = new Vector2f(10, 25);
                    SmartText.DrawText("Strength: " + _shootStrength, TextAlignment.LEFT, position, GameProperties.Color01, rw);
                    position = new Vector2f(10, 50);
                    SmartText.DrawText("Angle: " + _shootAngle, TextAlignment.LEFT, position, GameProperties.Color01, rw);
                    position = new Vector2f(10, 75);
                    SmartText.DrawText("Shots: " + _remainingShots, TextAlignment.LEFT, position, GameProperties.Color01, rw);
                }
                if (_playerNumber == 2)
                {
                    position = new Vector2f(790, 25);
                    SmartText.DrawText("Strength: " + _shootStrength, TextAlignment.RIGHT, position, GameProperties.Color01, rw);
                    position = new Vector2f(790, 50);
                    SmartText.DrawText("Angle: " + _shootAngle, TextAlignment.RIGHT, position, GameProperties.Color01, rw);
                    position = new Vector2f(790, 75);
                    SmartText.DrawText("Shots: " + _remainingShots, TextAlignment.RIGHT, position, GameProperties.Color01, rw);
                }
            }
        }

        private void SetupActionMap()
        {
            // e.g. _actionMap.Add(Keyboard.Key.Escape, ResetActionMap);
            _actionMap.Add(Keyboard.Key.Space, Shoot);
            _actionMap.Add(Keyboard.Key.Up, IncreaseShootStrength);
            _actionMap.Add(Keyboard.Key.Down, DecreaseShootStrength);

            _actionMap.Add(Keyboard.Key.Left, IncreaseShootAngle);
            _actionMap.Add(Keyboard.Key.Right, DecreaseShootAngle);
        }

        private void MapInputToActions()
        {
            foreach (var kvp in _actionMap)
            {
                if (Keyboard.IsKeyPressed(kvp.Key))
                {
                    // Execute the saved callback
                    kvp.Value();
                    _inputTimer += 0.125f;
                }
            }
        }

        private void LoadGraphics()
        {
           _sprite = new SmartSprite("../GFX/player.png");
           _sprite.Sprite.Origin = new Vector2f(_sprite.Sprite.GetLocalBounds().Width / 2, _sprite.Sprite.GetLocalBounds().Height);
        }

        #endregion Methods

    }
}
