//Nils


using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

public class Wizzard: Player
{

    public int cooldown = 10; //can activate its special effect all x seconds
    
    public DateTime last_time_used = DateTime.MinValue; //last time the special effect got used
    public bool special_effect_in_use = false;



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
        float distance = 70f;
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
        if (out_of_bounds_both_scales(newPosition)) { return; }


        do_teleportation_animation();
        position = newPosition;
        update_rectangles();
        last_time_used = DateTime.Now;
    }
    
    
    public override bool can_do_special_effect()
    {
        DateTime current_time = DateTime.Now;
        double vergangene_zeit= (current_time- last_time_used).TotalSeconds;
        return (vergangene_zeit > cooldown);
    }


    public void do_teleportation_animation()
    {
        GameLogic_object.t1_position = new Vector2(position.X, position.Y);
        GameLogic_object.showTeleportEffect = true;
        GameLogic_object.teleportFrameCounter = GameLogic_object.teleportFrameCounterCopy; 

    }

}