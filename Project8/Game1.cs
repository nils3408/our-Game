//alle 

using Microsoft.Xna.Framework;                         
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct2D1;
using SharpDX.XAudio2;
using System;
using System.Runtime.CompilerServices;

namespace Project8
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private Microsoft.Xna.Framework.Graphics.SpriteBatch _spriteBatch;

        //Spielfeld
        private Texture2D _backgroundTexture;
        // Spieler
        private Texture2D _player1Texture;
        private Texture2D _player2Texture;
        private Vector2 _player1Position;
        private Vector2 _player1Velocity;
        private Vector2 _player2Position;
        private Vector2 _player2Velocity;
        private Player player1;
        private Player player2;
        private Ball football;

        private const float MoveSpeed = 200f;




        private float GroundY => _graphics.PreferredBackBufferHeight-150;


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
            player1 = new Player(GraphicsDevice, new Vector2(100, GroundY - 50), Content.Load<Texture2D>("KopfkickerChar1"), GroundY - 50);
            player2 = new Player(GraphicsDevice, new Vector2(700, GroundY - 50), Content.Load<Texture2D>("KopfkickerChar2"), GroundY - 50);
            football = new Ball(GraphicsDevice, new Vector2(200, GroundY), Content.Load<Texture2D>("ball"), GroundY);

            player1.Set_other_Player(player2);
            player2.Set_other_Player(player1);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new Microsoft.Xna.Framework.Graphics.SpriteBatch(GraphicsDevice);
            _backgroundTexture = Content.Load<Texture2D>("Spielfeld");
        }


        protected override void Update(GameTime gameTime)
        {
            handle_player_movement(gameTime);
            handle_player_ball_collision(gameTime);
            player1.update_vertical((float)gameTime.ElapsedGameTime.TotalSeconds, GroundY - 50);
            player2.update_vertical((float)gameTime.ElapsedGameTime.TotalSeconds, GroundY - 50);
            football.move_parabel((float)gameTime.ElapsedGameTime.TotalSeconds);
            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();

            _spriteBatch.Draw(
            _backgroundTexture,
            new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight),
            Color.White
            );
            player1.draw(_spriteBatch);
            player2.draw(_spriteBatch);
            football.draw(_spriteBatch);
            // To do draw Hintergrund();

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void handle_player_movement(GameTime gameTime)
        {
            KeyboardState state = Keyboard.GetState();
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //Game 
            if (state.IsKeyDown(Keys.Escape))
                Exit();


            //Player 1
            if (state.IsKeyDown(Keys.A)) player1.move_left(delta);
            if (state.IsKeyDown(Keys.D)) player1.move_right(delta);

            if (state.IsKeyDown(Keys.W) && player1.IsOnGround(player1.position, GroundY - 50))
                player1.jump(delta, GroundY - 50);


            // player 2
            if (state.IsKeyDown(Keys.Left)) player2.move_left(delta);
            if (state.IsKeyDown(Keys.Right)) player2.move_right(delta);

            if (state.IsKeyDown(Keys.Up) && player2.IsOnGround(player2.position, GroundY - 50))
                player2.jump(delta, GroundY - 50);

        }



        private void handle_player_ball_collision(GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            // Spieler 1
            if (football.getRect().Intersects(player1.currentRect))
            {
                Vector2 direction = football.position - player1.position;
                direction.Normalize();
                football.velocity = direction * football.starting_velocity.Length();
            }

            // Spieler 2
            if (football.getRect().Intersects(player2.currentRect))
            {
                Vector2 direction = football.position - player2.position;
                direction.Normalize();
                football.velocity = direction * football.starting_velocity.Length();
            }
        }
    }
}