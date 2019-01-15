using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace Spiders
{
    public class SpawnSystem : UpdateSystem
    {
        private readonly EntityLoader _entityLoader;

        public SpawnSystem(World world, ContentManager contentManager)
        {
            _entityLoader = new EntityLoader(world, contentManager, "Entities");
        }

        public override void Initialize(World world)
        {
            _entityLoader.Load("spider");
            base.Initialize(world);

        }

        public override void Update(GameTime gameTime)
        {
        }
    }
}