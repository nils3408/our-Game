//nils, Lukas


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;


public class Player
{

    public Texture2D texture;
    public Texture2D special_move_texture;

    public Player otherPlayer;
    public int playerGroup;
    
    public bool can_move = true;
    public float schuriken_knockout_time = 3f;  // time a player is knocked out after getting hit from a Schuriken
    public float goomba_knockout_time = 1.1f;    //time a player is knocked out after getting hit from a goomba
    public float knockout_time = 0;
    public DateTime hitting_time = DateTime.MinValue;  //when player got hit by a schuriken or a goomba

    
    public bool is_teleporting = false;

    public float move_speed = 380f;
    public float move_speed2;  //copy if move_speed gets changed (for example in sonic character)
    
    public float jump_velocity = -500f;
    public float jump_velocity2;  //copy if jump_velocity gets changed(for example in sonic character)
    public Vector2 velocity;
    
    public int moving_direction = 1;
    public float gravity = 500f;

    public bool  forced_moving_value = false;
    public float forced_moving_velocity = 550f;
    public int   forced_moving_direction;
    public float forced_moving_cooldown = 1f;
    public DateTime forced_moving_activation_time = DateTime.MinValue;

    public Rectangle currentRect;
    public Rectangle futureRect;
    
    public  int RectangleWidth = 150;
    public  int RectangleHeight = 150;
    public const int RectangleWidth_copy = 150;   //backup when RectangleWith gets tempory overwritten
    public const int RectangleHeight_copy = 150;  // backups when RectangleHeight gets temporary ovberwritten 

    public Vector2 position;
    public Vector2 starting_position;

    public float maxHeightY = 3;

    public bool can_do_specialeffect;
    public bool is_using_specialeffect = false;
    public DateTime last_time_used = DateTime.MinValue;

    public int strength;

    public float groundY = float.NaN;     //nicht initialisert am anfang um möliche bugs abzufangen welche bei zb. 0.0f entstehen könnten
    public float groundY_copy = float.NaN;

    public PowerUp powerup1 = null;
    public PowerUp powerup2 = null;
    public bool powerUp1_in_use = false;
    public bool powerUp2_in_use = false;

    public DateTime last_time_powerUp1_got_activated = DateTime.MinValue;
    public DateTime last_time_powerUp2_got_activated = DateTime.MinValue;
    public TimeSpan powerUp_time_diff = TimeSpan.FromMilliseconds(1000);

    public DateTime activation_time_BigPlayer_powerup = DateTime.MinValue;
    public float cooldown_BigPlayer_powerup = 0;
    public bool BigPlayer_powerup_in_use = false;

    public DateTime activation_time_SmallPlayer_powerup = DateTime.MinValue;
    public float cooldown_SmallPlayer_powerup = 0;
    public bool SmallPlayer_powerup_in_use = false;

    public DateTime activation_time_MoveChange_powerup = DateTime.MinValue;
    public float cooldown_MoveChange_powerup = 0;
    public bool MoveChange_powerup_in_use = false;

    public PlayerControls controls;
    public GameLogic GameLogic_object;
    public ContentManager content;

    public bool shoots_diagonal = false;
    public bool shoots_horizontal = false;
    public bool shoots_lupfer = false;
    
    public DateTime time_shoot_lupfer_button_got_released     = DateTime.MaxValue;
    public DateTime time_shoot_diagonal_button_got_released   = DateTime.MaxValue;
    public DateTime time_shoot_horizontal_button_got_released = DateTime.MaxValue;
    public TimeSpan time_diff = TimeSpan.FromMilliseconds(350);

    /*
     * -------------------------------------------------
     * heredity
     *      each player has a special effect that gets executed through function do_special_effect()
     *      this function gets overwritten in each child-class inherting from this one
     *      
     * please comment your code guys!!!
     * 
     * starting position
     *  starting position gets defined in GameLocig.cs -> function: Set_Player
     *  
     *  
     *  
     *  Rectangle Width/height:
     *      these values get temporary overwritten in the BigPlayerPowerUp
     *      to reset them to their original values, we use copys of these values 
     *      
     *      same for groundY
     *      
     *   PowerUp
     *      when a PowerUp is used on a player (like biggerPlayerPowerUP) its values (as cooldown) are stored in the Player
     *      before the PowrUp gets deleted
     *      Moreover , with this aproach we can check on values of PowerUp from GameLogic (by calling connected player.value)
     *      
     *      which teleportaion PowerUp
     *          bug: you steal the PowerUp from your Oponent and activate it insteanc cause the taste is pressed
     *          for more than one frame
     *          solution: each powerup can only be activated each second / half second,..., something like this 
     *                    stored in variable powerUp_time_diff
     *      
     *     
     *   Player Controller // shooting
     *      There should be a small time difference between releasing a key and setting the correspondign varialbe to false
            as it is usual in many video games
            varialbe time_dif 
            functions to check if at least time_dif is gone since button got released:  
            public void update_released_Controller_Buttons()
     * --------------------------------------------------
     */


    public Player(GraphicsDevice graphicsDevice, Vector2 position1, Texture2D texture1, Texture2D special_move_texture1, int player, PlayerControls controls)
    {
        position = position1;
        playerGroup = player;
        moving_direction = get_moving_direction_from_player_group();

        texture = texture1;
        special_move_texture = special_move_texture1;

        currentRect = new Rectangle((int)position.X, (int)position.Y, RectangleWidth, RectangleHeight);
        futureRect = new Rectangle((int)position.X, (int)position.Y, RectangleWidth, RectangleHeight);
        can_do_specialeffect = true;
        strength = 1;
        move_speed2 = move_speed;

        this.controls = controls;

        if (player == 1)      { moving_direction = 1;  }
        else if (player == 2) { moving_direction = -1; }

    }


    public void Set_other_Player(Player otherPlayer1)
    {
        otherPlayer = otherPlayer1;
    }

    public void set_content(ContentManager content1)
    {
        content = content1;
    }

    public void activate_forced_moving(float dir)
    {
        forced_moving_value = true;
        forced_moving_activation_time = DateTime.Now;
        //direction must be 1 or -1
        if ( dir == 1) { forced_moving_direction = 1;  }
        else           { forced_moving_direction = -1; }
    }


    public void set_groundYs(float abc)
    {
        groundY = abc;
        groundY_copy = abc;
    }

    public void set_groundY_to_original_value()
    {
        groundY = groundY_copy;
    }

    public int get_moving_direction_from_player_group()
    {
       // at the start of the game
       // based from the playergroup we assume the next moving direction of the player
       // left player will move to the right, right player will move to the left 
       if (playerGroup == 1) { return 1; }
       else                 { return -1;}
    }


    //----------------------------------------------------------------------
    //----------------------------------------------------------------------
    //Input Handling
    public virtual void handle_input(float delta)
    {
        handle_tastatur_input(delta);
        handle_controller_input(delta);
    }


    public void handle_tastatur_input(float delta)
    {
        if (InputHandler.IsDown(controls.getKey(PlayerAction.Left)))
        {
            if (MoveChange_powerup_in_use == false) { move_helper(delta, -1); }
            else                                    { move_helper(delta, 1);  }
        }
        if (InputHandler.IsDown(controls.getKey(PlayerAction.Right)))
        {
            if (MoveChange_powerup_in_use == false) { move_helper(delta, 1); }
            else                                    { move_helper(delta, -1);}
        }
        if (InputHandler.IsDown(controls.getKey(PlayerAction.Jump)) && IsOnGround(position))
        {
            jump(delta);
        }

        if (InputHandler.IsDown(controls.getKey(PlayerAction.Lupfer)))      { shoots_lupfer = true;    }
        if (InputHandler.IsDown(controls.getKey(PlayerAction.Diagonal)))    { shoots_diagonal = true;  }
        if (InputHandler.IsDown(controls.getKey(PlayerAction.Horizontal)))  { shoots_horizontal = true;}

        if (InputHandler.IsDown(controls.getKey(PlayerAction.Special)))     do_special_effect(delta);
        if (InputHandler.IsDown(controls.getKey(PlayerAction.PowerUp_1)))   activate_powerUP(1);
        if (InputHandler.IsDown(controls.getKey(PlayerAction.PowerUp_2)))   activate_powerUP(2);


        //releasing keys
        if (InputHandler.IsReleased(controls.getKey(PlayerAction.Lupfer)))      { time_shoot_lupfer_button_got_released     = DateTime.Now; }
        if (InputHandler.IsReleased(controls.getKey(PlayerAction.Diagonal)))    { time_shoot_diagonal_button_got_released   = DateTime.Now; }
        if (InputHandler.IsReleased(controls.getKey(PlayerAction.Horizontal)))  { time_shoot_horizontal_button_got_released = DateTime.Now; }

        update_released_Controller_Buttons_and_Keys();
    }


    public void handle_controller_input(float delta)
    {
        //Controller (steuerung sind X-Box tasten, do not ask me why...Weil Microsoft

        if (InputHandler.IsL3MovedToSide("l", playerGroup))
        {
            if (MoveChange_powerup_in_use == false) { move_helper(delta, -1); }
            else                                    { move_helper(delta, 1); }
        }
        if (InputHandler.IsL3MovedToSide("r", playerGroup))
        {
            if (MoveChange_powerup_in_use == false) { move_helper(delta, 1); }
            else                                    { move_helper(delta, -1); }
        }
        if (InputHandler.IsGamePadButtonDown(Buttons.A, playerGroup))
        {
            jump(delta);
        }

        if (InputHandler.IsGamePadButtonDown(Buttons.X, playerGroup))   do_special_effect(delta);
        if (InputHandler.isLeftTriggerDown(playerGroup))                activate_powerUP(1);
        if (InputHandler.isRightTriggerDown(playerGroup))               activate_powerUP(2);


        if (InputHandler.IsGamePadButtonDown(Buttons.Y, playerGroup))
            shoots_horizontal = true;

        if (InputHandler.IsGamePadButtonDown(Buttons.B, playerGroup))
        {
            if (InputHandler.IsGamePadButtonDown(Buttons.LeftShoulder, playerGroup))
                shoots_lupfer = true;
            else
                shoots_diagonal = true;
        }


        //releasing button
        if (InputHandler.IsGamePadButtonReleased(Buttons.B, playerGroup))               { time_shoot_diagonal_button_got_released   = DateTime.Now; 
                                                                                          time_shoot_lupfer_button_got_released     = DateTime.Now; }
        if (InputHandler.IsGamePadButtonReleased(Buttons.Y, playerGroup))               { time_shoot_horizontal_button_got_released = DateTime.Now; }
        if (InputHandler.IsGamePadButtonReleased(Buttons.LeftShoulder, playerGroup))    { time_shoot_lupfer_button_got_released     = DateTime.Now; }


        update_released_Controller_Buttons_and_Keys();
    }


    //-----------------------------------------------------------------
    //-----------------------------------------------------------------
    public void update_released_Controller_Buttons_and_Keys()
    {
        update_is_shooting_diagonal();
        update_is_shooting_horizontal();
        update_is_shooting_lupfer();
    }

    public void update_is_shooting_horizontal()
    {
        if (shoots_horizontal == false) { return; }

        TimeSpan vergangeneZeit = DateTime.Now - time_shoot_horizontal_button_got_released;
        if (vergangeneZeit > time_diff)
        {
            time_shoot_horizontal_button_got_released = DateTime.MaxValue;
            shoots_horizontal = false;
        }
    }

    public void update_is_shooting_diagonal()
    {
        if (shoots_diagonal == false) { return; }

        TimeSpan vergangeneZeit = DateTime.Now - time_shoot_diagonal_button_got_released;
        if (vergangeneZeit > time_diff)
        {
            time_shoot_diagonal_button_got_released = DateTime.MaxValue;
            shoots_diagonal = false;
        }
    }

    public void update_is_shooting_lupfer()
    {
        if (shoots_lupfer == false) { return; }

        TimeSpan vergangeneZeit1 = DateTime.Now - time_shoot_lupfer_button_got_released;
        if (vergangeneZeit1 > time_diff)
        {
            time_shoot_lupfer_button_got_released = DateTime.MaxValue;
            shoots_lupfer = false;
        }       
    }


    public virtual void do_special_effect(float delta)
    {
        // this player does nothing special;
        return;
    }


//------------------------------------------------------------------------------------
//------------------------------------------------------------------------------------
// draw 

    public virtual void draw(SpriteBatch spritebatch, GameTime gameTime)
    {   

        if (moving_direction == -1)
        {
            spritebatch.Draw(texture,
                             currentRect, null, Color.White, 0f, Vector2.Zero,
                             SpriteEffects.FlipHorizontally, 0f
                             );
        }
        else
        {
            spritebatch.Draw(texture,
                            currentRect, null, Color.White, 0f, Vector2.Zero,
                            SpriteEffects.None, 0f
                            );
        }

        if (can_move == false)
        {
            draw_knockout_animation(spritebatch, gameTime);
        }
    }


    public void draw_knockout_animation(SpriteBatch spriteBatch, GameTime gameTime)
    {
        // draw the 3 stars over the character

        float size = 40;   // size der Sterne
        float p = 0.12f;   // puliserungsfaktor der sterne
        float rv = 4f;     // geschwindigkeit mit der die Sterne rotieren

        Texture2D s1 = GameLogic_object.s1;  //der stern
        Color color = new Color(255, 255, 100, 180); // halbtransparent gelb

        float time = (float)gameTime.TotalGameTime.TotalSeconds;
        float rotation = time * rv;
        float baseScale = 1f + p * (float)Math.Sin(time * 3); // Pulsieren (Skalierung schwankt)
        float scale = (size / s1.Width) * baseScale;

        Vector2 center_pos =    new Vector2(position.X+ currentRect.Width/2, position.Y- 10);
        Vector2 mittelpunkt = new Vector2(s1.Width / 2f, s1.Height / 2f);
        Vector2[] offsets = new Vector2[]
        {
            new Vector2(-50, -10),
            new Vector2(0, 0),
            new Vector2(50, -10)
        };


        foreach (var offset in offsets)
        {
            Vector2 starPos = center_pos + offset;
            spriteBatch.Draw(
                s1,                 
                starPos,           
                null,         
                color,
                rotation,             
                mittelpunkt,
                scale,
                SpriteEffects.None,
                0f
            );
        }
    }

//-------------------------------------------------------------------------------------------
//-------------------------------------------------------------------------------------------
    public void move_helper(float delta, float dir)
    {
        // gets called in player.handleInput 
        // do not let player move when he is currently moving by force 
        // to prevent double moving
        // to prevent oposite moving, which would lead to no movement at all

        if(forced_moving_value == false)
        { 
            move(delta, dir); 
        }
    }

    public virtual void move(float delta, float dir)
    {
        if ((this is  Sonic) == false)
        {

            if (forced_moving_value == true)
            {
                move_speed = forced_moving_velocity;
            }
            else
            {
                if (can_move == false) { return; }
                //dir must be -1 or 1
                if (dir != -1 && dir != 1) { throw new Exception("error in move() function. Dir is not -1 or 1"); }
                moving_direction = (int)dir;
                move_speed = move_speed2;
            }
        }
        

        float newPositionX = position.X + (delta * move_speed) * dir;
        Vector2 newPosition = new Vector2(newPositionX, position.Y);

        futureRect = new Rectangle((int)newPositionX, (int)position.Y, RectangleWidth, RectangleHeight);

        //when ball is fireball / iceball: player should not be able to touch it
        if (futureRect.Intersects(GameLogic_object.getBall().getRect())) { 
             if ((GameLogic_object.getBall().fire_powerUp_in_use == true) ||
                 ( GameLogic_object.getBall().ice_powerUp_in_use == true))  
             {
                return;
             }
        }

        if (!(futureRect.Intersects(otherPlayer.currentRect)))
        {
            if (out_of_bounds_both_scales(newPosition) == false)
            {
                position.X += (move_speed * delta) * dir;
                update_rectangles();
            }
        }


        // future rect overlaps with oponent
        // if stronger
        // push oponent away and move anýway
        else if (is_stronger_than_oponent(otherPlayer))
        {
            if (oponent_is_in_the_way(otherPlayer, dir)) otherPlayer.move(delta, dir);


            newPosition = new Vector2(newPositionX, position.Y);
            futureRect = new Rectangle((int)newPositionX, (int)position.Y, RectangleWidth, RectangleHeight);

            if (!futureRect.Intersects(otherPlayer.currentRect) && !out_of_bounds_both_scales(newPosition))
            {
                position.X += move_speed * delta * dir;
                update_rectangles();
            }
        }
    }

    public void forced_moving(float delta)
    {
        if (forced_moving_value)
        {
            move(delta, forced_moving_direction);
        }
    }


    public void jump(float delta)
    {
        if(can_move == false) { return; }

        if (!(IsOnGround(position))) return;
        velocity.Y = jump_velocity;

    }


    public void getKnockout_by_schuriken()
    {
        getKnockout(1);
    }
    public void getKnockout_by_goomba()
    {
        getKnockout(2);
    }

    public void getKnockout(int i)
    {
        //  integer to deklare the object that causes the knockout
        //  Schuriken: i=1 
        //  Goomba:    i=2

        if (can_move == false) { return; }  // do not go knockout if you are already knockout 

        can_move = false;
        hitting_time = DateTime.Now;
        knockout_time = (i == 1) ? schuriken_knockout_time : goomba_knockout_time;
    }

    public void update_knockout_phase()
    {
        if (can_move == true) { return; }  // nothing to do

        TimeSpan timeSinceHit = DateTime.Now - hitting_time;
        if (timeSinceHit.TotalSeconds >= knockout_time)
        { 
            can_move = true;
            knockout_time = 0;
        }
    }

    public void update_forced_moving_if_time_is_over()
    {
        if (forced_moving_value == false) { return; }

        TimeSpan time = DateTime.Now - forced_moving_activation_time;
        if(time.TotalSeconds >= forced_moving_cooldown)
        {
            forced_moving_value = false;
        }
    }


    // ------------------------------------------------------------------------
    // PowerUp stuff 

    public void activate_powerUP(int slot)
    {
        if (can_move == false) return;

        bool reset_powerup = true;    
        
        if (slot == 1 && powerup1 != null)
        {
            if (can_activate_powerUp1() == false) { return;}

           //switch part handling a few edge case /bugs
            switch(powerup1.get_class_name())
            {
                case "StealingPowerUp":
                    // for the reset powerup, preventing that (stolen=) powerup gets set to null
                    reset_powerup = false;
                    break;

                case "BigPlayerPowerUp":
                    // player gets bigger -> players intersect, players can not move anymore -> bug
                    int newSize = powerup1.getNewSize();
                    Rectangle testRect = new Rectangle((int)position.X, (int)position.Y-101, newSize, newSize); //size 250 is set currently set in BigPlayerPowerUp
                    if(testRect.Intersects(otherPlayer.currentRect))
                    {
                        return;
                    }
                    break;

                case "IceballPowerUp":
                    GameLogic_object.playIceSound();
                    break;

                case "Panzer2PowerUp":
                    powerup1 = new Panzer1PowerUp(GameLogic_object.getBall(), content);
                    reset_powerup = false;
                    break;
            }


            // values on the player or ball get set in the corresponding PowerUp object/ class
            powerup1.activate(this, 1);
            last_time_powerUp1_got_activated = DateTime.Now;

            if (reset_powerup == true)
            {
                powerup1 = null;
            }

        }


        else if (slot == 2 && powerup2 != null)
        {
            if (can_activate_powerUp2() == false) { return; }

            //switch part handling a few edge case /bugs
            switch (powerup2.get_class_name())
            {
                case "StealingPowerUp":
                    // for the reset powerup, preventing that (stolen=) powerup gets set to null
                    reset_powerup = false;
                    break;

                case "BigPlayerPowerUp":
                    // player gets bigger -> players intersect, players can not move anymore -> bug
                    //size 250 is set currently set in BigPlayerPowerUp
                    int newSize = powerup2.getNewSize();
                    Rectangle testRect = new Rectangle((int)position.X, (int)position.Y - 101, newSize, newSize); //size 250 is set currently set in BigPlayerPowerUp
                    if (testRect.Intersects(otherPlayer.currentRect))
                    {
                        return;
                    }
                    break;

                case "Panzer2PowerUp":
                    powerup2 = new Panzer1PowerUp(GameLogic_object.getBall(), content);
                    reset_powerup = false;
                    break;
            }

            // values on the player or ball get set in the corresponding PowerUp object/ class
            powerup2.activate(this, 2);
            last_time_powerUp2_got_activated = DateTime.Now;

            if (reset_powerup == true)
            {
                powerup2 = null;
            }
        }
    }

    public bool can_activate_powerUp1()
    {
        // check if enough time (>= powerUp_time_diff) is passed since last activation of powerup1
        
        TimeSpan dt = DateTime.Now - last_time_powerUp1_got_activated;
        return dt >= powerUp_time_diff;
    }

    public bool can_activate_powerUp2()
    {
        // check if enough time (>= powerUp_time_diff) is passed since last activation of powerup2

        TimeSpan dt = DateTime.Now - last_time_powerUp2_got_activated;
        return dt >= powerUp_time_diff;
    }



    public void set_PowerUp(PowerUp a)
    {
        if (powerup1 == null)
        {
            powerup1 = a;
            return;
        }
        if (powerup2 == null)
        {
            powerup2 = a;
            return;
        }
    }


    public void reset_powerUps_if_time_is_over()
    {
       reset_BigPlayer_PowerUp_if_time_is_over();
       reset_Small_Player_PowerUp_if_time_is_over();
       reset_MoveChange_PowerUp_if_time_has_come();
    }

    public void reset_MoveChange_PowerUp_if_time_has_come()
    {
        if (MoveChange_powerup_in_use == false) { return; }

        DateTime current_time = DateTime.Now;
        double vergangene_zeit = (current_time - activation_time_MoveChange_powerup).TotalSeconds;

        if (vergangene_zeit > cooldown_MoveChange_powerup)
        {
            reset_MoveChange_after_MoveChangePowerUp_is_over();
        }
    }

    public void reset_BigPlayer_PowerUp_if_time_is_over()
    {
        if(BigPlayer_powerup_in_use == false) { return; }

        DateTime current_time = DateTime.Now;
        double vergangene_zeit = (current_time - activation_time_BigPlayer_powerup ).TotalSeconds;

        if (vergangene_zeit > cooldown_BigPlayer_powerup)
        {
            reset_Size_after_BigPlayerPowerUp_is_over();
            BigPlayer_powerup_in_use = false;
        }  
    }

    public void reset_Small_Player_PowerUp_if_time_is_over()
        // in oposite to activate part
        // checks for own values and makes the own player big again
        // after oponent has set the values of this player
    {
        if (SmallPlayer_powerup_in_use == false) { return; }

        DateTime current_time = DateTime.Now;
        double vergangene_zeit = (current_time - activation_time_SmallPlayer_powerup).TotalSeconds;

        if (vergangene_zeit > cooldown_SmallPlayer_powerup)
        {
            // only (re-)set size of players when they not collide after (re-)setting
            // temporary solution to bug fix where
            //          player A is small, player stands on the other player B , playeer A gets big again, player collide, no player can move
            // bias -10 to make sure rectangle intersect instead of overlap (Rect.Intersect would not work than)

            Rectangle testRect1 = new Rectangle((int)position.X, (int)position.Y-40, (int)RectangleWidth_copy , (int)RectangleHeight_copy);
            if ( testRect1.Intersects(otherPlayer.currentRect)) {
                return;
            }

            reset_size_after_SmallPlayerPowerUp_is_over();
            SmallPlayer_powerup_in_use = false;
           
        }
    }

    public void reset_groundY()
    {
        groundY = groundY_copy;
    }

    public void reset_size()
    {
        RectangleHeight = RectangleHeight_copy;
        RectangleWidth = RectangleWidth_copy;
    }

    public void reset_Size_after_BigPlayerPowerUp_is_over()
    {
        //BigPlayerPowerUp was used -> player is bigger than used to be
        if(RectangleHeight > RectangleHeight_copy || RectangleWidth > RectangleWidth_copy)
        {
            reset_size();
            reset_groundY();
        }
    }

    public void reset_size_after_SmallPlayerPowerUp_is_over()
    {
        //SmallPowerUp was used -> player is smaller than used to be
        // reset it 
        if (RectangleHeight < RectangleHeight_copy || RectangleWidth < RectangleWidth_copy)
        {
            reset_size();
            reset_groundY();
        }
    }

    public void reset_MoveChange_after_MoveChangePowerUp_is_over()
    {
        MoveChange_powerup_in_use = false;
        activation_time_MoveChange_powerup = DateTime.MinValue;
    }


//-----------------------------------------------------------------------
    public virtual void update_vertical(float delta)
    {
        velocity.Y += gravity * delta;
        float newY = Math.Max(position.Y + velocity.Y * delta, maxHeightY);

        // Neue Position vorbereiten
        Vector2 newPos = new Vector2(position.X, newY);
        Rectangle testRect = new Rectangle((int)newPos.X, (int)newPos.Y, RectangleWidth, RectangleHeight);


        if (testRect.Intersects(GameLogic_object.getBall().getRect()))
        {
            // make sure player does not intersect with ball from the down side when (ball = iceball)
            if (GameLogic_object.getBall().ice_powerUp_in_use  == true)
            {
                velocity.Y = 0;
                return;
            }

            if (GameLogic_object.getBall().fire_powerUp_in_use == true &&
                GameLogic_object.getBall().velocity.Length() < 5)
            {
                //make sure player can not jump into fireball that is on the ground and does not move
                velocity.Y = 0;
                return;
            }
        }


        // Prüfe Kollision mit anderem Spieler B
        if (testRect.Intersects(otherPlayer.currentRect))
        {
            // Prüfe ob der Spieler von oben auf den anderen Spieler B fällt
            if (velocity.Y > 0 && position.Y + RectangleHeight <= otherPlayer.position.Y + 10) // 10 ist Toleranz
            {
                // Spieler landet auf dem anderen Spieler B
                position.Y = otherPlayer.position.Y - RectangleHeight;
                velocity.Y = 0;
                //spider man check here 

            }
            else if (velocity.Y < 0 && position.Y >= otherPlayer.position.Y + otherPlayer.RectangleHeight - 10)
            {
                // Spieler stößt von unten gegen den anderen Spieler 
                position.Y = otherPlayer.position.Y + otherPlayer.RectangleHeight;
                velocity.Y = 0;
            }
            else
            {
                // Seitliche Kollision - stoppe die Bewegung
                velocity.Y = 0;
            }
        }
        else
        {
            // Keine Kollision mit anderem Spieler
            position.Y = newY;
        }

        // Prüfe Kollision mit Boden
        if (position.Y >= groundY)
        {
            position.Y = groundY;
            velocity.Y = 0;
        }

        update_rectangles();
    }


    public void update_rectangles()
    {
        //update current_rectangle and future_rectangle
        currentRect = new Rectangle((int)position.X, (int)position.Y, RectangleWidth, RectangleHeight);
        futureRect = new Rectangle((int)position.X, (int)position.Y, RectangleWidth, RectangleHeight);
    }

    public void update_special_effect_in_use()
    {
        TimeSpan elapsed = DateTime.Now - last_time_used;
        
        if (elapsed.TotalSeconds > 0.2)
        {
            is_using_specialeffect = false;
        }
    }


    public bool out_of_bounds_both_scales(Vector2 newPosition1)
    {
        return (out_of_bounds_X_Scale(newPosition1) || out_of_bounds_Y_Scale(newPosition1));
    }


    public bool out_of_bounds_X_Scale(Vector2 newPosition1)
    {
        if (newPosition1.X >= 1800) return true;
        if (newPosition1.X < 0) return true;
        return false;
    }

    public bool out_of_bounds_Y_Scale(Vector2 newPosition1)
    {
        if (newPosition1.Y < 0) return true;
        return false;
    }


    public bool IsOnGround(Vector2 position)
    {
        return position.Y >= groundY;
    }

    public bool is_stronger_than_oponent(Player otherPlayer1)
    {
        return (strength > otherPlayer1.strength);
    }

    public bool oponent_is_in_the_way(Player otherPlayer1, float direction)
    {
        // check if other Player is in the given direction compared to the position of the own player
        // otherPlayer would be_in_the_way 

        if (direction != -1 && direction != 1) { throw new Exception("error in in_the_way() function. Dir is not -1 or 1"); }


        if (direction == 1) { return otherPlayer.position.X >= position.X; } // right
        return otherPlayer.position.X <= position.X; //to the left

    }

    public virtual bool can_do_special_effect()
    {
        return can_do_specialeffect;
    }

    public bool is_horizontal_overlapping_with_a_Player(Rectangle testRect)
    {
        //check if a rectangle is overlapping with the currentRect of a Player
        return (testRect.X < otherPlayer.currentRect.X + RectangleWidth &&
                testRect.X + RectangleWidth > otherPlayer.currentRect.X);

    }

    public void set_back_to_starting_position()
    {
        position = starting_position;
        update_rectangles();
    }

    public virtual void update_values()
    {
        // overwritten in other players
    }

}

