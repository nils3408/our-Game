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
using Microsoft.Xna.Framework.Content;
//using SharpDX.Direct2D1;
//using SharpDX.XAudio2;
using System;
//using System.Drawing;
//using System.Runtime.CompilerServices;
using our_Game;


    public class GameLogic : GameState
    {
     

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
        private float jumpheight = 250f;
        //generell
        private float collisionCooldown1 = 0f; 
        private float collisionCooldown2 = 0f;
        private const float CollisionCooldownTime = 0.5f; 

        private float groundY =550;

        float goalScale = 0.3f;
        int goalWidth;
        int goalHeight;

        //Tore Counter
        private int scorePlayer1 = 0;
        private int scorePlayer2 = 0;
        private SpriteFont scoreFont;
    public GameLogic(Game baseGame):base(baseGame)

        {
            //this.playerList = playerList;
        }

        public override void Initialize()
        {
 /*          FullScreen Mode: 
           ToDo Spielfeld anpassen!!
            _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            _graphics.IsFullScreen = true;
            _graphics.ApplyChanges();          
*/
            player1 = new Spiderman (_graphicsDevice, new Vector2(60, groundY), Content.Load<Texture2D>("Spiderman"),1);
            player2 = new Player    (_graphicsDevice, new Vector2(_graphics.PreferredBackBufferWidth -300,groundY ), Content.Load<Texture2D>("KopfkickerChar2_neu"),2);
            football = new Ball(_graphicsDevice,new Vector2(_graphics.PreferredBackBufferWidth / 2f, groundY), Content.Load<Texture2D>("football"));

            player1.Set_other_Player(player2);
            player2.Set_other_Player(player1);
            
        }

        public override void LoadContent()
        {
            spriteBatch = new Microsoft.Xna.Framework.Graphics.SpriteBatch(_graphicsDevice);
            _backgroundTexture = Content.Load<Texture2D>("Spielfeld2");
            _goalTexture = Content.Load<Texture2D>("Tore");
            
            goalWidth = (int)(_goalTexture.Width * goalScale);
            goalHeight = (int)(_goalTexture.Height * goalScale);
            
            _leftGoalPosition = new Vector2(-50, groundY  - 165);
            _rightGoalPosition = new Vector2(_graphics.PreferredBackBufferWidth - goalWidth + 50, groundY - 165);
            scoreFont = Content.Load<SpriteFont>("Arial");
        }


    public override void Update(GameTime gameTime)
    {
        handle_player_movement(gameTime);
        handle_player_ball_collision(gameTime);
        player1.update_vertical((float)gameTime.ElapsedGameTime.TotalSeconds, groundY - 50);
        player2.update_vertical((float)gameTime.ElapsedGameTime.TotalSeconds, groundY - 50);
        football.move((float)gameTime.ElapsedGameTime.TotalSeconds, groundY);
        check_for_goal();
            
            //Zurück ins Menu wenn ESC losgelassen wird 
            if (InputHandler.IsReleased(Keys.Escape)) {
                System.Diagnostics.Debug.WriteLine("escape!");
                Game1.nextState = Game1.menu;
            }
            
        }


        public override void Draw(GameTime gameTime)
        {
            _graphicsDevice.Clear(Microsoft.Xna.Framework.Color.CornflowerBlue);
            spriteBatch.Begin();

            spriteBatch.Draw(
            _backgroundTexture,
            new Microsoft.Xna.Framework.Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight),
            Microsoft.Xna.Framework.Color.White
            );
            spriteBatch.Draw(_goalTexture, new Microsoft.Xna.Framework.Rectangle((int)_leftGoalPosition.X, (int)_leftGoalPosition.Y, goalWidth, goalHeight), null, Microsoft.Xna.Framework.Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
            spriteBatch.Draw(_goalTexture, new Microsoft.Xna.Framework.Rectangle((int)_rightGoalPosition.X, (int)_rightGoalPosition.Y, goalWidth, goalHeight), Microsoft.Xna.Framework.Color.White);
            player1.draw(spriteBatch);
            player2.draw(spriteBatch);
            football.draw(spriteBatch);

            spriteBatch.DrawString(scoreFont, $"{scorePlayer1} : {scorePlayer2}", new Vector2(_graphics.PreferredBackBufferWidth / 2f, 20), Color.White);
            
            spriteBatch.End();

            
        }

        private void handle_player_movement(GameTime gameTime)
        {
            KeyboardState state = Keyboard.GetState();
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;


            //Player 1
            if (state.IsKeyDown(Keys.A)) player1.move_left(delta);
            if (state.IsKeyDown(Keys.D)) player1.move_right(delta);
            if (state.IsKeyDown(Keys.W) && player1.IsOnGround(player1.position, groundY-250))
                player1.jump(delta, groundY-jumpheight);
            if (state.IsKeyDown(Keys.E)) player1.do_special_effect(delta);


            // player 2
            if (state.IsKeyDown(Keys.Left)) player2.move_left(delta);
            if (state.IsKeyDown(Keys.Right)) player2.move_right(delta);
            if (state.IsKeyDown(Keys.Up) && player2.IsOnGround(player2.position, groundY -250))
                player2.jump(delta, groundY - jumpheight );

        }

         //Check Ball im Tor
        private void check_for_goal()
        {
        //_leftGoalPosition und _rightGoalPosition x - / + 1/2Ballsize
            Microsoft.Xna.Framework.Rectangle leftGoal = new Microsoft.Xna.Framework.Rectangle((int)_leftGoalPosition.X - 25, (int)_leftGoalPosition.Y, goalWidth , goalHeight);
            Microsoft.Xna.Framework.Rectangle rightGoal = new Microsoft.Xna.Framework.Rectangle((int)_rightGoalPosition.X + 25, (int)_rightGoalPosition.Y, goalWidth, goalHeight);

            Microsoft.Xna.Framework.Rectangle ballRect = football.getRect();

          
            if (leftGoal.Contains(ballRect))
            {
                scorePlayer2++;
                football.Reset_Position(new Vector2(_graphics.PreferredBackBufferWidth / 2f, groundY));
            }

            
            if (rightGoal.Contains(ballRect))
            {
                scorePlayer1++;
                football.Reset_Position(new Vector2(_graphics.PreferredBackBufferWidth / 2f, groundY));
            }
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

                football.reset_velocity();
                football.change_direction(direction);
                collisionCooldown1 = CollisionCooldownTime;

                // shooting    
                KeyboardState state = Keyboard.GetState();
                if (state.IsKeyDown(Keys.S)) { football.getShooted();}
            }   

            // Spieler 2
            if (football.getRect().Intersects(player2.currentRect) && collisionCooldown2 <=0)
            {
                 Vector2 direction2 = football.position - player2.position;

                football.reset_velocity(); 
                football.change_direction(direction2);
                collisionCooldown2 = CollisionCooldownTime;

                // shooting    
                KeyboardState state = Keyboard.GetState();
                if (state.IsKeyDown(Keys.Down)) { football.getShooted(); }

            }
        } 
    }

