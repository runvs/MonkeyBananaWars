using JamUtilities;
using SFML.Graphics;
using SFML.Window;

namespace JamTemplate
{
    public class Score
    {

        World _world;

        #region Fields

        #endregion Fields

        #region Methods



        public Score(World world)
        {
            _world = world;

            // TODO fill Entries
        }

        public void Draw(RenderWindow rw)
        {

            rw.Clear(GameProperties.Color10);

            SmartText.DrawText("Game Over", TextAlignment.MID, new Vector2f(400, 50), new Vector2f(1.25f, 1.25f), GameProperties.Color01, rw);
            if (_world._p2.IsDead)
            {
                SmartText.DrawText("Player 1 wins", TextAlignment.MID, new Vector2f(400, 100), new Vector2f(1.0f, 1.0f), GameProperties.Color01, rw);
            }
            if (_world._p1.IsDead)
            {
                SmartText.DrawText("Player 2 wins", TextAlignment.MID, new Vector2f(400, 100), new Vector2f(1.0f, 1.0f), GameProperties.Color01, rw);
            }



            float xposp1start = 100.0f;
            float xposp2start = 550.0f;
            float yposstart = 250.0f;

            float rectsize = 200.0f;


            SmartText.DrawText("Player 1", TextAlignment.LEFT, new SFML.Window.Vector2f(xposp1start, 200), GameProperties.Color01, rw);
            RectangleShape shape = new RectangleShape(new Vector2f(rectsize, rectsize));
            shape.FillColor = GameProperties.Color10;
            shape.OutlineColor= GameProperties.Color02;
            shape.OutlineThickness = 2;
            shape.Position = new Vector2f(xposp1start, yposstart);
            rw.Draw(shape);

            CircleShape circ = new CircleShape(2);
            Color col = GameProperties.Color03;
            col.A = 125;
            circ.FillColor = col;
            foreach (var xy in _world._p1._shothistory)
            {
                float x = xposp1start + (xy._x) / 90.0f * rectsize;
                float y = yposstart + (xy._y - 200.0f) / 200.0f * rectsize;
                circ.Position = new Vector2f(x, y);
                rw.Draw(circ);
            }

            SmartText.DrawText("angle", TextAlignment.LEFT, new SFML.Window.Vector2f(xposp1start + 70.0f, yposstart + 200), new Vector2f(0.75f, 0.75f), GameProperties.Color01, rw);
            SmartText.DrawText("strenght", TextAlignment.RIGHT, new SFML.Window.Vector2f(xposp1start - 5, yposstart+80), new Vector2f(0.75f, 0.75f), GameProperties.Color01, rw);


            SmartText.DrawText("Player 2", TextAlignment.LEFT, new SFML.Window.Vector2f(xposp2start, 200), GameProperties.Color01, rw);
            shape = new RectangleShape(new Vector2f(200, 200));
            shape.FillColor = GameProperties.Color10;
            shape.OutlineColor = GameProperties.Color01;
            shape.OutlineThickness = 2;
            shape.Position = new Vector2f(xposp2start, yposstart);
            rw.Draw(shape);

            foreach (var xy in _world._p2._shothistory)
            {
                float x = xposp2start + (xy._x) / 90.0f * rectsize;
                float y = yposstart + (xy._y - 200.0f) / 200.0f * rectsize;
                circ.Position = new Vector2f(x, y);
                rw.Draw(circ);
            }
            SmartText.DrawText("angle", TextAlignment.LEFT, new SFML.Window.Vector2f(xposp2start + 70.0f, yposstart + 200), new Vector2f(0.75f, 0.75f), GameProperties.Color01, rw);
            SmartText.DrawText("strenght", TextAlignment.RIGHT, new SFML.Window.Vector2f(xposp2start - 5, yposstart+80), new Vector2f(0.75f, 0.75f), GameProperties.Color01, rw);
        }

        #endregion Methods

    }
}
