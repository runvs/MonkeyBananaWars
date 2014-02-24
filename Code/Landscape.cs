using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JamUtilities;
using JamUtilities.Particles;
using SFML.Graphics;
using SFML.Window;

namespace JamTemplate
{
    public class Landscape
    {

        Dictionary<float, float> _heightMap;
        public float Spacing { get; private set; }

        private List<Color> _colorList;
       

        public Landscape()
        {
            _colorList = ColorList.GetColorList( GameProperties.Color09, GameProperties.Color08,GameProperties.Color07,GameProperties.Color06, GameProperties.Color05);

            ParticleManager.CreateAreaRain(new FloatRect(-500, 0, 1500, 20), GameProperties.Color02);

            Spacing = 2.0f;
            _heightMap = new Dictionary<float, float>();
            for (float xval = 0.0f; xval <= GameProperties.WindowSize.X; xval += Spacing)
            {
                float yval = Gauss(xval) + Noise(xval);
                _heightMap.Add(xval, yval);
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
        private float freq1 = 25.0f;
        private float freq2 = 48;
        private float freq3 = 60;

        private float offs1 = 15;
        private float offs2 = 0;
        private float offs3 = 20;

        private float factor1 = 5;
        private float factor2 = 7;
        private float factor3 = 0;
        
        private float Noise(float xval)
        {
            return (float)(factor1 * Math.Sin((xval + offs1) / freq1) + factor2 * Math.Sin((xval + offs2) / freq2) + factor3 * Math.Sin((xval + offs3) / freq3));
        }


        public void Draw (RenderWindow rw)
        {
            int i = 0;
            foreach (var v in _heightMap)
            {
                RectangleShape shape = new RectangleShape(new Vector2f(Spacing, v.Value));
                shape.Origin = new Vector2f(0, v.Value);

                shape.Position = new Vector2f(i * Spacing, 600.0f);



                shape.FillColor = _colorList[(int)(v.Value/500 *_colorList.Count)%_colorList.Count];
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
