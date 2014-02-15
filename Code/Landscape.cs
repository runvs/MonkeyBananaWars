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
        public float Spacing { get; private set; }
        public Landscape()
        {
            Spacing = 2.0f;
            _heightMap = new Dictionary<float, float>();
            for (float xval = 0.0f; xval <= GameProperties.WindowSize.X; xval += Spacing)
            {
                float yval = Gauss(xval);
                _heightMap.Add(xval, yval);
                //Console.WriteLine(xval + "    " +  yval);

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
                RectangleShape shape = new RectangleShape(new Vector2f(Spacing, v.Value));
                shape.Origin = new Vector2f(0, v.Value);

                shape.Position = new Vector2f(i * Spacing, 600.0f);
                shape.FillColor = GameProperties.Color07;
                rw.Draw(shape);
                i++;
            }
        }


        public float GetHeightAtPosition(float xval)
        {
            float lastxval = 0.0f;
            float lastyval = 0.0f;
            float newxval = 0.0f;
            float newyval = 0.0f;
            foreach (var v in _heightMap)
            {
                if (v.Key < xval)
                {
                    lastxval = v.Key;
                    lastyval = v.Value;
                }
                else
                {
                    newxval = v.Key;
                    newyval = v.Value;
                    break;
                }
            }

            // TODO implement interpolation if needed
            return lastyval;

        }

        internal void DamageLandscape(float xval)
        {
            float lastxval = 0.0f;
            foreach (var v in _heightMap)
            {
                if (v.Key < xval)
                {
                     lastxval = v.Key;
                }
                else
                {
                    break;
                }
            }
            try
            {
                _heightMap[lastxval] -= GameProperties.LandScapeDecimator;
                _heightMap[lastxval - Spacing] -= GameProperties.LandScapeDecimator / 2.0f;
                _heightMap[lastxval + Spacing] -= GameProperties.LandScapeDecimator / 2.0f;
            }
            catch (KeyNotFoundException e)
            {

            }
        }
    }
}
