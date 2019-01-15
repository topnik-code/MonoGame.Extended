using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using MonoGame.Extended.Sprites;

namespace Spiders
{
    public class RenderSystem : EntityDrawSystem
    {
        private readonly GraphicsDevice _graphicsDevice;
        private readonly SpriteBatch _spriteBatch;
        private ComponentMapper<Sprite> _spriteMapper;
        private ComponentMapper<Transform2> _transformMapper;

        public RenderSystem(GraphicsDevice graphicsDevice)
            : base(Aspect.All(typeof(Sprite), typeof(Transform2)))
        {
            _graphicsDevice = graphicsDevice;
            _spriteBatch = new SpriteBatch(graphicsDevice);
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _spriteMapper = mapperService.GetMapper<Sprite>();
            _transformMapper = mapperService.GetMapper<Transform2>();
        }

        public override void Draw(GameTime gameTime)
        {
            _graphicsDevice.Clear(Color.DarkOrange);
            
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: Matrix.CreateScale(1));

            foreach (var entity in ActiveEntities)
            {
                var transform = _transformMapper.Get(entity);
                var sprite = _spriteMapper.Get(entity);

                sprite.Draw(_spriteBatch, transform.Position, transform.Rotation, transform.Scale);
            }

            _spriteBatch.End();
        }

    }
}