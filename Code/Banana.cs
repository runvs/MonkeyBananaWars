using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JamUtilities;
using SFML.Graphics;
using SFML.Window;

namespace JamTemplate
{
    public class Banana
    {
        public Vector2f Position { get; private set; }
        public Vector2f Velocity { get; private set; }

        private World _world;

        SmartSprite _sprite;


        public Banana (World world, Vector2f position, Vector2f velocity)
        {
            IsAlive = true;
            _world = world;
            Position = position;
            Velocity = velocity;

            try
            {
                LoadGraphics();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }



        public void Draw (RenderWindow rw)
        {
            _sprite.Position = Position;
            _sprite.Draw(rw);
        }

        public void Update (float deltaT)
        {
            Velocity = Velocity * GameProperties.FrictionCoefficient + GetAcceleration() * deltaT;
            Position = Position + Velocity * deltaT;

            _sprite.Rotation += deltaT* 100.0f;

            _sprite.Update(deltaT);
        }

        private Vector2f GetAcceleration()
        {
            return GameProperties.GravitationalAcceleration;
        }




        private void LoadGraphics()
        {
            _sprite = new SmartSprite("../GFX/banana.png");
            _sprite.Sprite.Origin = new Vector2f(_sprite.Sprite.GetLocalBounds().Width / 2.0f, _sprite.Sprite.GetLocalBounds().Height/2.0f);
        }

        public bool IsAlive { get; set; }
    }

}
