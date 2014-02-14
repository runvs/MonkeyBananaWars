using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Graphics;
using SFML.Window;

namespace JamTemplate
{
    public class Landscape
    {

        Dictionary<float, float> _heightMap;
        private float _landscapeSpacing = 10.0f;
        public Landscape()
        {
            _heightMap = new Dictionary<float, float>();
            for (float xval = 0.0f; xval <= GameProperties.WindowSize.X; xval += _landscapeSpacing)
            {
                float yval = Gauss(xval);
                _heightMap.Add(xval, yval);
                Console.WriteLine(xval + "    " +  yval);

            }
        }

        private float mu = 400.0f;
        private float sigma = 75.0f;
        private float offset = 20.0f;
        private float scaling = 12000.0f;

        private float Gauss(float xval)
        {
            return offset + scaling * (float)((1/sigma*Math.Sqrt(2*Math.PI)) * Math.Exp(-0.5*((xval-mu)/sigma)*(xval-mu)/sigma));
        }


        public void Draw (RenderWindow rw)
        {
            int i = 0;
            foreach (var v in _heightMap)
            {
                RectangleShape shape = new RectangleShape(new Vector2f(_landscapeSpacing, v.Value));
                shape.Origin = new Vector2f(0, v.Value);

                shape.Position = new Vector2f(i * _landscapeSpacing, 600.0f);
                shape.FillColor = GameProperties.Color05;
                rw.Draw(shape);
                i++;
            }
        }





    }
}
