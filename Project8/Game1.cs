//alle 


/*------------------------------------------------------------------------------------------------------------------
 * Hinweise
 * 
 * Ball Collsion Issue
 *      For a decent time there was an issue with the collisions. 
 *      When a player collisionates with the ball, the ball changes its direction                  -> wished
 *      however one frame later: collision gets detected again: ball changes its direction again  -> not wished
 *      solution: a minimum time intervall that must pass before a collision with the same player can be detected again
 *                therefore we have the two variables CollisionCoolDown1, CollisionCoolDown2 
 * 
 * 
 * ---------------------------------------------------------------------------------------------------------------------
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct2D1;
using SharpDX.XAudio2;
using System;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace Project8
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private Microsoft.Xna.Framework.Graphics.SpriteBatch _spriteBatch;

        //Spielfeld
        private Texture2D _backgroundTexture;
        private Texture2D _goalTexture;
        private Vector2 _leftGoalPosition;
        private Vector2 _rightGoalPosition;
        // Spieler, Ball
        private Texture2D _player1Texture;
        private Texture2D _player2Texture;
        private Vector2 _player1Position;
        private Vector2 _player1Velocity;
        private Vector2 _player2Position;
        private Vector2 _player2Velocity;
        private Player player1;
        private Player player2;
        private Ball football;

        //genrell
        private float collisionCooldown1 = 0f; 
        private float collisionCooldown2 = 0f;
        private const float CollisionCooldownTime = 0.5f; 

        private float groundY => _graphics.PreferredBackBufferHeight - 300;

        float goalScale;
        int goalWidth;
        int goalHeight;
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
/*          FullScreen Mode: 
 *          ToDo Spielfeld anpassen!!
            _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            _graphics.IsFullScreen = true;
            _graphics.ApplyChanges();          
*/
            player1 = new Player(GraphicsDevice, new Vector2(60, groundY - 50), Content.Load<Texture2D>("KopfkickerChar1"), 1);
            player2 = new Player(GraphicsDevice, new Vector2(700,groundY- 50), Content.Load<Texture2D>("KopfkickerChar2"),2);
            football = new Ball(GraphicsDevice, new Vector2(300, groundY- 50 ), Content.Load<Texture2D>("ball"));

            player1.Set_other_Player(player2);
            player2.Set_other_Player(player1);
            goalScale = 0.2f;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new Microsoft.Xna.Framework.Graphics.SpriteBatch(GraphicsDevice);
            _backgroundTexture = Content.Load<Texture2D>("Spielfeld2");
            _goalTexture = Content.Load<Texture2D>("Tore2");
            
            goalWidth = (int)(_goalTexture.Width * goalScale);
            goalHeight = (int)(_goalTexture.Height * goalScale);
            
            _leftGoalPosition = new Vector2(-50, groundY + 100 - goalHeight);
            _rightGoalPosition = new Vector2(_graphics.PreferredBackBufferWidth - goalWidth + 50, groundY + 100 - goalHeight);

        }


        protected override void Update(GameTime gameTime)
        {
            handle_player_movement(gameTime);
            handle_player_ball_collision(gameTime);
            player1.update_vertical((float)gameTime.ElapsedGameTime.TotalSeconds, groundY - 50);
            player2.update_vertical((float)gameTime.ElapsedGameTime.TotalSeconds, groundY - 50);
            football.move((float)gameTime.ElapsedGameTime.TotalSeconds, groundY+50);
            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.CornflowerBlue);
            _spriteBatch.Begin();

            _spriteBatch.Draw(
            _backgroundTexture,
            new Microsoft.Xna.Framework.Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight),
            Microsoft.Xna.Framework.Color.White
            );
            _spriteBatch.Draw(_goalTexture, new Microsoft.Xna.Framework.Rectangle((int)_leftGoalPosition.X, (int)_leftGoalPosition.Y, goalWidth, goalHeight), null, Microsoft.Xna.Framework.Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
            _spriteBatch.Draw(_goalTexture, new Microsoft.Xna.Framework.Rectangle((int)_rightGoalPosition.X, (int)_rightGoalPosition.Y, goalWidth, goalHeight), Microsoft.Xna.Framework.Color.White);
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

            if (state.IsKeyDown(Keys.W) && player1.IsOnGround(player1.position, groundY - 50))
                player1.jump(delta, groundY - 50);


            // player 2
            if (state.IsKeyDown(Keys.Left)) player2.move_left(delta);
            if (state.IsKeyDown(Keys.Right)) player2.move_right(delta);
            if (state.IsKeyDown(Keys.Up) && player2.IsOnGround(player2.position, groundY - 50))
                player2.jump(delta, groundY - 50);

        }



        private void handle_player_ball_collision(GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            collisionCooldown1 -= delta;
            collisionCooldown2 -= delta;



            // Spieler 1
            if (football.getRect().Intersects(player1.currentRect) && collisionCooldown1 <=0)
            {
                 Vector2 direction = football.position - player1.position;
                 direction.Normalize();

                 football.reset_velocity();
                 football.change_direction();
                 collisionCooldown1 = CollisionCooldownTime;
            }

            // Spieler 2
            if (football.getRect().Intersects(player2.currentRect) && collisionCooldown2 <=0)
            {
                 Vector2 direction2 = football.position - player2.position;
                 direction2.Normalize();

                 football.reset_velocity();
                 football.change_direction();
                 collisionCooldown2 = CollisionCooldownTime;
                
            }
        } 
    }
}
