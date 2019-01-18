using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGame.Extended.TextureAtlases;

namespace MonoGame.Extended.Animations.Animators
{
    public class Animator : IUpdate
    {
        private KeyFrameAnimation _currentAnimation;
        private Action _onCompleted;

        public Animator(TextureAtlas textureAtlas)
        {
            TextureAtlas = textureAtlas;
            Animations = new Dictionary<string, KeyFrameAnimation>();
        }

        public TextureAtlas TextureAtlas { get; }
        public Dictionary<string, KeyFrameAnimation> Animations { get; }

        public KeyFrameAnimation Play(string name, Action onCompleted = null)
        {
            if (_currentAnimation == null || _currentAnimation.IsComplete)
            {
                _currentAnimation = Animations[name];

                if (_currentAnimation != null)
                    _onCompleted = onCompleted;
            }

            return _currentAnimation;
        }

        public void Update(float elapsedSeconds)
        {
            if (_currentAnimation != null && !_currentAnimation.IsComplete)
            {
                _currentAnimation.Update(elapsedSeconds);

                if (_currentAnimation.IsComplete)
                {
                    _onCompleted?.Invoke();
                    _onCompleted = null;
                }
            }
        }
    }
}