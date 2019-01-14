using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.TextureAtlases;

namespace Spiders
{
    public class SpawnSystem : UpdateSystem
    {
        private readonly ContentManager _contentManager;

        public SpawnSystem(ContentManager contentManager)
        {
            _contentManager = contentManager;
        }

        public override void Initialize(World world)
        {
            base.Initialize(world);

            var texture = _contentManager.Load<Texture2D>("elthen-spider");

            var entity = world.CreateEntity();
            entity.Attach(new Sprite(new TextureRegion2D(texture, 0, 0, 32, 32)));
        }

        public override void Update(GameTime gameTime)
        {
        }
    }
}