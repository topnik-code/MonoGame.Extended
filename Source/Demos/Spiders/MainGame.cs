using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;

namespace Spiders
{
    public class MainGame : Game
    {
        // ReSharper disable once NotAccessedField.Local
        private GraphicsDeviceManager _graphicsDeviceManager;
        private World _world;

        public MainGame()
        {
            _graphicsDeviceManager = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 800,
                PreferredBackBufferHeight = 480
            };
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void LoadContent()
        {
            _world = new WorldBuilder()
                .AddSystem(world => new RenderSystem(GraphicsDevice))
                .AddSystem(world => new SpawnSystem(world, Content))
                .Build();

            Components.Add(_world);
        }
    }
}
