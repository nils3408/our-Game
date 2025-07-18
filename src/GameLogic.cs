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
 * Item position
 *      item position can not be deklared within the construkctor when initalising the first time
 *      The reason for this is:
 *      when assigning the item a new position compare it with the postions of the other items to avoid overlapping
 *      however items get deklared before the they get array of references to the other items assigned
 *          Comparing positions with the other items (null) would lead to null- error 
 *      solution:
 *          declare all items -> give each item array of references of all items -> give each item a new position and 
 *          check on collission
 *          
 *          
 *  
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
using System.Diagnostics;
using System.Collections.Generic;

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
    //player
    private Player player1;
    private Player player2;
    private Player last_player_touching_the_ball = null;


    private Ball football;
    private float jumpheight = 250f;
    //generell
    private float collisionCooldown1 = 0f;
    private float collisionCooldown2 = 0f;
    private const float CollisionCooldownTime = 0.4f;
    
    public Dictionary<string, Texture2D> powerUpTextures;

    private float groundY = 550;

    float goalScale = 0.4f;
    int goalWidth;
    int goalHeight;

    //Tore Counter
    private int scorePlayer1 = 0;
    private int scorePlayer2 = 0;
    private SpriteFont scoreFont;

    private float gameTimer = 0f;
    private bool gameRunning = true;

    public Item item1;
    public Item[] items;

    private Texture2D _tribuneTexture; 
    private Vector2 _leftTribunePosition;  
    private Vector2 _rightTribunePosition;

    float tribuneScale = 1f; 
    int tribuneWidth;
    int tribuneHeight;
    float greenAreaY;
    float centerX;
    float tribuneSpacing;

    
    
    public GameLogic(Game baseGame) : base(baseGame){}
    public GameLogic(Game baseGame, Player leftPlayer, Player rightPlayer) : base(baseGame)

    {
        SetPlayer(leftPlayer, rightPlayer);
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

        //Initiert nur Player wenn es durch den neuen Konstruktor nicht vorher gelöscht wird, Zeilen könnten auch gelöscht werden eigentlich, später!
        if (player1 == null) player1 = new Spiderman(_graphicsDevice, new Vector2(60, groundY), Content.Load<Texture2D>("Spiderman"), 1);
        if (player2 == null) player2 = new Sonic(_graphicsDevice, new Vector2(_graphics.PreferredBackBufferWidth - 300, groundY), Content.Load<Texture2D>("sonic"), 2);

        powerUpTextures = new()
        {
            { "firefootball", Content.Load<Texture2D>("firefootball") },
            { "icefootball",  Content.Load<Texture2D>("icefootball")  }
        };
        football = new Ball(_graphicsDevice, new Vector2(_graphics.PreferredBackBufferWidth / 2f, groundY), Content.Load<Texture2D>("football"), powerUpTextures);

        player1.Set_other_Player(player2);
        player2.Set_other_Player(player1);

        item1 = new Item(_graphicsDevice, Content, football);
        items = new Item[] { item1};
        distribute_items();
        update_all_item_positions();


    }

    public override void LoadContent()
    {
        spriteBatch = new Microsoft.Xna.Framework.Graphics.SpriteBatch(_graphicsDevice);
        _backgroundTexture = Content.Load<Texture2D>("Spielfeld3");
        _goalTexture = Content.Load<Texture2D>("Tore");

        goalWidth = (int)(_goalTexture.Width * goalScale);
        goalHeight = (int)(_goalTexture.Height * goalScale);

        _leftGoalPosition = new Vector2(-100, groundY - 255);
        _rightGoalPosition = new Vector2(_graphics.PreferredBackBufferWidth - goalWidth + 100, groundY - 255);
        scoreFont = Content.Load<SpriteFont>("Arial");

        _tribuneTexture = Content.Load<Texture2D>("TribueneLang");
        tribuneWidth = (int)(_tribuneTexture.Width * tribuneScale);
        tribuneHeight = (int)(_tribuneTexture.Height * tribuneScale);

        greenAreaY = groundY+50;
        

        _leftTribunePosition = new Vector2(450, -100);
        

    }


    public override void Update(GameTime gameTime)
    {
        handle_player_movement(gameTime);
        handle_player_ball_collision(gameTime);
        
        player1.update_vertical((float)gameTime.ElapsedGameTime.TotalSeconds);
        player2.update_vertical((float)gameTime.ElapsedGameTime.TotalSeconds);

        football.move((float)gameTime.ElapsedGameTime.TotalSeconds, groundY);

        handle_player_coin_colission();
        handle_ball_coin_collision();
        check_for_goal();

        player1.reset_powerUp1_if_time_is_over();
        player1.reset_powerUp2_if_time_is_over();
        player2.reset_powerUp1_if_time_is_over();
        player2.reset_powerUp2_if_time_is_over();
        football.reset_powerUps_if_time_is_over();



        //Zurück ins Menu wenn ESC losgelassen wird 
        if (InputHandler.IsReleased(Keys.Escape))
        {
            System.Diagnostics.Debug.WriteLine("escape!");
            Game1.nextState = new Menu(baseGame);
            Game1.GameIsInitialized = false;
        }
        if (gameRunning)
        {
            gameTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
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
        //Tribünen
        spriteBatch.Draw(_tribuneTexture,
        new Microsoft.Xna.Framework.Rectangle((int)_leftTribunePosition.X, (int)_leftTribunePosition.Y, tribuneWidth, tribuneHeight),
        Microsoft.Xna.Framework.Color.White);
        


        player1.draw(spriteBatch);
        player2.draw(spriteBatch);
        football.draw(spriteBatch);

        //draw items
        foreach(Item item in items)
        {
            item.draw(spriteBatch, gameTime);
        }

        // Score und Timer anzeigen
        spriteBatch.DrawString(scoreFont, $"{scorePlayer1} : {scorePlayer2}", new Vector2(_graphics.PreferredBackBufferWidth / 2f, 20), Color.White);
        string timerText = $" {Math.Floor(gameTimer)}s";
        spriteBatch.DrawString(scoreFont, timerText, new Vector2(20, 20), Color.White);

        spriteBatch.End();
    }

    private void handle_player_movement(GameTime gameTime)
    {
        // movement and other input
        KeyboardState state = Keyboard.GetState();
        float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;


        //Player 1
        if (state.IsKeyDown(Keys.A)) player1.move(delta, -1);
        if (state.IsKeyDown(Keys.D)) player1.move(delta, 1);
        if (state.IsKeyDown(Keys.W) && player1.IsOnGround(player1.position))
            player1.jump(delta);
        
        if (state.IsKeyDown(Keys.E)) player1.do_special_effect(delta);
        if (state.IsKeyDown(Keys.R)) player1.activate_powerUP1();
        if (state.IsKeyDown(Keys.F)) player1.activate_powerUP2();


        // player 2
        // {J K L I} statt {W S A D}
        if (state.IsKeyDown(Keys.J)) player2.move(delta, -1);
        if (state.IsKeyDown(Keys.L)) player2.move(delta, 1);
        if (state.IsKeyDown(Keys.I) && player2.IsOnGround(player2.position))
            player2.jump(delta);
        
        if (state.IsKeyDown(Keys.O)) player2.do_special_effect(delta);
        if (state.IsKeyDown(Keys.P))  player2.activate_powerUP1();
        if (state.IsKeyDown(Keys.OemPeriod)) player2.activate_powerUP2();


    }



    private void handle_player_coin_colission()
    {
        // when a player colliderects with a item , he gets the PoweUP, linked to the PowerUP
        handle_player_coin_collision_helper(player1);
        handle_player_coin_collision_helper(player2);
        
    }

    private void handle_player_coin_collision_helper(Player playerABC)
    {
        foreach (Item itemA in items)
        {
            if (playerABC.currentRect.Intersects(itemA.current_Rect))
            {
                playerABC.set_PowerUp(itemA.linked_powerup);

                itemA.set_new_powerUP();
                itemA.set_random_position();

            }
        }
    }


    private void handle_ball_coin_collision()
    {
        //when ball collides with the ball. The last player touching the ball gets the PowerUp linked to the Item
        if (last_player_touching_the_ball == null) { return; }

        foreach (Item itemA in items)
        {
            if (football.getRect().Intersects(itemA.current_Rect))
            {
                last_player_touching_the_ball.set_PowerUp(itemA.linked_powerup);

                itemA.set_new_powerUP();
                itemA.set_random_position();

            }
        }
    }


    //Check Ball im Tor
    private void check_for_goal()
    {
        //_leftGoalPosition und _rightGoalPosition x - / + 1/2Ballsize
        Microsoft.Xna.Framework.Rectangle leftGoal = new Microsoft.Xna.Framework.Rectangle((int)_leftGoalPosition.X - 25, (int)_leftGoalPosition.Y, goalWidth, goalHeight);
        Microsoft.Xna.Framework.Rectangle rightGoal = new Microsoft.Xna.Framework.Rectangle((int)_rightGoalPosition.X + 25, (int)_rightGoalPosition.Y, goalWidth, goalHeight);

        Microsoft.Xna.Framework.Rectangle ballRect = football.getRect();


        if (leftGoal.Contains(ballRect))
        {
            scorePlayer2++;
            football.Reset_Position(new Vector2(_graphics.PreferredBackBufferWidth / 2f, groundY));
            football.reset_values();
            player1.set_back_to_starting_position();
            player2.set_back_to_starting_position();
            player1.reset_rect_size();
            player2.reset_rect_size();
            update_all_item_positions();
            
        }


        if (rightGoal.Contains(ballRect))
        {
            scorePlayer1++;
            football.Reset_Position(new Vector2(_graphics.PreferredBackBufferWidth / 2f, groundY));
            football.reset_values();
            player1.set_back_to_starting_position();
            player2.set_back_to_starting_position();
            player1.reset_rect_size();
            player2.reset_rect_size();
            update_all_item_positions();
        }
    }



    private void handle_player_ball_collision(GameTime gameTime)
    {
        float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
        collisionCooldown1 -= delta;
        collisionCooldown2 -= delta;


        // Spieler 1
        if (football.getRect().Intersects(player1.currentRect) && collisionCooldown1 <= 0)
        {
            last_player_touching_the_ball = player1;
            Vector2 direction = football.position - player1.position;

            if (football.fire_powerUp_in_use == false)    football.reset_velocity();
            football.change_direction(direction);
            collisionCooldown1 = CollisionCooldownTime;

            // shooting    
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.S)) { football.get_shooted_diagonal(); }
            if (state.IsKeyDown(Keys.Y)) { football.get_shooted_horizontal(); }
        }

        // Spieler 2
        if (football.getRect().Intersects(player2.currentRect) && collisionCooldown2 <= 0)
        {
            last_player_touching_the_ball= player2;
            Vector2 direction2 = football.position - player2.position;

            football.reset_velocity();
            football.change_direction(direction2);
            collisionCooldown2 = CollisionCooldownTime;

            // shooting    
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.K)) { football.get_shooted_diagonal(); }
            if (state.IsKeyDown(Keys.M)) { football.get_shooted_horizontal(); }

        }
    }


    public void SetPlayer(Player left, Player right)
    {
        float exakter_ground_y = groundY - 40;

        player1 = left;
        player1.position = new Vector2(60, exakter_ground_y);
        player1.starting_position= new Vector2(60, exakter_ground_y);
        player1.set_groundYs(exakter_ground_y);

        player2 = right;
        player2.position = new Vector2(_graphics.PreferredBackBufferWidth - 300, exakter_ground_y);
        player2.starting_position = new Vector2(_graphics.PreferredBackBufferWidth - 300, exakter_ground_y);
        player2.set_groundYs(exakter_ground_y);

    }


    public void distribute_items()
    {
        // alle items jedem item-object zuordnen
        for (int i = 0; i< items.Length; i++)
        {
            items[i].set_all_items(items);
        }
    }

    public void update_all_item_positions()
    {
        //update position of all items
       for(int i = 0; i< items.Length; i++)
       {
            items[i].set_random_position();
       }
    }
}

