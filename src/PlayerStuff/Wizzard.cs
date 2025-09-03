//Nils


using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

using Microsoft.Xna.Framework.Media;      //sound
using Microsoft.Xna.Framework.Audio;      //sound


/*----------------------------------------------------
 * teleporting 
 * 
 *     //  3 pahses of teleportation animation
       //       1. animation at old position
       //       2. animation at new position after pahse 1 is over
       //       3. draw Player again at new position
 *    
 * --------------------------------------------------
*/


public class Wizzard: Player
{

    public int cooldown = 6; //can activate its special effect all x seconds
    
    public bool special_effect_in_use = false;

    float distance = 90f;  //distance the player is away from the ball after teleportation

    public int teleportFrameCounter = 2;
    public int teleportFrameCounter_copy = 2;
    public int teleportFrameCounter2 = 2;
    public int teleportFrameCounter2_copy = 2;
    public bool showTeleportEffect = false;
    public bool showTeleportEffect2 = false;
    public Vector2 t1_pos = new Vector2(0, 0);
    public Vector2 t2_pos = new Vector2(0, 0);

    Texture2D t1;


    //Konstruktor for Wizzard
    public Wizzard(GraphicsDevice graphicsDevice, Vector2 position1, Texture2D texture1, Texture2D special_move_texture, int player, PlayerControls controls)
              : base(graphicsDevice, position1, texture1, special_move_texture, player, controls)
    {}


   
    public override void do_special_effect(float delta)
    {
        if (can_do_special_effect() == false)   { return; }
        if (can_move == false)                  { return; }      

        // Wizzards: special effect:  teleporation
        //he can teleport himself next to the ball (left or right based on its playergroup)

        if (GameLogic_object == null)           { return; }
        if (GameLogic_object.getBall() == null) { return; }

        Ball ball = GameLogic_object.getBall();
        float pos_x;
        float pos_y;

        if (playerGroup == 1)
        {
            pos_x = ball.position.X - distance - currentRect.Width;
            pos_y = ball.position.Y;
        }
        else 
        {
            pos_x = ball.position.X + ball.BallSize +distance;
            pos_y = ball.position.Y;
        }

        Vector2 newPosition= new Vector2(pos_x, pos_y);
        Rectangle futureRect = new Rectangle((int)pos_x, (int)pos_y, RectangleWidth, RectangleHeight);

        // out of bounds or intersecting with other player -> do nothing
        if (out_of_bounds_both_scales(newPosition))         { return; }
        if (futureRect.Intersects(otherPlayer.currentRect)) { return; }


        GameLogic_object.playTeleportationSound();
        do_teleportation_animation(pos_x, pos_y);
        position = newPosition;
        update_rectangles();
        last_time_used = DateTime.Now;
        is_using_specialeffect = true;
    }
    
    
    public override bool can_do_special_effect()
    {
        DateTime current_time = DateTime.Now;
        double vergangene_zeit= (current_time- last_time_used).TotalSeconds;
        return (vergangene_zeit > cooldown);
    }


    public void do_teleportation_animation(float x2, float y2)
        //x2,y2 are the cooridnates of the new position
        //set the values for the teleportating animation
    {
        t1_pos =  new Vector2(position.X, position.Y);
        t2_pos =  new Vector2(x2, y2);
        
        showTeleportEffect = true;
        is_teleporting = true;

        teleportFrameCounter  = teleportFrameCounter_copy;
        teleportFrameCounter2 = teleportFrameCounter2_copy;

    }

    public override void draw(SpriteBatch spritebatch, GameTime gameTime)
    {
        draw_teleporting_animation(spritebatch);
        draw_player(spritebatch, gameTime);
    }

    public void draw_teleporting_animation(SpriteBatch spriteBatch)
    {
        if (is_teleporting == false) { return; }

        t1 = GameLogic_object.t1; 

        if (showTeleportEffect)
        {
            spriteBatch.Draw(t1, new Rectangle((int)t1_pos.X, (int)t1_pos.Y, 150, 150), Color.White);
        }
        if (showTeleportEffect2)
        {
            spriteBatch.Draw(t1, new Rectangle((int)t2_pos.X, (int)t2_pos.Y, 150, 150), Color.White);
        }
    }

    public void draw_player(SpriteBatch spriteBatch, GameTime gameTime)
    {
        if (is_teleporting) { return; }


        if (moving_direction == -1)
        {
            spriteBatch.Draw(texture,
                             currentRect, null, Color.White, 0f, Vector2.Zero,
                             SpriteEffects.FlipHorizontally, 0f
                             );
        }
        else
        {
            spriteBatch.Draw(texture,
                            currentRect, null, Color.White, 0f, Vector2.Zero,
                            SpriteEffects.None, 0f
                            );

        }

        if (can_move == false)
        {
            draw_knockout_animation(spriteBatch, gameTime);
        }
    }

    public override void update_values()
    {
        //  update teloprtating values

        if (showTeleportEffect)
        {
            teleportFrameCounter--;

            if (teleportFrameCounter <= 0)
            {
                showTeleportEffect = false;
                showTeleportEffect2 = true;
            }
        }

        if (showTeleportEffect2)
        {
            teleportFrameCounter2--;

            if (teleportFrameCounter2 <= 0)
            {
                showTeleportEffect2 = false;
            }
        }

        if (showTeleportEffect2 == false && showTeleportEffect == false)
        {
            is_teleporting = false;
        }
    }



}