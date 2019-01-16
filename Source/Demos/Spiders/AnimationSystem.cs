using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using MonoGame.Extended.Sprites;

namespace Spiders
{
    public class AnimationSystem : EntityUpdateSystem
    {
        private ComponentMapper<Sprite> _spriteMapper;
        private ComponentMapper<Animator> _animatorMapper;

        public AnimationSystem() 
            : base(Aspect.All(typeof(Sprite), typeof(Animator)))
        {
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _spriteMapper = mapperService.GetMapper<Sprite>();
            _animatorMapper = mapperService.GetMapper<Animator>();
        }

        private float _currentFrame;

        public override void Update(GameTime gameTime)
        {
            // TODO: This is a total hackfest.
            _currentFrame += gameTime.GetElapsedSeconds() * 5;

            foreach (var entity in ActiveEntities)
            {
                var sprite = _spriteMapper.Get(entity);
                var animator = _animatorMapper.Get(entity);
                
                var frameIndex = animator.Animations[0].Frames[(int)_currentFrame];
                sprite.TextureRegion = animator.TextureAtlas[frameIndex];

                if (_currentFrame > animator.Animations[0].Frames.Count - 1)
                    _currentFrame = 0;
            }
        }
    }
}