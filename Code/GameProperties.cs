using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JamUtilities;
using SFML.Graphics;
using SFML.Window;

namespace JamTemplate
{
    public static class GameProperties
    {
        #region Color Stuff

        public static Color Color01 { get { return new Color(255,255,153); } }
        public static Color Color02 { get { return new Color(235, 231, 137); } }
        public static Color Color03 { get { return new Color(215, 207, 121); } }
        public static Color Color04 { get { return new Color(195,183,105); } }
        public static Color Color05 { get { return new Color(175, 159, 89); } }
        public static Color Color06 { get { return new Color(155, 136, 74); } }
        public static Color Color07 { get { return new Color(135, 112, 58); } }
        public static Color Color08 { get { return new Color(115, 88, 42); } }
        public static Color Color09 { get { return new Color(95, 64, 26); } }
        public static Color Color10 { get { return new Color(75, 40, 10); } }
        
        #endregion Color Stuff

        public static Vector2f WindowSize { get{return new Vector2f(800.0f, 600.0f);} }

        public static Vector2f GravitationalAcceleration { get { return new Vector2f(0,90.0f); } }
        public static float AirFrictionCoefficient { get { return 0.99995f; } }

        public static float ShootStrengthIncrement { get { return 5.0f; } }
        public static float ShootAngleIncrement { get { return 1.5f; } }

        public static float LandScapeDecimator { get { return 10.5f; } }

        public static float SetupTimerMax { get { return 2.1f; } }

        public static float WindChangeTimer { get { return 8.0f; } }

        public static float MaxWindSpeedAcceleration { get { return 15; } }

        public static float FlashTimerMax { get { return 6.4f; } }

        public static int FlashMaxXDistance { get {return 40;}}
    }
}
