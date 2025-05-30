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
        
        


        private float GroundY => _graphics.PreferredBackBufferHeight - 50;


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
            player1 = new Player(GraphicsDevice, new Vector2(100, GroundY), Content.Load<Texture2D>("KopfkickerChar1"));
            player2 = new Player(GraphicsDevice, new Vector2(700, GroundY), Content.Load<Texture2D>("KopfkickerChar2"));
            football = new Ball(GraphicsDevice,    new Vector2(200, GroundY), Content.Load<Texture2D>("ball"));

            player1.Set_other_Player(player2);
            player2.Set_other_Player(player1);
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new Microsoft.Xna.Framework.Graphics.SpriteBatch(GraphicsDevice);
        }


        protected override void Update(GameTime gameTime)
        {
            handle_player_movement(gameTime);
            handle_player_ball_collision();
            
            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();

            player1.draw(_spriteBatch);
            player2.draw(_spriteBatch);
            football.draw(_spriteBatch);
            // To do draw Hintergrund();

            _spriteBatch.End();

            base.Draw(gameTime);
        }


        private bool IsOnGround(Vector2 position)
        {
            return position.Y >= GroundY;
        }

        
        private void handle_player_movement(GameTime gameTime)
        {
            KeyboardState state = Keyboard.GetState();
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //Game 
            if (state.IsKeyDown(Keys.Escape))
                Exit();


            //Player 1
            if (state.IsKeyDown(Keys.A))
                
                player1.move_left(delta);

            if (state.IsKeyDown(Keys.D))
                player1.move_right(delta);

            if (state.IsKeyDown(Keys.W) && IsOnGround(_player1Position))
                player1.jump(delta);


            // player 2
            if (state.IsKeyDown(Keys.Left))
                player2.move_left(delta);
            if (state.IsKeyDown(Keys.Right))
                player2.move_right(delta);

            if (state.IsKeyDown(Keys.Up) && IsOnGround(_player2Position))
                player2.jump(delta);

        }

        private void handle_player_ball_collision()
        {
            if (football.getRect().Intersects(player1.currentRect))
            {
                float direction = Math.Sign(football.position.X - player1.position.X);
                football.velocity.X += direction * football.starting_velocity.X;
                football.velocity.Y += football.starting_velocity.Y;
                //todo 
            }


            if (football.getRect().Intersects(player2.currentRect))
            {
                float direction = Math.Sign(football.position.X - player2.position.X);
                football.velocity.X += direction * football.starting_velocity.X;
                football.velocity.Y += football.starting_velocity.Y;
                //to do 
            }



            
        }
    }
}

