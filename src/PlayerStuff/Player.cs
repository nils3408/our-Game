﻿//nils, Lukas


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
using System.Security.Cryptography.X509Certificates;


public class Player
{

    public Texture2D texture;
    public Texture2D special_move_texture;

    public Player otherPlayer;
    public int playerGroup;
    
    public bool can_move = true;
    public float knockout_time = 3f;  // time a player is knocked out after getting hit from a Schuriken
    public DateTime schuriken_hitting_time = DateTime.Now; // time the player got hit by a Schuriken

    public float move_speed = 380f;
    public float move_speed2;  //copy if move_speed gets changed (for example in sonic character)
    
    public float jump_velocity = -500f;
    public float jump_velocity2;  //copy if jump_velocity gets changed(for example in sonic character)
    public Vector2 velocity;
    
    public int moving_direction = 1;
    public float gravity = 500f;

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
    public int strength;

    public float groundY = float.NaN;     //nicht initialisert am anfang um möliche bugs abzufangen welche bei zb. 0.0f entstehen könnten
    public float groundY_copy = float.NaN;

    public PowerUp powerup1 = null;
    public PowerUp powerup2 = null;
    public bool powerUp1_in_use = false;
    public bool powerUp2_in_use = false;

    public DateTime activation_time_BigPlayer_powerup = DateTime.MinValue;
    public float cooldown_BigPlayer_powerup = 0;
    public bool BigPlayer_powerup_in_use = false;

    public DateTime activation_time_SmallPlayer_powerup = DateTime.MinValue;
    public float cooldown_SmallPlayer_powerup = 0;
    public bool SmallPlayer_powerup_in_use = false;

    public PlayerControls controls;
    public GameLogic GameLogic_object;
    public ContentManager content;

    public bool shoots_diagonal = false;
    public bool shoots_horizontal = false;
    public bool shoots_lupfer = false;
    
    public DateTime time_kreis_got_released = DateTime.MaxValue;
    public DateTime time_dreieck_got_released = DateTime.MaxValue;
    public DateTime time_L1_got_released = DateTime.MaxValue;
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

    public void handle_input(float delta)
    { 
        //tastatur
        if (InputHandler.IsDown(controls.getKey(PlayerAction.Left))) move(delta, -1);
        if (InputHandler.IsDown(controls.getKey(PlayerAction.Right))) move(delta, 1);
        if (InputHandler.IsDown(controls.getKey(PlayerAction.Jump)) && IsOnGround(position)) jump(delta);

        if (InputHandler.IsDown(controls.getKey(PlayerAction.Special))) do_special_effect(delta);
        if (InputHandler.IsDown(controls.getKey(PlayerAction.PowerUp_1))) activate_powerUP(1);
        if (InputHandler.IsDown(controls.getKey(PlayerAction.PowerUp_2))) activate_powerUP(2);

        //-----------------------------------------------------------------------------------------
        //Controller (steuerung sind X-Box tasten, do not ask me why
        if (InputHandler.IsGamePadButtonDown(Buttons.A, playerGroup))   jump(delta);
        if (InputHandler.IsL3MovedUp(playerGroup))                      jump(delta);
        if (InputHandler.IsL3MovedToSide("l", playerGroup))             move(delta, -1);
        if (InputHandler.IsL3MovedToSide("r", playerGroup))             move(delta, 1);

        if (InputHandler.IsGamePadButtonDown(Buttons.X, playerGroup))   do_special_effect(delta);
        if (InputHandler.isLeftTriggerDown(playerGroup))                activate_powerUP(1);
        if (InputHandler.isRightTriggerDown(playerGroup))               activate_powerUP(2);

        //pressing button
        if (InputHandler.IsGamePadButtonDown(Buttons.B, playerGroup) && 
            !(InputHandler.IsGamePadButtonDown(Buttons.LeftShoulder, playerGroup)))     shoots_diagonal   = true;

        if (InputHandler.IsGamePadButtonDown(Buttons.B, playerGroup) &&
            InputHandler.IsGamePadButtonDown(Buttons.LeftShoulder, playerGroup))        shoots_lupfer = true;

        if (InputHandler.IsGamePadButtonDown(Buttons.Y, playerGroup))                   shoots_horizontal = true;

        //releasing button
        if (InputHandler.IsGamePadButtonReleased(Buttons.B, playerGroup))
        {
            time_kreis_got_released = DateTime.Now;
        }
        if (InputHandler.IsGamePadButtonReleased(Buttons.Y, playerGroup))
        {
            time_dreieck_got_released = DateTime.Now;
        }
        if (InputHandler.IsGamePadButtonReleased(Buttons.LeftShoulder, playerGroup))
        {
            time_L1_got_released = DateTime.Now;
        }

        update_released_Controller_Buttons();
    }

    public void update_released_Controller_Buttons()
    {
        update_is_shooting_diagonal();
        update_is_shooting_horizontal();
        update_is_shooting_lupfer();
    }

    public void update_is_shooting_horizontal()
    {
        if (shoots_horizontal == false) { return; }

        //horizontal shooting is by dreieck
        TimeSpan vergangeneZeit = DateTime.Now - time_dreieck_got_released;
        if (vergangeneZeit> time_diff)
        {
            time_dreieck_got_released = DateTime.MaxValue;
            shoots_horizontal = false;
        }
    }

    public void update_is_shooting_diagonal()
    {
        if (shoots_diagonal == false) { return; }

        //diagonal shooting is by kreis
        TimeSpan vergangeneZeit = DateTime.Now - time_kreis_got_released;
        if (vergangeneZeit > time_diff)
        {
            time_kreis_got_released = DateTime.MaxValue;
            shoots_diagonal = false;
        }
    }

    public void update_is_shooting_lupfer()
    {
        if (shoots_lupfer == false) { return; }

        // lupfer is by Kreis + L1
        TimeSpan vergangeneZeit1 = DateTime.Now - time_kreis_got_released;
        TimeSpan vergangeneZeit2 = DateTime.Now - time_L1_got_released;

        if (vergangeneZeit1 > time_diff)
        {
            time_kreis_got_released = DateTime.MaxValue;
            shoots_lupfer = false;
        }
        if (vergangeneZeit2 > time_diff)
        {
            time_L1_got_released = DateTime.MaxValue;
            shoots_lupfer = false;
        }
    }


    public virtual void do_special_effect(float delta)
    {
        // this player does nothing special;
        return;
    }


    public void draw(SpriteBatch spritebatch)
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
    }


    public virtual void move(float delta, float dir)
    {
        if (can_move == false) { return; }
        //dir must be -1 or 1
        if (dir != -1 && dir != 1) { throw new Exception("error in move() function. Dir is not -1 or 1"); }

        moving_direction = (int) dir;

        float newPositionX = position.X + (delta * move_speed) * dir;
        Vector2 newPosition = new Vector2(newPositionX, position.Y);

        futureRect = new Rectangle((int)newPositionX, (int)position.Y, RectangleWidth, RectangleHeight);

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


    public void jump(float delta)
    {
        if(can_move == false) { return; }

        float newPositionY = position.Y - jump_velocity * delta;
        Vector2 newPosition = new Vector2(position.X, newPositionY);

        if (!(IsOnGround(position))) return;
        velocity.Y = jump_velocity;

    }


    public void schuriken_knockout()
    {
        can_move = false;
        schuriken_hitting_time = DateTime.Now;
    }

    public void update_schuriken_knockout_phase()
    {
        if (can_move == true) { return; }  // nothing to do

        TimeSpan timeSinceHit = DateTime.Now - schuriken_hitting_time;
        if (timeSinceHit.TotalSeconds >= knockout_time)
          { can_move = true; }
    }

    // ------------------------------------------------------------------------
    // PowerUp stuff 

    public void activate_powerUP(int slot)
    {
        if (can_move == false) return;

        if (slot == 1 && powerup1 != null)
        {
            powerup1.activate(this);

            switch(powerup1.get_class_name())
            {
                case "BigPlayerPowerUp":
                    activation_time_BigPlayer_powerup = DateTime.Now;
                    cooldown_BigPlayer_powerup = powerup1.get_cooldown();
                    BigPlayer_powerup_in_use = true;
                    break;

                case "SmallPlayerPowerUp":
                    // note !: values of the other Player get set as this Player is effected by the PowerUp
                    otherPlayer.activation_time_SmallPlayer_powerup = DateTime.Now;
                    otherPlayer.cooldown_SmallPlayer_powerup = powerup1.get_cooldown();
                    otherPlayer.SmallPlayer_powerup_in_use = true;
                    break;
            }

            powerup1 = null;
        }

        else if (slot == 2 && powerup2 != null)
        {
            powerup2.activate(this);

            switch (powerup2.get_class_name())
            {
                case "BigPlayerPowerUp":
                    activation_time_BigPlayer_powerup = DateTime.Now;
                    cooldown_BigPlayer_powerup = powerup2.get_cooldown();
                    BigPlayer_powerup_in_use = true;
                    break;

                case "SmallPlayerPowerUp":
                    // note !: values of the other Player get set as this Player is effected by the PowerUp
                    otherPlayer.activation_time_SmallPlayer_powerup = DateTime.Now;
                    otherPlayer.cooldown_SmallPlayer_powerup = powerup2.get_cooldown();
                    otherPlayer.SmallPlayer_powerup_in_use = true;
                    break;
            }

            powerup2 = null;
        }
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
        if (RectangleHeight < RectangleHeight_copy || RectangleWidth < RectangleWidth_copy)
        {
            reset_size();
            reset_groundY();
        }
    }


//-----------------------------------------------------------------------
    public virtual void update_vertical(float delta)
    {
        velocity.Y += gravity * delta;
        float newY = Math.Max(position.Y + velocity.Y * delta, maxHeightY);

        // Neue Position vorbereiten
        Vector2 newPos = new Vector2(position.X, newY);
        Rectangle testRect = new Rectangle((int)newPos.X, (int)newPos.Y, RectangleWidth, RectangleHeight);

        



        // Prüfe Kollision mit anderem Spieler
        if (testRect.Intersects(otherPlayer.currentRect))
        {
            // Prüfe ob der Spieler von oben auf den anderen Spieler fällt
            if (velocity.Y > 0 && position.Y + RectangleHeight <= otherPlayer.position.Y + 10) // 10 ist Toleranz
            {
                // Spieler landet auf dem anderen Spieler
                position.Y = otherPlayer.position.Y - RectangleHeight;
                velocity.Y = 0;
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
}

