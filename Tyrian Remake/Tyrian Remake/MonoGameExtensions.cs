using System;
using System.Reflection;
using Microsoft.Xna.Framework;

namespace Tyrian_Remake
{
    public static class MonoGameExtensions
    {
        public static void SetPosition(this GameWindow window, Point position)
        {
            var otkWindow = GetForm(window);
            if (otkWindow == null) return;
            otkWindow.X = position.X;
            otkWindow.Y = position.Y;
        }

        public static OpenTK.GameWindow GetForm(this GameWindow gameWindow)
        {
            Type type = typeof(OpenTKGameWindow);
            FieldInfo field = type.GetField("window", BindingFlags.NonPublic | BindingFlags.Instance);
            if (field != null)
                return field.GetValue(gameWindow) as OpenTK.GameWindow;
            return null;
        }
    }
}
