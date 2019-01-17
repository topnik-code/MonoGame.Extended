using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Animations.Animators;
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
        
        public override void Update(GameTime gameTime)
        {
            foreach (var entity in ActiveEntities)
            {
                var sprite = _spriteMapper.Get(entity);
                var animator = _animatorMapper.Get(entity);
                
                var frameIndex = animator.Animations["walk"].CurrentFrame;
                
                sprite.TextureRegion = animator.TextureAtlas[frameIndex];

                animator.Animations["walk"].Update(gameTime);
            }
        }
    }
}