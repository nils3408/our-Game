using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

public class Knight: Player
{
    public int cooldown = 13; //can activate its special effect all x seconds
    public int speciaL_effect_timer = 3;  //can do its special effect for x seconds

    public DateTime last_time_used = DateTime.MinValue; //last time the special effect got used
    public bool special_effect_in_use = false;



    //Konstruktor for Knight
    public Knight(GraphicsDevice graphicsDevice, Vector2 position1, Texture2D texture1, int player)
              : base(graphicsDevice, position1, texture1, player)
    {}


    public override void do_special_effect(float delta)
    {
        if (can_do_special_effect())
        {
            // special effect: stronger than its oponent-> can push him away
            strength = 2;   //current normal value = 1
            
            special_effect_in_use = true;
            last_time_used = DateTime.Now;

        }
    }



    public override void move(float delta, float dir)
    {

        reset_strength_if_cooldown_is_over();

        //dir must be -1 or 1
        if (dir != -1 && dir != 1) { throw new Exception("error in move() function. Dir is not -1 or 1"); }

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
            return;
        }


        // future rect overlaps with oponent
        //new code of the overwritten function
        if (is_stronger_than_oponent(otherPlayer))
        {
            if (oponent_is_in_the_way(otherPlayer, dir)) otherPlayer.move(delta, dir);

            // Recalculate future position
            newPosition = new Vector2(newPositionX, position.Y);
            futureRect = new Rectangle((int)newPositionX, (int)position.Y, RectangleWidth, RectangleHeight);

            if (!futureRect.Intersects(otherPlayer.currentRect) && !out_of_bounds_both_scales(newPosition))
            {
                position.X += move_speed * delta * dir;
                update_rectangles();
            }
        }
    }


    public void reset_strength_if_cooldown_is_over()
    {
        if (execution_time_is_over())
        {
            special_effect_in_use = false;
            strength = 1;
        }

    }

    public override bool can_do_special_effect()
    {
        DateTime current_time = DateTime.Now;
        double vergangene_zeit = (current_time - last_time_used).TotalSeconds;
        return (vergangene_zeit > cooldown);
    }

    public bool execution_time_is_over()
    {
        // check whether the duration the special effect is used is over
        DateTime current_time = DateTime.Now;
        double vergangene_zeit = (current_time - last_time_used).TotalSeconds;
        return (vergangene_zeit > speciaL_effect_timer);
    }
}