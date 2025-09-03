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
 * ---------------------------------------------------------------------------------------------------------------------
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;
using System.Diagnostics;
using System.Collections.Generic;

using our_Game;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.CompilerServices;
//Sound
using Microsoft.Xna.Framework.Media; 
using Microsoft.Xna.Framework.Audio; 


public class GameLogic : GameState
{
    //Sounds
    Song background_sound;
    private SoundEffect goalSound;
    private SoundEffect iceSound;
    private SoundEffect FansByGoalSound;
    private SoundEffect CoinSound;

    private SoundEffect teleportationSound;
    private SoundEffect marioSound;

    //Spielfeld
    private Texture2D _backgroundTexture;
    private Texture2D _goalTexture;
    private Vector2 _leftGoalPosition;
    private Vector2 _rightGoalPosition;

    //player, ball
    private Player player1;
    private Player player2;
    private Player last_player_touching_the_ball = null;
    private Ball football;
    private bool randomPlayer1 = false;  //is player1.type == RandomPlayer ?
    private bool randomPlayer2 = false;  //is player2.type == randomPlayer?
    
    //collision
    private float collisionCooldown1 = 0f;
    private float collisionCooldown2 = 0f;
    private const float CollisionCooldownTime = 0.05f;

    Texture2D vs_zeichen;
    Texture2D red_window;

    public Dictionary<string, Texture2D> ball_textures;

    private static Random rng = new Random();

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
    private string winnerText = "";
    private Texture2D overlayTexture;

    private float gameTimer = 0f;
    private bool gameRunning = true;

    // item
    public Item item1;
    public Item[] items;

    // Shuriken
    List<Schuriken> schurikenListe = new List<Schuriken>();
    Texture2D schuriken_texture;

    //panzer
    List<Panzer> panzerListe = new List<Panzer>();
    Texture2D panzer_texture;

    //Goomba
    List<Goomba> goombaListe = new List<Goomba>();
    Texture2D goomba_texture;
    int number_of_goombas_in_the_game = 2;

    //animations
    public Texture2D t1;   //wizzard teleportarion
    public Texture2D s1;  // stun - knckout animation

    private Texture2D _tribuneTexture;
    private Vector2 _leftTribunePosition;

    float tribuneScale = 1f;
    int tribuneWidth;
    int tribuneHeight;
    float greenAreaY;

    // Fan Characters
    private Texture2D fanrotTexture;
    private Texture2D fanrotBlau1Texture;
    private Texture2D fanrotBlau2Texture;
    private Texture2D fanrot2Texture;
    private List<BackgroundFan> backgroundFans = new List<BackgroundFan>();

    //Anstoßfeatures
    public bool specialModesEnabled = true;   // Masterschalter um Anstoßfeature an oder aus zu machen            TODO: Button im Menu hinzufügen
    private enum RoundMode { Normal, GoombaMode, WallFrontGoals, WallButtonTrigger, MovingWall,}
    private RoundMode currentMode = RoundMode.Normal;
    private Texture2D holzwandTexture;

    // Mode zerstörbare Wand
    private int leftWallHP = 0;
    private int rightWallHP = 0;
    private const int MaxWallHits = 3;

    
    //button für Wand-Trigger-Modus
    private Texture2D buttonTexture;
    private int buttonSize = 64;
    private Rectangle leftButtonRect;
    private Rectangle rightButtonRect;
    private bool leftWallActive = false;
    private bool rightWallActive = false;

    // Moving Wall (vertikale Schieberiegel vor den Toren)
    private int movingWallThickness = 10;
    private int movingWallHeight = 140;   
    private float movingWallSpeed = 110f;           //speed

    private float leftWallY;   
    private float rightWallY;  
    private int leftWallDir = 1;   
    private int rightWallDir = -1; 


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
        PlayerFactory.Initialize(_graphicsDevice, Content);


        ball_textures = new()
        {
            { "football" ,    Content.Load<Texture2D>("balls/football")     },
            { "firefootball", Content.Load<Texture2D>("balls/firefootball") },
            { "icefootball",  Content.Load<Texture2D>("balls/icefootball")  }
        };

        football = new Ball(_graphicsDevice, new Vector2(930, groundY), ball_textures);

        randomPlayer1 = is_RandomPlayer(player1);
        randomPlayer2 = is_RandomPlayer(player2);
        if (randomPlayer1) { player_becomes_new_random_player(player1, 1); }
        if (randomPlayer2) { player_becomes_new_random_player(player2, 2); }
        set_other_players();

        item1 = new Item(_graphicsDevice, Content, football);
        items = new Item[] { item1 };
        distribute_items();
        update_all_item_positions();

        // DON'T Initialize background fans here - they need textures first!
        // InitializeBackgroundFans(); - moved to LoadContent()
    }


    //----------------------------------------------------------------------------------------
    public void SetPlayer(Player left, Player right)
    {
        SetPlayer1(left);
        SetPlayer2(right);
    }

    public void SetPlayer1(Player left)
    {
        float exakter_ground_y = groundY - 40;

        player1 = left;
        player1.position = new Vector2(250, exakter_ground_y);
        player1.starting_position = new Vector2(250, exakter_ground_y);
        player1.set_groundYs(exakter_ground_y);
        player1.GameLogic_object = this;
        player1.set_content(Content);
        player1.Set_other_Player(player2);
    }

    public void SetPlayer2(Player right)
    {
        float exakter_ground_y = groundY - 40;

        player2 = right;
        player2.position = new Vector2(_graphics.PreferredBackBufferWidth - 400, exakter_ground_y);
        player2.starting_position = new Vector2(_graphics.PreferredBackBufferWidth - 400, exakter_ground_y);
        player2.set_groundYs(exakter_ground_y);
        player2.GameLogic_object = this;
        player2.set_content(Content);
        player2.Set_other_Player(player1);
    }

    public PlayerFactory.Types getRandomPlayerType()
    {
        //return randomPlayertype that is not from class RandomPlayer
        int n = rng.Next(1, PlayerFactory.TypesCount);
        if ((PlayerFactory.Types)n == PlayerFactory.Types.RandomPlayer)
        {
            return getRandomPlayerType();
        }
        return (PlayerFactory.Types)n;
    }

    public void player_becomes_new_random_player(Player abc, int id)
    {
        // hinweis: id is the playergroup 

        PlayerFactory.Types luna = getRandomPlayerType();
        abc = PlayerFactory.CreatePlayer(luna, abc.position, id, abc.controls);

        if (id == 1) { SetPlayer1(abc); }
        else { SetPlayer2(abc); }
    }

    public void set_other_players()
    {
        player1.Set_other_Player(player2);
        player2.Set_other_Player(player1);
    }


    //-----------------------------------------------------------------------------------

    public override void LoadContent()
    {
        spriteBatch = new Microsoft.Xna.Framework.Graphics.SpriteBatch(_graphicsDevice);
        _backgroundTexture = Content.Load<Texture2D>("spielfeld_pictures/Spielfeld3");

        goalSound = Content.Load<SoundEffect>("sounds/goal");
        iceSound  = Content.Load<SoundEffect>("sounds/ice2");
        CoinSound = Content.Load<SoundEffect>("sounds/coin1");
        FansByGoalSound     = Content.Load<SoundEffect>("sounds/FansByGoal");
        teleportationSound  = Content.Load<SoundEffect>("sounds/wizzard_sound2");
        marioSound          = Content.Load<SoundEffect>("sounds/mario_sound");

        background_sound = Content.Load<Song>("sounds/crowd_noise");
        MediaPlayer.IsRepeating = true;
        MediaPlayer.Volume = 0.1f;       
        MediaPlayer.Play(background_sound);

        _goalTexture = Content.Load<Texture2D>("Tore");
        set_goal_size();
        update_goal_positions();

        scoreFont = Content.Load<SpriteFont>("Arial");

        _tribuneTexture = Content.Load<Texture2D>("TribueneLang");
        tribuneWidth = (int)(_tribuneTexture.Width * tribuneScale);
        tribuneHeight = (int)(_tribuneTexture.Height * tribuneScale);

        greenAreaY = groundY + 50;


        _leftTribunePosition = new Vector2(450, -100);

        panzer_texture = Content.Load<Texture2D>("Panzer");
        schuriken_texture = Content.Load<Texture2D>("shuriken");
        goomba_texture    = Content.Load<Texture2D>("goomba");
        vs_zeichen = Content.Load<Texture2D>("vs_zeichen");
        red_window = Content.Load<Texture2D>("red_window");

        overlayTexture = new Texture2D(_graphicsDevice, 1, 1);
        overlayTexture.SetData(new[] { Color.White });

        t1 = Content.Load<Texture2D>("animation_p1");   // teleportation animation wizzard 
        s1 = Content.Load<Texture2D>("stun7");         //  stun - knockoput animation

        // Load fan textures
        fanrotTexture = Content.Load<Texture2D>("fans/FanRot1");
        fanrotBlau1Texture = Content.Load<Texture2D>("fans/FanBlau1");
        fanrotBlau2Texture = Content.Load<Texture2D>("fans/FanBlau2");
        fanrot2Texture = Content.Load<Texture2D>("fans/FanRot2");

        holzwandTexture = Content.Load<Texture2D>("Holzwand");
        buttonTexture = Content.Load<Texture2D>("Button");

        InitializeBackgroundFans();
    }

    private void InitializeBackgroundFans()
    {
        // Linke Seite Fans (Rot) - mit FanRot1 normal, FanRot2 excited
        backgroundFans.Add(new BackgroundFan(fanrotTexture, fanrot2Texture, new Vector2(500, 300), 0.5f, 2f, 0.3f));
        backgroundFans.Add(new BackgroundFan(fanrotTexture, fanrot2Texture, new Vector2(700, 230), 0.5f, 2f, 0.2f));
        backgroundFans.Add(new BackgroundFan(fanrotTexture, fanrot2Texture, new Vector2(600, 370), 0.5f, 2f, 0.4f));
        backgroundFans.Add(new BackgroundFan(fanrotTexture, fanrot2Texture, new Vector2(800, 300), 0.5f, 2f, 0.1f));

        // Rechte Seite Fans (Blau) - mit FanBlau1 normal, FanBlau2 excited
        backgroundFans.Add(new BackgroundFan(fanrotBlau1Texture, fanrotBlau2Texture, new Vector2(980, 300), 0.5f, 2f, 0.3f));
        backgroundFans.Add(new BackgroundFan(fanrotBlau1Texture, fanrotBlau2Texture, new Vector2(1200, 230), 0.5f, 2f, 0.4f));
        backgroundFans.Add(new BackgroundFan(fanrotBlau1Texture, fanrotBlau2Texture, new Vector2(1300, 370), 0.5f, 2f, 0.1f));
        backgroundFans.Add(new BackgroundFan(fanrotBlau1Texture, fanrotBlau2Texture, new Vector2(1150, 300), 0.5f, 2f, 0.2f));
    }

    public Ball getBall()
    {
        return football;
    }

    public bool is_RandomPlayer(Player abc)
    {
        // checks whether a player object is from the class RandomPlayer
        return (abc is RandomPlayer);
    }

    public override void Update(GameTime gameTime)
    {
        player1.update_values();
        player2.update_values();

        player1.update_knockout_phase();
        player2.update_knockout_phase();

        handle_player_movement(gameTime);
        handle_player_ball_collision(gameTime);

        player1.update_vertical((float)gameTime.ElapsedGameTime.TotalSeconds);
        player2.update_vertical((float)gameTime.ElapsedGameTime.TotalSeconds);

        football.move((float)gameTime.ElapsedGameTime.TotalSeconds, groundY);

        BlockPlayerAgainstWalls(player1);
        BlockPlayerAgainstWalls(player2);


        handle_player_coin_colission();
        handle_ball_coin_collision();
        handle_player_schuriken_collision();
        handle_player_goomba_collision();

        if (!gameWon)
        {
            check_for_goal();
        }

        player1.reset_powerUps_if_time_is_over();
        player2.reset_powerUps_if_time_is_over();
        football.reset_powerUps_if_time_is_over();

        player1.update_special_effect_in_use();
        player2.update_special_effect_in_use();

        move_schuriken(gameTime);
        update_schuriken_list();
        move_Panzer(gameTime);
        update_panzer_list();
        update_goombas(gameTime);
        

        // Update background fans
        foreach (var fan in backgroundFans)
        {
            fan.Update(gameTime);
        }

        //Zurück ins Menu wenn ESC losgelassen wird 
        if (InputHandler.IsReleased(Keys.Escape))
        {
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
                Game1.openGames.Remove(this);
                Game1.nextState = new Menu(baseGame);
            }
            return;
        }

        UpdateMovingWalls((float)gameTime.ElapsedGameTime.TotalSeconds);
    }


    // -----------------------------------------------------------------------------------------
    //Draws 

    public override void Draw(GameTime gameTime)
    {
        _graphicsDevice.Clear(Microsoft.Xna.Framework.Color.CornflowerBlue);
        spriteBatch.Begin();

        spriteBatch.Draw(
        _backgroundTexture,
        new Microsoft.Xna.Framework.Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight),
        Microsoft.Xna.Framework.Color.White
        );
        spriteBatch.Draw(_tribuneTexture,
        new Microsoft.Xna.Framework.Rectangle((int)_leftTribunePosition.X, (int)_leftTribunePosition.Y, tribuneWidth, tribuneHeight),
        Microsoft.Xna.Framework.Color.White);
        // Draw background fans
        foreach (var fan in backgroundFans)
        {
            fan.Draw(spriteBatch);
        }
        Color gameColor = gameWon ? Color.White * 0.3f : Color.White;


        football.draw(spriteBatch, gameTime);
        //goals
        spriteBatch.Draw(_goalTexture, new Microsoft.Xna.Framework.Rectangle((int)_leftGoalPosition.X, (int)_leftGoalPosition.Y, goalWidth, goalHeight), null, Microsoft.Xna.Framework.Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
        spriteBatch.Draw(_goalTexture, new Microsoft.Xna.Framework.Rectangle((int)_rightGoalPosition.X, (int)_rightGoalPosition.Y, goalWidth, goalHeight), Microsoft.Xna.Framework.Color.White);

        player1.draw(spriteBatch, gameTime);
        player2.draw(spriteBatch, gameTime);
        
        foreach (Item i in items)                { i.draw(spriteBatch, gameTime); }  //draw items
        foreach (Schuriken s in schurikenListe)  { s.draw(spriteBatch, gameTime); }  //draw Shiruken 
        foreach (Panzer p in panzerListe)        { p.draw(spriteBatch);           }  //draw Panzer       

        //draw the Interface under ground Y
        Draw_i_dont_know();

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



        switch (currentMode)
        {
            case RoundMode.WallFrontGoals:
                // HP-Modus: Wände sind sichtbar, solange HP > 0
                if (leftWallHP > 0)
                {
                    var leftRectWall = new Rectangle(leftGoal.Right + 1, leftGoal.Y, 10, leftGoal.Height);
                    spriteBatch.Draw(holzwandTexture, leftRectWall, Color.White);
                }
                if (rightWallHP > 0)
                {
                    var rightRectWall = new Rectangle(rightGoal.Left - 11, rightGoal.Y, 10, rightGoal.Height);
                    spriteBatch.Draw(holzwandTexture, rightRectWall, Color.White);
                }
                break;

            case RoundMode.WallButtonTrigger:
                
              

                if (leftWallActive)
                {
                    var leftRectWall = new Rectangle(leftGoal.Right + 1, leftGoal.Y, 10, leftGoal.Height);
                    spriteBatch.Draw(holzwandTexture, leftRectWall, Color.White);
                }
                if (rightWallActive)
                {
                    var rightRectWall = new Rectangle(rightGoal.Left - 11, rightGoal.Y, 10, rightGoal.Height);
                    spriteBatch.Draw(holzwandTexture, rightRectWall, Color.White);
                }


                if (leftWallActive)
                    spriteBatch.Draw(buttonTexture, leftButtonRect, Color.White);
                if (rightWallActive)
                    spriteBatch.Draw(buttonTexture, rightButtonRect, Color.White);
                break;
                

            case RoundMode.MovingWall:
                
                var leftRect = new Rectangle(leftGoal.Right + 1, (int)leftWallY, movingWallThickness, movingWallHeight);
                var rightRect = new Rectangle(rightGoal.Left - movingWallThickness - 1, (int)rightWallY, movingWallThickness, movingWallHeight);
                spriteBatch.Draw(holzwandTexture, leftRect, Color.White);
                spriteBatch.Draw(holzwandTexture, rightRect, Color.White);
                break;

            case RoundMode.GoombaMode:
                foreach (Goomba abc in goombaListe)
                    abc.draw(spriteBatch);
                break;
        }



        spriteBatch.End();
    }


    void DrawPowerUp(Texture2D texture, Rectangle area, int size)
    {
        //size = size of the PowerUptexture

        spriteBatch.Draw(red_window, area, Color.White);
        if (texture != null)
        {
            spriteBatch.Draw(texture, new Rectangle(area.X + 15, area.Y + 15, size, size), Color.White);
        }
    }

    void DrawPlayerSpecialMoveTexture(Texture2D tex, Vector2 centerPosition, int desiredSize, Player abc)
    {
        float alpha = 0.4f;
        if (abc.can_do_special_effect() || abc.is_using_specialeffect)
        {
            alpha = 1f;
        }

            float baseScale = (float)desiredSize / Math.Max(tex.Width, tex.Height);
            float scale = baseScale * (abc.is_using_specialeffect ? 1.3f : 1f);

            Vector2 origin = new Vector2(tex.Width / 2f, tex.Height / 2f);

            spriteBatch.Draw(
                tex,
                centerPosition,
                null,
                Color.White * alpha,
                0f,
                origin,
                scale,
                SpriteEffects.None,
                0f
            );
    }


    public void Draw_i_dont_know()
    {
        /*  i dont know how to name this function
         
            draw vs zeichen, special effect textures, powerup textures
            to make sure, channging one value( f.e. the vs sign shuld be smaller) does not force a
            a change of the other values , the values depend from each other.
            calculation values and drawing textures goes from middle to outside
        */

        int middle = 1920;

        // draw the texture in the middle of the screen
        float scaling = 0.5f;
        int x_size = (int)(vs_zeichen.Width * scaling);
        int y_size = (int)(vs_zeichen.Height * scaling);
        int x_pos = (int)(middle / 2 - x_size / 2);
        int y_pos = 800;
        spriteBatch.Draw(vs_zeichen, new Rectangle(x_pos, y_pos, x_size, y_size), Color.White);


        // draw player special_move_textures
        int distance2 = 140;                       // whished_distanze_between_vs_and_special_move_texture
        int size2 = 140;                           // size of the round character_symbol_textures
        int x_pos21 = x_pos - distance2 - size2;   // position of left character symbol
        int x_pos22 = x_pos + distance2 + x_size;  // pposition of right character symbol
        DrawPlayerSpecialMoveTexture(player1.special_move_texture, new Vector2(x_pos21 + size2 / 2, 800 + size2 / 2), size2, player1);
        DrawPlayerSpecialMoveTexture(player2.special_move_texture, new Vector2(x_pos22 + size2 / 2, 800 + size2 / 2), size2, player2);

        //draw player powerUp_textures
        int size3 = 130;   //size of backgroudn texture
        int size4 = 100;   //size of the real PowerUp_texture 
        int distance3 = 20;  //distance to the specialeffect symbol
        int xpos31 = x_pos21 - distance3 - size3;
        int xpos32 = x_pos22 + distance3 + size3;

        DrawPowerUp(player1.powerup1?.get_powerUp_texture(), new Rectangle(xpos31, 750, size3, size3), size4);
        DrawPowerUp(player1.powerup2?.get_powerUp_texture(), new Rectangle(xpos31, 910, size3, size3), size4);
        DrawPowerUp(player2.powerup1?.get_powerUp_texture(), new Rectangle(xpos32, 750, size3, size3), size4);
        DrawPowerUp(player2.powerup2?.get_powerUp_texture(), new Rectangle(xpos32, 910, size3, size3), size4);

        //draw the text of the Mode
        string modeText = currentMode switch
        {
            RoundMode.WallFrontGoals =>    "Mode: Wand vor dem Tor",
            RoundMode.WallButtonTrigger => "Mode: Triff den Knopf",
            RoundMode.MovingWall =>        "Mode: MovingWall",
            RoundMode.GoombaMode =>        "Mode: Goombas ausweichen",
            _ => string.Empty
        };

        if (string.IsNullOrEmpty(modeText) == false)
        {
            Vector2 size = scoreFont.MeasureString(modeText);
            spriteBatch.DrawString(
                scoreFont,
                modeText,
                new Vector2((_graphics.PreferredBackBufferWidth - size.X) / 2, 720),
                Color.Yellow
            );
        }
    }


    // ----------------------------------------------------------------------------------------------
    // ----------------------------------------------------------------------------------------------
    // player input and colissions


    private void handle_player_movement(GameTime gameTime)
    {
        // movement and other input
        KeyboardState state = Keyboard.GetState();
        float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

        player1.handle_input(delta);
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
                itemA.set_random_position(player1, player2);
                playCollectingCoinSound();

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
                itemA.set_random_position(player1, player2);
                playCollectingCoinSound();
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
                if (player1.shoots_diagonal) { football.get_shooted_diagonal(); }
                if (player1.shoots_horizontal) { football.get_shooted_horizontal(); }
                if (player1.shoots_lupfer) { football.get_shooted_lupfer(); }

                //todo: auslagern in PlayerControls/ Input handler /playeer.handleInput()
                //drücken um player.shoot...  auf true zu setzen
                //loslassen um player.shoot...auf false zu setzen
                KeyboardState state = Keyboard.GetState();
                if (state.IsKeyDown(Keys.S)) { football.get_shooted_diagonal(); }
                if (state.IsKeyDown(Keys.X)) { football.get_shooted_horizontal(); }
                if (state.IsKeyDown(Keys.C)) { football.get_shooted_lupfer(); }
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
                if (player2.shoots_diagonal) { football.get_shooted_diagonal(); }
                if (player2.shoots_horizontal) { football.get_shooted_horizontal(); }
                if (player2.shoots_lupfer) { football.get_shooted_lupfer(); }

                //todo auslaegrn in PlayerControls /InputHandler/ player.handleInput()
                //drücken um player.shoot...  auf true zu setzen
                //loslassen um player.shoot...auf false zu setzen
                KeyboardState state = Keyboard.GetState();
                if (state.IsKeyDown(Keys.K)) { football.get_shooted_diagonal(); }
                if (state.IsKeyDown(Keys.M)) { football.get_shooted_horizontal(); }
                if (state.IsKeyDown(Keys.OemPeriod)) { football.get_shooted_lupfer(); }
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
                        p.getKnockout_by_schuriken();
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


    private void handle_player_goomba_collision()
    {
        if (currentMode != RoundMode.GoombaMode) { return; }
        
        Player[] players = new Player[] { player1, player2 };
        foreach (Goomba goomba in goombaListe)
        {
            foreach (Player p in players)
            {
                if (p.currentRect.Intersects(goomba.current_rect))
                {
                    p.getKnockout_by_goomba();
                }
            }
        }
    }



    // ------------------------------------------------------------------------------------------------------------------
    // ------------------------------------------------------------------------------------------------------------------
    // Object stuff item, Schuriken, Panzer

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
            items[i].set_random_position(player1, player2);
        }
    }

    //----------------------------------
    //Schuriken
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

    public void update_schuriken_list()
    {
        // remove all schurken that are out of the game
        schurikenListe.RemoveAll(s =>
            s.position.X >= 1800 ||
            s.position.X + s.texture_width < -10 ||
            s.position.Y < -50);
    }

    //----------------------------------
    //Panzer
    public void add_Panzer(Vector2 pos, Player owner, int direction)
    {
        //adds a Schuriken object to the current List 
        panzerListe.Add(new Panzer(panzer_texture, pos, owner, direction));
    }

    public void move_Panzer(GameTime gameTime)
    {
        foreach (Panzer p in panzerListe)
        {
            p.move((float)gameTime.ElapsedGameTime.TotalSeconds);
        }
    }

    public void update_panzer_list()
    {
        // remove all schurken that are out of the game
        panzerListe.RemoveAll(s =>
            s.position.X >= 1800 ||
            s.position.X + s.texture_width < -10 ||
            s.position.Y < -50);
    }
    

    //----------------------------------
    //Goombas
    public void update_goombas(GameTime gameTime)
    {
        foreach (Goomba abc in goombaListe)
        {
            abc.move((float)gameTime.ElapsedGameTime.TotalSeconds);

            if (abc.position.X < abc.left_bounder || abc.position.X > abc.right_bounder){
                abc.change_direction();
            }
        }
    }


    //--------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------
    // goal stuff


    //Check Ball im Tor
    private void check_for_goal()
    {
        int crossbarHeight = 20;
        Rectangle ballRect = football.getRect();

        leftGoal = new Rectangle((int)_leftGoalPosition.X - 25, (int)_leftGoalPosition.Y + crossbarHeight, goalWidth, goalHeight - crossbarHeight);
        rightGoal = new Rectangle((int)_rightGoalPosition.X + 25, (int)_rightGoalPosition.Y + crossbarHeight, goalWidth, goalHeight - crossbarHeight);

        Rectangle leftCrossbar = new Rectangle(leftGoal.X, leftGoal.Y - crossbarHeight, leftGoal.Width, crossbarHeight);
        Rectangle rightCrossbar = new Rectangle(rightGoal.X, rightGoal.Y - crossbarHeight, rightGoal.Width, crossbarHeight);

        football.handle_crossbar_collision(leftCrossbar);
        football.handle_crossbar_collision(rightCrossbar);

        // ----- Front-Wände je nach Modus -----
        int wallThickness = 10;

        if (currentMode == RoundMode.WallFrontGoals)
        {
            if (leftWallHP > 0)
            {
                Rectangle leftWall = new Rectangle(leftGoal.Right + 1, leftGoal.Y, wallThickness, leftGoal.Height);
                if (football.handle_goal_front_wall_collision(leftWall)) { leftWallHP--; }
            }
            if (rightWallHP > 0)
            {
                Rectangle rightWall = new Rectangle(rightGoal.Left - wallThickness - 1, rightGoal.Y, wallThickness, rightGoal.Height);
                if (football.handle_goal_front_wall_collision(rightWall)) { rightWallHP--; }
            }
        }
        else if (currentMode == RoundMode.WallButtonTrigger)
        {
            int offsetY = 120, offsetX = 40;
            leftButtonRect = new Rectangle(leftGoal.Right + offsetX, leftGoal.Y - offsetY, buttonSize, buttonSize);
            rightButtonRect = new Rectangle(rightGoal.Left - offsetX - buttonSize, rightGoal.Y - offsetY, buttonSize, buttonSize);

            // Wand abprallen lassen, solange aktiv
            if (leftWallActive)
            {
                Rectangle leftWall = new Rectangle(leftGoal.Right + 1, leftGoal.Y, 10, leftGoal.Height);
                football.handle_goal_front_wall_collision(leftWall);
            }
            if (rightWallActive)
            {
                Rectangle rightWall = new Rectangle(rightGoal.Left - 11, rightGoal.Y, 10, rightGoal.Height);
                football.handle_goal_front_wall_collision(rightWall);
            }

            // Button-Kollision prüfen
            if (leftWallActive && football.getRect().Intersects(leftButtonRect))
            {
                leftWallActive = false;
                Debug.WriteLine("Linke Wand zerstört (Button getroffen).");
            }
            if (rightWallActive && football.getRect().Intersects(rightButtonRect))
            {
                rightWallActive = false;
                Debug.WriteLine("Rechte Wand zerstört (Button getroffen).");
            }
        }
        else if (currentMode == RoundMode.MovingWall)
        {
            // Wand-Segmente vor die Tore (vertikale Balken, die hoch/runter fahren)
            Rectangle leftWall = new Rectangle(
                leftGoal.Right + 1,
                (int)leftWallY,
                movingWallThickness,
                movingWallHeight
            );

            Rectangle rightWall = new Rectangle(
                rightGoal.Left - movingWallThickness - 1,
                (int)rightWallY,
                movingWallThickness,
                movingWallHeight
            );

            // Ball daran abprallen lassen
            football.handle_goal_front_wall_collision(leftWall);
            football.handle_goal_front_wall_collision(rightWall);
        }

        if (leftGoal.Contains(ballRect))
        {
            scorePlayer2++;
            TriggerGoalAnimation(2);
            playGoalSound();
            playFansByGoalSound();
            if (scorePlayer2 >= winningScore) { gameWon = true; winnerText = "Player 2 wins!"; }
            else { gameWon = false; reset_values_after_goal(); }
        }

        if (rightGoal.Contains(ballRect))
        {
            scorePlayer1++;
            TriggerGoalAnimation(1);
            playGoalSound();
            playFansByGoalSound();
            if (scorePlayer1 >= winningScore) { gameWon = true; winnerText = "Player 1 wins!"; }
            else { gameWon = false; reset_values_after_goal(); }
        }
    }

    private void TriggerGoalAnimation(int scoringTeam)
    {
        // Team 1 = linke Fans (Index 0-3), Team 2 = rechte Fans (Index 4-7)
        if (scoringTeam == 1)
        {
            // Linke Fans jubeln (rote Fans)
            for (int i = 0; i < 4 && i < backgroundFans.Count; i++)
            {
                backgroundFans[i].StartGoalAnimation();
            }
        }
        else if (scoringTeam == 2)
        {
            // Rechte Fans jubeln (blaue Fans)
            for (int i = 4; i < 8 && i < backgroundFans.Count; i++)
            {
                backgroundFans[i].StartGoalAnimation();
            }
        }
    }

    private void StopAllGoalAnimations()
    {
        foreach (var fan in backgroundFans)
        {
            fan.StopGoalAnimation();
        }
    }


    public void reset_values_after_goal()
    {
        changeKickOffMode();

        //change player character if randomPlayer
        if (randomPlayer1)
        {
            player_becomes_new_random_player(player1, 1);
            SetPlayer1(player1);
        }
        if (randomPlayer2)
        {
            player_becomes_new_random_player(player2, 2);
            SetPlayer2(player2);
        }
        set_other_players();


        football.Reset_Position(new Vector2(_graphics.PreferredBackBufferWidth / 2f, groundY));
        football.reset_values();
        player1.set_back_to_starting_position();
        player2.set_back_to_starting_position();
        player1.reset_size();
        player2.reset_size();
        player1.reset_groundY();
        player2.reset_groundY();
        player1.reset_MoveChange_after_MoveChangePowerUp_is_over();
        player2.reset_MoveChange_after_MoveChangePowerUp_is_over();
        update_all_item_positions();
        reset_goal_size();

        set_other_players();
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

    private void UpdateMovingWalls(float dt)
    {
        if (currentMode != RoundMode.MovingWall) return;

        int crossbarHeight = 20;

        // Linkes Tor: erlaubter Bewegungsbereich
        int leftTop = (int)_leftGoalPosition.Y + crossbarHeight;
        int leftBottomMax = leftTop + (goalHeight - crossbarHeight) - movingWallHeight;

        leftWallY += movingWallSpeed * dt * leftWallDir;
        if (leftWallY <= leftTop) { leftWallY = leftTop; leftWallDir = 1; }
        if (leftWallY >= leftBottomMax) { leftWallY = leftBottomMax; leftWallDir = -1; }

        // Rechtes Tor: Bereich
        int rightTop = (int)_rightGoalPosition.Y + crossbarHeight;
        int rightBottomMax = rightTop + (goalHeight - crossbarHeight) - movingWallHeight;

        rightWallY += movingWallSpeed * dt * rightWallDir;
        if (rightWallY <= rightTop) { rightWallY = rightTop; rightWallDir = 1; }
        if (rightWallY >= rightBottomMax) { rightWallY = rightBottomMax; rightWallDir = -1; }
    }

    public void changeKickOffMode()
    {
        if (!specialModesEnabled)
        {
            currentMode = RoundMode.Normal;
            return;
        }


        RoundMode[] modes = (RoundMode[])Enum.GetValues(typeof(RoundMode));  //make array of enum 
        int index = rng.Next(modes.Length);
        currentMode = modes[index];

        // Zustand pro Modus vorbereiten
        if (currentMode == RoundMode.WallFrontGoals) // dein HP-Modus
        {
            leftWallHP = MaxWallHits;
            rightWallHP = MaxWallHits;
            leftWallActive = false;
            rightWallActive = false;
        }
        else if (currentMode == RoundMode.WallButtonTrigger)
        {
            // Wände sind aktiv bis ein Button getroffen wird
            leftWallActive = true;
            rightWallActive = true;
            // (HP ignorieren)
            leftWallHP = 0;
            rightWallHP = 0;
        }
        else if (currentMode == RoundMode.MovingWall)
        {
            // Wand-Segmente mittig im Tor starten
            int crossbarHeight = 20;
            int leftTop = (int)_leftGoalPosition.Y + crossbarHeight;
            int rightTop = (int)_rightGoalPosition.Y + crossbarHeight;

            int usableH = goalHeight - crossbarHeight;
            leftWallY = leftTop + (usableH - movingWallHeight) * 0.5f;
            rightWallY = rightTop + (usableH - movingWallHeight) * 0.5f;

            leftWallDir = 1;
            rightWallDir = -1;
            leftWallHP = 0; rightWallHP = 0;
            leftWallActive = false; rightWallActive = false;
        }
        else if (currentMode == RoundMode.GoombaMode)
        {
            goombaListe.Clear(); // alte Goombas entfernen
            for (int i = 0; i < number_of_goombas_in_the_game; i++)
            {
                int direction = (i < number_of_goombas_in_the_game / 2) ? 1 : -1;
                goombaListe.Add(new Goomba(goomba_texture, (550 + i * 370), groundY, direction));
            }
        }

        else // Normal
        {
            leftWallHP = 0; rightWallHP = 0;
            leftWallActive = false; rightWallActive = false;
        }
    }

    private IEnumerable<Rectangle> GetBlockingWallsForPlayers()
    {
        int wallThickness = 10;
        if (currentMode == RoundMode.WallFrontGoals)
        {
            if (leftWallHP > 0)
                yield return new Rectangle(leftGoal.Right + 1, leftGoal.Y, wallThickness, leftGoal.Height);
            if (rightWallHP > 0)
                yield return new Rectangle(rightGoal.Left - wallThickness - 1, rightGoal.Y, wallThickness, rightGoal.Height);
        }
        else if (currentMode == RoundMode.WallButtonTrigger)
        {
            if (leftWallActive)
                yield return new Rectangle(leftGoal.Right + 1, leftGoal.Y, wallThickness, leftGoal.Height);
            if (rightWallActive)
                yield return new Rectangle(rightGoal.Left - wallThickness - 1, rightGoal.Y, wallThickness, rightGoal.Height);
        }
    }

    private void BlockPlayerAgainstWalls(Player p)
    {
        // Für MovingWall NICHT blocken
        if (currentMode == RoundMode.MovingWall) return;

        foreach (var wall in GetBlockingWallsForPlayers())
        {
            if (!p.currentRect.Intersects(wall)) continue;

            // Auflösung entlang der kleineren Überlappung
            var a = p.currentRect;
            float overlapX = Math.Min(a.Right, wall.Right) - Math.Max(a.Left, wall.Left);
            float overlapY = Math.Min(a.Bottom, wall.Bottom) - Math.Max(a.Top, wall.Top);

            Vector2 pos = p.position;

            if (overlapX <= overlapY)
            {
                // seitlich abblocken
                if (a.Center.X < wall.Center.X)
                    pos.X = wall.Left - p.RectangleWidth - 1;     // von links blocken
                else
                    pos.X = wall.Right + 1;                       // von rechts blocken
            }
            else
            {
                // vertikal abblocken (falls er irgendwie "in" die Wand fällt)
                if (a.Center.Y < wall.Center.Y)
                    pos.Y = wall.Top - p.RectangleHeight - 1;     // von oben
                else
                    pos.Y = wall.Bottom + 1;                      // von unten
            }

            p.position = pos;
            p.update_rectangles();
            // Falls er mehrere Wände schneidet, erneut prüfen
        }
    }

    //-----------------------------------------------------------------------------------------------
    //sounds

    public void playGoalSound()
    {
        SoundEffectInstance goalSoundInstance = goalSound.CreateInstance();
        goalSoundInstance.Volume = 1.0f; 
        goalSoundInstance.Play();
    }

    public void playIceSound()
    {
        SoundEffectInstance iceSounInstance = iceSound.CreateInstance();
        iceSounInstance.Volume = 0.8f;
        iceSounInstance.Play();
    }

    public void playFansByGoalSound()
    {
        //Fans cheering after a player has scored a goal
        
        SoundEffectInstance FansByGoal = FansByGoalSound.CreateInstance();
        FansByGoal.Volume = 0.3f;

        //soundlänge in abhängikeit von der fan animation skalieren
        //double originalLength = FansByGoalSound.Duration.TotalSeconds;
        //double targetLength = 5.0;  // = fan.animationDuration
        //float pitch = (float)((originalLength / targetLength) -1.0);
        //pitch = Math.Clamp(pitch, -1.0f, 1.0f);
        //FansByGoal.Pitch = pitch;
        
        FansByGoal.Play();
    }

    public void playCollectingCoinSound()
    {
        SoundEffectInstance cc = CoinSound.CreateInstance();
        cc.Volume = 0.5f;
        cc.Play();
    }

    public void playTeleportationSound()
    {
        SoundEffectInstance cc = teleportationSound.CreateInstance();
        cc.Volume = 0.7f;
        cc.Play();
    }

    public void playMarioSound()
    {
        SoundEffectInstance cc = marioSound.CreateInstance();
        cc.Volume = 0.1f;
        cc.Play();
    }
}


