using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Project8
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // Spieler
        private Texture2D _rectangleTexture;
        private Vector2 _player1Position;
        private Vector2 _player2Position;
        private Vector2 _player1Velocity;
        private Vector2 _player2Velocity;

        private const int RectangleWidth = 50;
        private const int RectangleHeight = 50;
        private const float MoveSpeed = 200f;
        private const float JumpVelocity = -400f;
        private const float Gravity = 1000f;

        private float GroundY => _graphics.PreferredBackBufferHeight - RectangleHeight;

        // Ball
        private Texture2D _ballTexture;
        private Vector2 _ballPosition;
        private Vector2 _ballVelocity;
        private const int BallSize = 32;
        private const float BallFriction = 500f;
        private const float BallPushStrength = 200f;
        private const float BallLiftStrength = -150f;
        private const int BallMargin = 100; // ← Abstand zum Fensterrand

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 480;
        }

        protected override void Initialize()
        {
            _player1Position = new Vector2(100, GroundY);
            _player2Position = new Vector2(600, GroundY);

            _ballPosition = new Vector2(
                (_player1Position.X + _player2Position.X) / 2f + RectangleWidth / 2f - BallSize / 2f,
                GroundY + RectangleHeight / 2f - BallSize / 2f
            );
            _ballVelocity = Vector2.Zero;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _rectangleTexture = new Texture2D(GraphicsDevice, 1, 1);
            _rectangleTexture.SetData(new[] { Color.White });

            _ballTexture = Content.Load<Texture2D>("ball");
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState state = Keyboard.GetState();
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 newPlayer1Pos = _player1Position;
            Vector2 newPlayer2Pos = _player2Position;

            if (state.IsKeyDown(Keys.A))
                newPlayer1Pos.X -= MoveSpeed * delta;
            if (state.IsKeyDown(Keys.D))
                newPlayer1Pos.X += MoveSpeed * delta;

            if (state.IsKeyDown(Keys.Left))
                newPlayer2Pos.X -= MoveSpeed * delta;
            if (state.IsKeyDown(Keys.Right))
                newPlayer2Pos.X += MoveSpeed * delta;

            Rectangle futureRect1 = new Rectangle((int)newPlayer1Pos.X, (int)_player1Position.Y, RectangleWidth, RectangleHeight);
            Rectangle futureRect2 = new Rectangle((int)newPlayer2Pos.X, (int)_player2Position.Y, RectangleWidth, RectangleHeight);

            Rectangle currentRect1 = new Rectangle((int)_player1Position.X, (int)_player1Position.Y, RectangleWidth, RectangleHeight);
            Rectangle currentRect2 = new Rectangle((int)_player2Position.X, (int)_player2Position.Y, RectangleWidth, RectangleHeight);

            if (!futureRect1.Intersects(currentRect2))
                _player1Position.X = newPlayer1Pos.X;

            if (!futureRect2.Intersects(currentRect1))
                _player2Position.X = newPlayer2Pos.X;

            if (state.IsKeyDown(Keys.W) && IsOnGround(_player1Position))
                _player1Velocity.Y = JumpVelocity;

            if (state.IsKeyDown(Keys.Up) && IsOnGround(_player2Position))
                _player2Velocity.Y = JumpVelocity;

            _player1Velocity.Y += Gravity * delta;
            _player2Velocity.Y += Gravity * delta;

            _player1Position.Y += _player1Velocity.Y * delta;
            _player2Position.Y += _player2Velocity.Y * delta;

            if (_player1Position.Y >= GroundY)
            {
                _player1Position.Y = GroundY;
                _player1Velocity.Y = 0;
            }

            if (_player2Position.Y >= GroundY)
            {
                _player2Position.Y = GroundY;
                _player2Velocity.Y = 0;
            }

            _player1Position.X = MathHelper.Clamp(_player1Position.X, 0, _graphics.PreferredBackBufferWidth - RectangleWidth);
            _player2Position.X = MathHelper.Clamp(_player2Position.X, 0, _graphics.PreferredBackBufferWidth - RectangleWidth);

            Rectangle ballRect = new Rectangle((int)_ballPosition.X, (int)_ballPosition.Y, BallSize, BallSize);
            Rectangle player1Rect = new Rectangle((int)_player1Position.X, (int)_player1Position.Y, RectangleWidth, RectangleHeight);
            Rectangle player2Rect = new Rectangle((int)_player2Position.X, (int)_player2Position.Y, RectangleWidth, RectangleHeight);

            if (ballRect.Intersects(player1Rect))
            {
                float dir = Math.Sign(_ballPosition.X - _player1Position.X);
                _ballVelocity.X += dir * BallPushStrength;
                _ballVelocity.Y = BallLiftStrength;
            }

            if (ballRect.Intersects(player2Rect))
            {
                float dir = Math.Sign(_ballPosition.X - _player2Position.X);
                _ballVelocity.X += dir * BallPushStrength;
                _ballVelocity.Y = BallLiftStrength;
            }

            _ballVelocity.Y += Gravity * delta;

            if (_ballVelocity.X > 0)
                _ballVelocity.X = Math.Max(0, _ballVelocity.X - BallFriction * delta);
            else if (_ballVelocity.X < 0)
                _ballVelocity.X = Math.Min(0, _ballVelocity.X + BallFriction * delta);

            _ballPosition += _ballVelocity * delta;

            float ballGroundY = GroundY + RectangleHeight / 2f - BallSize / 2f;
            if (_ballPosition.Y >= ballGroundY)
            {
                _ballPosition.Y = ballGroundY;
                _ballVelocity.Y = 0;
            }

            // Randbegrenzung mit Abstand
            float minX = BallMargin;
            float maxX = _graphics.PreferredBackBufferWidth - BallSize - BallMargin;
            _ballPosition.X = MathHelper.Clamp(_ballPosition.X, minX, maxX);

            if (state.IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            _spriteBatch.Draw(_rectangleTexture,
                new Rectangle((int)_player1Position.X, (int)_player1Position.Y, RectangleWidth, RectangleHeight),
                Color.Blue);

            _spriteBatch.Draw(_rectangleTexture,
                new Rectangle((int)_player2Position.X, (int)_player2Position.Y, RectangleWidth, RectangleHeight),
                Color.Red);

            _spriteBatch.Draw(_ballTexture,
                new Rectangle((int)_ballPosition.X, (int)_ballPosition.Y, BallSize, BallSize),
                Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private bool IsOnGround(Vector2 position)
        {
            return position.Y >= GroundY;
        }
    }
}
