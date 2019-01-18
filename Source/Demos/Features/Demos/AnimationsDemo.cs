using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Animations;
using MonoGame.Extended.Animations.SpriteSheets;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.TextureAtlases;

namespace Features.Demos
{
    public class AnimationsDemo : DemoBase
    {
        public override string Name => "Animations";

        private SpriteBatch _spriteBatch;

        private AnimatedSprite _animatedSprite;

        //private Zombie _zombie;
        //private SpriteSheetAnimation _animation;
        //private Sprite _fireballSprite;
        //private AnimatedSprite _motwSprite;

        public AnimationsDemo(GameMain game)
            : base(game)
        {
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            var texture = Content.Load<Texture2D>("Animations/elthen-spider");
            var textureAtlas = TextureAtlas.Create("elthen-adventurer", texture, 32, 32);
            var animations = new SpriteSheetAnimationFactory(textureAtlas);
            animations.Add("idle", new SpriteSheetAnimationData(new[] { 0, 1, 2, 3, 4 }, 0.15f));
            animations.Add("walk", new SpriteSheetAnimationData(new[] { 9, 10, 11, 12, 13, 14 }, 0.15f));
            _animatedSprite = new AnimatedSprite(animations, "idle");

            //var zombieAnimations = Content.Load<SpriteSheetAnimationFactory>("Animations/zombie-animations");
            //_zombie = new Zombie(zombieAnimations, new Vector2(100, 100));

            //var fireballTexture = Content.Load<Texture2D>("Animations/fireball");
            //var fireballAtlas = TextureAtlas.Create("Animations/fireball-atlas", fireballTexture, 130, 50);
            //_animation = new SpriteSheetAnimation("fireballAnimation", fireballAtlas.Regions.ToArray()) { FrameDuration = 0.2f };
            //_fireballSprite = new Sprite(_animation.CurrentFrame);// { Position = new Vector2(-150, 100) };

            //var motwTexture = Content.Load<Texture2D>("Animations/motw");
            //var motwAtlas = TextureAtlas.Create("Animations/fireball-atlas", motwTexture, 52, 72);
            //var motwAnimationFactory = new SpriteSheetAnimationFactory(motwAtlas);
            //motwAnimationFactory.Add("idle", new SpriteSheetAnimationData(new[] { 0 }));
            //motwAnimationFactory.Add("walkSouth", new SpriteSheetAnimationData(new[] { 0, 1, 2, 1 }, isLooping: false));
            //motwAnimationFactory.Add("walkWest", new SpriteSheetAnimationData(new[] { 12, 13, 14, 13 }, isLooping: false));
            //motwAnimationFactory.Add("walkEast", new SpriteSheetAnimationData(new[] { 24, 25, 26, 25 }, isLooping: false));
            //motwAnimationFactory.Add("walkNorth", new SpriteSheetAnimationData(new[] { 36, 37, 38, 37 }, isLooping: false));
            //_motwSprite = new AnimatedSprite(motwAnimationFactory);// { Position = new Vector2(20, 20) };
            //_motwSprite.Play("walkSouth").IsLooping = true;
        }

        protected override void Update(GameTime gameTime)
        {
            var deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            var keyboardState = Keyboard.GetState();

            _animatedSprite.Update(deltaSeconds);

            //// motw
            //if (keyboardState.IsKeyDown(Keys.W))
            //    _motwSprite.Play("walkNorth");

            //if (keyboardState.IsKeyDown(Keys.A))
            //    _motwSprite.Play("walkWest");

            //if (keyboardState.IsKeyDown(Keys.S))
            //    _motwSprite.Play("walkSouth");

            //if (keyboardState.IsKeyDown(Keys.D))
            //    _motwSprite.Play("walkEast");

            // camera
            if (keyboardState.IsKeyDown(Keys.R))
                Camera.ZoomIn(deltaSeconds);

            if (keyboardState.IsKeyDown(Keys.F))
                Camera.ZoomOut(deltaSeconds);

            //// zombie
            //if (keyboardState.IsKeyDown(Keys.Left))
            //    _zombie.Walk(-1.0f);

            //if (keyboardState.IsKeyDown(Keys.Right))
            //    _zombie.Walk(1.0f);

            //if (keyboardState.IsKeyDown(Keys.Space))
            //    _zombie.Attack();

            //if (keyboardState.IsKeyDown(Keys.Enter))
            //    _zombie.Die();

            //// update must be called before collision detection
            //_zombie.Update(gameTime);

            //_animation.Update(deltaSeconds);
            //_fireballSprite.TextureRegion = _animation.CurrentFrame;

            //_motwSprite.Update(deltaSeconds);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //_spriteBatch.Begin(transformMatrix: Camera.GetViewMatrix(), samplerState: SamplerState.PointClamp);
            //_zombie.Draw(_spriteBatch);
            //_spriteBatch.Draw(_fireballSprite, new Transform2(200, 200));
            //_spriteBatch.End();

            //_spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: Camera.GetViewMatrix());
            //_spriteBatch.Draw(_motwSprite, new Transform2(300, 300));
            //_spriteBatch.End();

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: Matrix.CreateScale(4));
            _spriteBatch.Draw(_animatedSprite, new Transform2(50, 50));
            _spriteBatch.End();


            base.Draw(gameTime);
        }
    }

    public enum ZombieState
    {
        None,
        Appearing,
        Idle,
        Walking,
        Attacking,
        Dying
    }

    public class Zombie : IUpdate
    {
        public Zombie(SpriteSheetAnimationFactory animations, Vector2 position)
        {
            _sprite = new AnimatedSprite(animations);
            _transform = new Transform2(position);

            State = ZombieState.Appearing;
            IsOnGround = false;
        }

        private readonly AnimatedSprite _sprite;
        private float _direction = -1.0f;
        private ZombieState _state;
        private readonly Transform2 _transform;

        public RectangleF BoundingBox => _sprite.GetBoundingRectangle(_transform.Position, _transform.Rotation, _transform.Scale);
        public bool IsOnGround { get; private set; }
        public bool IsReady => State != ZombieState.Appearing && State != ZombieState.Dying;

        public ZombieState State
        {
            get => _state;
            private set
            {
                if (_state != value)
                {
                    _state = value;

                    switch (_state)
                    {
                        case ZombieState.Attacking:
                            _sprite.Play("attack", () => State = ZombieState.Idle);
                            break;
                        case ZombieState.Dying:
                            _sprite.Play("die", () => State = ZombieState.Appearing);
                            break;
                        case ZombieState.Idle:
                            _sprite.Play("idle");
                            break;
                        case ZombieState.Appearing:
                            _sprite.Play("appear", () => State = ZombieState.Idle);
                            break;
                        case ZombieState.Walking:
                            _sprite.Play("walk", () => State = ZombieState.Idle);
                            break;
                    }
                }
            }
        }

        public Vector2 Velocity { get; set; }

        public void Update(float elapsedSeconds)
        {
            _sprite.Update(elapsedSeconds);

            IsOnGround = false;

            if (State == ZombieState.Walking && Math.Abs(Velocity.X) < 0.1f)
                State = ZombieState.Idle;

            _transform.Position += Velocity * elapsedSeconds;
            Velocity = Vector2.Zero;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_sprite, _transform);
        }

        public void Walk(float direction)
        {
            _sprite.Effect = _direction > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            _direction = direction;

            Velocity = new Vector2(200f * _direction, Velocity.Y);

            if (IsReady)
                State = ZombieState.Walking;
        }

        public void Attack()
        {
            if (IsReady)
                State = ZombieState.Attacking;
        }

        public void Die()
        {
            State = ZombieState.Dying;
        }

        public void Jump()
        {
            if (IsReady && IsOnGround)
            {
                State = ZombieState.None;
                Velocity = new Vector2(Velocity.X, -650);
            }
        }
    }
}