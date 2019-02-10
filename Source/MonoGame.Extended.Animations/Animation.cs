using System;

namespace MonoGame.Extended.Animations
{
    public abstract class Animation : IUpdate, IDisposable
    {
        private readonly bool _disposeOnComplete;
        private bool _isComplete;

        protected Animation(string name, bool disposeOnComplete)
        {
            Name = name;
            IsPaused = false;
            _disposeOnComplete = disposeOnComplete;
        }

        public string Name { get; }

        public Action OnComplete { get; set; }

        public bool IsComplete
        {
            get => _isComplete;
            protected set
            {
                if (_isComplete != value)
                {
                    _isComplete = value;

                    if (_isComplete)
                    {
                        OnComplete?.Invoke();

                        if (_disposeOnComplete)
                            Dispose();
                    }
                }
            }
        }

        public bool IsDisposed { get; private set; }
        public bool IsPlaying => !IsPaused && !IsComplete;
        public bool IsPaused { get; private set; }
        public float CurrentTime { get; protected set; }

        public virtual void Dispose()
        {
            IsDisposed = true;
        }

        public void Play()
        {
            IsPaused = false;
        }

        public void Pause()
        {
            IsPaused = true;
        }

        public void Stop()
        {
            Pause();
            Rewind();
        }

        public void Rewind()
        {
            CurrentTime = 0;
        }

        protected abstract bool OnUpdate(float elapsedSeconds);

        public void Update(float elapsedSeconds)
        {
            if (!IsPlaying)
                return;

            CurrentTime += elapsedSeconds;
            IsComplete = OnUpdate(elapsedSeconds);
        }
    }
}