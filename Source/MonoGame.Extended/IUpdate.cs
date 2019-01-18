using Microsoft.Xna.Framework;

namespace MonoGame.Extended
{
    public interface IUpdate
    {
        void Update(float elapsedSeconds);
    }

    public interface IDraw
    {
        void Draw(float elapsedSeconds);
    }

    public static class UpdateExtensions
    {
        public static void Update(this IUpdate updateable, GameTime gameTime)
        {
            updateable.Update(gameTime.GetElapsedSeconds());
        }
    }

    public static class DrawExtensions
    {
        public static void Draw(this IDraw drawable, GameTime gameTime)
        {
            drawable.Draw(gameTime.GetElapsedSeconds());
        }
    }
}