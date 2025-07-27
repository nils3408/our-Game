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
using System.Security.Cryptography.X509Certificates;
using System.Runtime.CompilerServices;


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
    private Texture2D[] football_textures;
    private float jumpheight = 250f;
    //generell
    private float collisionCooldown1 = 0f;
    private float collisionCooldown2 = 0f;
    private const float CollisionCooldownTime = 0.4f;

    public Dictionary<string, Texture2D> ball_textures;

    private float groundY = 550;

    public float goalScale = 0.4f;
    public float goalScale_copy = 0.4f;
    int goalWidth;
    int goalHeight;
    Microsoft.Xna.Framework.Rectangle leftGoal;
    Microsoft.Xna.Framework.Rectangle rightGoal;

    //Tore Counter
    private int scorePlayer1 = 0;
    private int scorePlayer2 = 0;
    private int winningScore = 7;
    private SpriteFont scoreFont;
    private bool gameWon = false;
    private string winnerText = "TEST";
    private Texture2D overlayTexture;

    private float gameTimer = 0f;
    private bool gameRunning = true;

    // item
    public Item item1;
    public Item[] items;

    // Shuriken
    List<Schuriken> schurikenListe = new List<Schuriken>();
    Texture2D schuriken_texture;


    private Texture2D _tribuneTexture;
    private Vector2 _leftTribunePosition;
    private Vector2 _rightTribunePosition;

    float tribuneScale = 1f;
    int tribuneWidth;
    int tribuneHeight;
    float greenAreaY;
    float centerX;
    float tribuneSpacing;





    public GameLogic(Game baseGame) : base(baseGame) { }
    public GameLogic(Game baseGame, Player leftPlayer, Player rightPlayer) : base(baseGame)

    {
        SetPlayer(leftPlayer, rightPlayer);
    }

    public override void Initialize()
    {


        //Initiert nur Player wenn es durch den neuen Konstruktor nicht vorher gelöscht wird, Zeilen könnten auch gelöscht werden eigentlich, später!
        //if (player1 == null) player1 = new Spiderman(_graphicsDevice, new Vector2(60, groundY), Content.Load<Texture2D>("Spiderman"), 1);
        //if (player2 == null) player2 = new Sonic(_graphicsDevice, new Vector2(_graphics.PreferredBackBufferWidth - 300, groundY), Content.Load<Texture2D>("sonic"), 2);

        ball_textures = new()
        {
            {"football" ,     Content.Load<Texture2D>("football")     },
            { "firefootball", Content.Load<Texture2D>("firefootball") },
            { "icefootball",  Content.Load<Texture2D>("icefootball")  }
        };

        football = new Ball(_graphicsDevice, new Vector2(_graphics.PreferredBackBufferWidth / 2f, groundY), ball_textures);

        player1.Set_other_Player(player2);
        player2.Set_other_Player(player1);

        item1 = new Item(_graphicsDevice, Content, football);
        items = new Item[] { item1 };
        distribute_items();
        update_all_item_positions();


    }

    public override void LoadContent()
    {
        spriteBatch = new Microsoft.Xna.Framework.Graphics.SpriteBatch(_graphicsDevice);
        _backgroundTexture = Content.Load<Texture2D>("Spielfeld3");

        _goalTexture = Content.Load<Texture2D>("Tore");
        set_goal_size();
        update_goal_positions();

        scoreFont = Content.Load<SpriteFont>("Arial");

        _tribuneTexture = Content.Load<Texture2D>("TribueneLang");
        tribuneWidth = (int)(_tribuneTexture.Width * tribuneScale);
        tribuneHeight = (int)(_tribuneTexture.Height * tribuneScale);

        greenAreaY = groundY + 50;


        _leftTribunePosition = new Vector2(450, -100);

        schuriken_texture = Content.Load<Texture2D>("shuriken");

        overlayTexture = new Texture2D(_graphicsDevice, 1, 1);
        overlayTexture.SetData(new[] { Color.White });

    }


    public override void Update(GameTime gameTime)
    {
        player1.update_schuriken_knockout_phase();
        player2.update_schuriken_knockout_phase();

        handle_player_movement(gameTime);
        handle_player_ball_collision(gameTime);

        player1.update_vertical((float)gameTime.ElapsedGameTime.TotalSeconds);
        player2.update_vertical((float)gameTime.ElapsedGameTime.TotalSeconds);

        football.move((float)gameTime.ElapsedGameTime.TotalSeconds, groundY);

        handle_player_coin_colission();
        handle_ball_coin_collision();
        handle_player_schuriken_collision();
        if (!gameWon)
        {
            check_for_goal();
        }


        player1.reset_powerUps_if_time_is_over();
        player2.reset_powerUps_if_time_is_over();
        football.reset_powerUps_if_time_is_over();

        move_schuriken(gameTime);
        update_schuriken_list();


        //Zurück ins Menu wenn ESC losgelassen wird 
        if (InputHandler.IsReleased(Keys.Escape))
        {
            System.Diagnostics.Debug.WriteLine("escape!");
            //Game1.GameIsInitialized = false;
            Game1.nextState = new Menu(baseGame);

        }
        if (gameRunning)
        {
            gameTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
        if (gameWon)
        {
            if (InputHandler.IsReleased(Keys.Escape))
            {
                System.Diagnostics.Debug.WriteLine("escape!");
                Game1.nextState = new Menu(baseGame);
            }
            return; // Keine weitere Spiellogik ausführen
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

        Color gameColor = gameWon ? Color.White * 0.3f : Color.White;

        player1.draw(spriteBatch);
        player2.draw(spriteBatch);
        football.draw(spriteBatch, gameTime);

        //draw items
        foreach (Item item in items)
        {
            item.draw(spriteBatch, gameTime);
        }

        //draw Shiruken 
        foreach (Schuriken s in schurikenListe)
        {
            s.draw(spriteBatch, gameTime);
        }


        // Score und Timer anzeigen
        spriteBatch.DrawString(scoreFont, $"{scorePlayer1} : {scorePlayer2}", new Vector2(_graphics.PreferredBackBufferWidth / 2f, 20), Color.White);
        string timerText = $" {Math.Floor(gameTimer)}s";
        spriteBatch.DrawString(scoreFont, timerText, new Vector2(20, 20), Color.White);

        if (gameWon)
        {
            // Semi-transparenter Overlay
            spriteBatch.Draw(overlayTexture,
                new Microsoft.Xna.Framework.Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight),
                Color.Black * 0.5f);

            // Gewinner-Text
            Vector2 textSize = scoreFont.MeasureString(winnerText);
            Vector2 textPosition = new Vector2(
                (_graphics.PreferredBackBufferWidth - textSize.X) / 2,
                (_graphics.PreferredBackBufferHeight - textSize.Y) / 2
            );

            spriteBatch.DrawString(scoreFont, winnerText, textPosition, Color.Gold);

            // "ESC für Menü" Text
            string escText = "ESC um zum Startbildschirm zu kommen!";
            Vector2 escTextSize = scoreFont.MeasureString(escText);
            Vector2 escTextPosition = new Vector2(
                (_graphics.PreferredBackBufferWidth - escTextSize.X) / 2,
                textPosition.Y + textSize.Y + 50
            );

            spriteBatch.DrawString(scoreFont, escText, escTextPosition, Color.White);
        }

        spriteBatch.End();
    }



    public void SetPlayer(Player left, Player right)
    {
        float exakter_ground_y = groundY - 40;

        player1 = left;
        player1.position = new Vector2(60, exakter_ground_y);
        player1.starting_position = new Vector2(60, exakter_ground_y);
        player1.set_groundYs(exakter_ground_y);
        player1.GameLogic_object = this;

        player2 = right;
        player2.position = new Vector2(_graphics.PreferredBackBufferWidth - 300, exakter_ground_y);
        player2.starting_position = new Vector2(_graphics.PreferredBackBufferWidth - 300, exakter_ground_y);
        player2.set_groundYs(exakter_ground_y);
        player2.GameLogic_object = this;

    }



    // ----------------------------------------------------------------------------------------------
    // ----------------------------------------------------------------------------------------------
    // player input and colissions


    private void handle_player_movement(GameTime gameTime)
    {
        // movement and other input
        KeyboardState state = Keyboard.GetState();
        float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;


        //Player 1
        player1.handle_input(delta);


        // player 2
        // {J K L I} statt {W S A D}
        player2.handle_input(delta);


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


    private void handle_player_ball_collision(GameTime gameTime)
    {
        if (football.ice_powerUp_in_use) return;

        float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
        collisionCooldown1 -= delta;
        collisionCooldown2 -= delta;


        // Spieler 1
        if (football.getRect().Intersects(player1.currentRect) && collisionCooldown1 <= 0)
        {
            last_player_touching_the_ball = player1;
            Vector2 direction = football.position - player1.position;

            if (football.fire_powerUp_in_use == false) football.reset_velocity();
            football.change_direction(direction);
            collisionCooldown1 = CollisionCooldownTime;

            // shooting    
            if (player1.can_move)
            {
                KeyboardState state = Keyboard.GetState();
                if (state.IsKeyDown(Keys.S)) { football.get_shooted_diagonal(); }
                if (state.IsKeyDown(Keys.Y)) { football.get_shooted_horizontal(); }
            }
        }

        // Spieler 2
        if (football.getRect().Intersects(player2.currentRect) && collisionCooldown2 <= 0)
        {
            last_player_touching_the_ball = player2;
            Vector2 direction2 = football.position - player2.position;

            if (football.fire_powerUp_in_use == false) football.reset_velocity();
            football.change_direction(direction2);
            collisionCooldown2 = CollisionCooldownTime;

            // shooting    
            if (player2.can_move)
            {
                KeyboardState state = Keyboard.GetState();
                if (state.IsKeyDown(Keys.K)) { football.get_shooted_diagonal(); }
                if (state.IsKeyDown(Keys.M)) { football.get_shooted_horizontal(); }
            }

        }
    }



    private void handle_player_schuriken_collision()
    {
        Player[] players = new Player[] { player1, player2 };
        List<Schuriken> toRemove = new List<Schuriken>();

        foreach (Schuriken s in schurikenListe)
        {
            foreach (Player p in players)
            {
                if (s.current_Rect.Intersects(p.currentRect))
                {
                    if (s.owner != p)
                    {
                        p.schuriken_knockout();
                        toRemove.Add(s);
                        break;
                    }
                }
            }
        }

        foreach (var s in toRemove)
        {
            schurikenListe.Remove(s);
        }
    }


    // ------------------------------------------------------------------------------------------------------------------
    // ------------------------------------------------------------------------------------------------------------------
    // Object stuff item, Schuriken


    public void distribute_items()
    {
        // alle items jedem item-object zuordnen
        for (int i = 0; i < items.Length; i++)
        {
            items[i].set_all_items(items);
        }
    }

    public void update_all_item_positions()
    {
        //update position of all items
        for (int i = 0; i < items.Length; i++)
        {
            items[i].set_random_position();
        }
    }


    public void add_Schuriken(Vector2 pos, Player owner, int direction)
    {

        //adds a Schuriken object to the current List 
        schurikenListe.Add(new Schuriken(schuriken_texture, pos, owner, direction));
    }

    public void move_schuriken(GameTime gameTime)
    {
        foreach (Schuriken s in schurikenListe)
        {
            s.move((float)gameTime.ElapsedGameTime.TotalSeconds);
        }
    }


    public void delete_Schuriken(Schuriken schuriken_to_be_removed)
    {
        schurikenListe.Remove(schuriken_to_be_removed);
    }



    public void update_schuriken_list()
    {
        // remove all schurken that are out of the game
        schurikenListe.RemoveAll(s =>
            s.position.X >= 1800 ||
            s.position.X + s.texture_width < -10 ||
            s.position.Y < -50);
    }




    //--------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------
    // goal stuff


    //Check Ball im Tor
    private void check_for_goal()
    {
        //_leftGoalPosition und _rightGoalPosition x - / + 1/2Ballsize
        leftGoal = new Microsoft.Xna.Framework.Rectangle((int)_leftGoalPosition.X - 25, (int)_leftGoalPosition.Y, goalWidth, goalHeight);
        rightGoal = new Microsoft.Xna.Framework.Rectangle((int)_rightGoalPosition.X + 25, (int)_rightGoalPosition.Y, goalWidth, goalHeight);

        Microsoft.Xna.Framework.Rectangle ballRect = football.getRect();

        if (leftGoal.Contains(ballRect))
        {
            scorePlayer2++;
            if (scorePlayer2 >= winningScore)
            {
                gameWon = true;
                winnerText = "Player 2 wins!";
            }
            else
            {
                gameWon = false;
                reset_values_after_goal();

            }


        }


        if (rightGoal.Contains(ballRect))
        {
            scorePlayer1++;
            if (scorePlayer1 >= winningScore)
            {
                gameWon = true;
                winnerText = "Player 1 wins!";
            }
            else
            {
                gameWon = false;
                reset_values_after_goal();
            }
        }
    }

    public void reset_values_after_goal()
    {
        football.Reset_Position(new Vector2(_graphics.PreferredBackBufferWidth / 2f, groundY));
        football.reset_values();
        player1.set_back_to_starting_position();
        player2.set_back_to_starting_position();
        player1.reset_size();
        player1.reset_groundY();
        player2.reset_size();
        player2.reset_groundY();
        update_all_item_positions();
        reset_goal_size();
    }


    public void set_goal_size()
    {
        goalWidth = (int)(_goalTexture.Width * goalScale);
        goalHeight = (int)(_goalTexture.Height * goalScale);

        update_goal_positions();
    }

    public void reset_goal_size()
    {
        goalScale = goalScale_copy;
        set_goal_size();
    }

    public void update_goal_positions()
    {
        // Feste Punkte definieren
        float fixedBottomY = groundY - 780 + _goalTexture.Height; // Unterer Punkt bleibt fest
        float leftGoalInnerX = 200;  // Innerer Punkt des linken Tors (rechte Kante)
        float rightGoalInnerX = _graphics.PreferredBackBufferWidth - 200; // Innerer Punkt des rechten Tors (linke Kante)

        // Linkes Tor: Wächst nach links und oben
        // X-Position: innerer Punkt minus Breite = linke Kante
        // Y-Position: unterer Punkt minus Höhe = obere Kante  
        _leftGoalPosition = new Vector2(leftGoalInnerX - goalWidth, fixedBottomY - goalHeight);

        // Rechtes Tor: Wächst nach rechts und oben
        // X-Position: innerer Punkt = linke Kante (Tor wächst nach rechts)
        // Y-Position: unterer Punkt minus Höhe = obere Kante
        _rightGoalPosition = new Vector2(rightGoalInnerX, fixedBottomY - goalHeight);
    }
}

