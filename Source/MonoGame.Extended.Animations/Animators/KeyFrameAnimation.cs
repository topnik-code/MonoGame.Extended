using System;
using System.Linq;
using Newtonsoft.Json;

namespace MonoGame.Extended.Animations.Animators
{
    public class KeyFrameAnimation : Animation
    {
        public const float DefaultFrameDuration = 0.2f;

        public KeyFrameAnimation(string name, int[] keyFrames)
            : base(name, false)
        {
            KeyFrames = keyFrames;
            KeyFrameIndex = IsReversed ? KeyFrames.Length - 1 : 0;
        }

        [JsonProperty]
        public int[] KeyFrames { get; }

        public float FrameDuration { get; set; } = DefaultFrameDuration;
        public bool IsLooping { get; set; } = true;
        public bool IsReversed { get; set; }
        public bool IsPingPong { get; set; }

        public new bool IsComplete => CurrentTime >= AnimationDuration;

        public float AnimationDuration => IsPingPong
            ? (KeyFrames.Length * 2 - 2) * FrameDuration
            : KeyFrames.Length * FrameDuration;

        public int KeyFrameIndex { get; private set; }
        public int CurrentFrame => KeyFrames.Any() ? KeyFrames[KeyFrameIndex] : 0;

        public float FramesPerSecond
        {
            get => 1.0f / FrameDuration;
            set => FrameDuration = value / 1.0f;
        }

        protected override bool OnUpdate(float deltaTime)
        {
            if (IsComplete)
            {
                if (IsLooping)
                    CurrentTime -= AnimationDuration;
            }

            if (KeyFrames.Length == 1)
            {
                KeyFrameIndex = 0;
                return IsComplete;
            }

            var frameIndex = (int)(CurrentTime / FrameDuration);
            var length = KeyFrames.Length;

            if (IsPingPong)
            {
                frameIndex = frameIndex % (length * 2 - 2);

                if (frameIndex >= length)
                    frameIndex = length - 2 - (frameIndex - length);
            }

            if (IsLooping)
            {
                if (IsReversed)
                {
                    frameIndex = frameIndex % length;
                    frameIndex = length - frameIndex - 1;
                }
                else
                    frameIndex = frameIndex % length;
            }
            else
            {
                frameIndex = IsReversed ? Math.Max(length - frameIndex - 1, 0) : Math.Min(length - 1, frameIndex);
            }

            KeyFrameIndex = frameIndex;
            return IsComplete;
        }
    }
}
