//Nils


using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

public class Sonic: Player
{

    public int cooldown = 7; //can activate its special effect all x seconds
    public int speciaL_effect_timer = 2;  //can do its special effect for x seconds
    
    public DateTime last_time_used = DateTime.MinValue; //last time the special effect got used
    public bool special_effect_in_use = false;

    public float fast_move_speed = 1000;



    //Konstruktor for Sonic
    public Sonic(GraphicsDevice graphicsDevice, Vector2 position1, Texture2D texture1, Texture2D special_move_texture, int player, PlayerControls controls)
              : base(graphicsDevice, position1, texture1, special_move_texture, player, controls)
    {}


   
    public override void move(float delta, float dir)
    {
        if (can_move == false) { return; }

        if (special_effect_in_use)
        {
            if (execution_time_is_over())
            {
                move_speed = move_speed2;  //back to old value , copy of move_speed, inialiezed in parent
                special_effect_in_use = false;
            }
        }

        base.move(delta, dir);
    }
    
    
    public override void do_special_effect(float delta)
    {
        if (can_do_special_effect() == false) { return; }

        // sonics: special effect:  super fast 
        move_speed = fast_move_speed;
        
        last_time_used = DateTime.Now;
        special_effect_in_use=true; 

    }
    
    
    public override bool can_do_special_effect()
    {
        DateTime current_time = DateTime.Now;
        double vergangene_zeit= (current_time- last_time_used).TotalSeconds;
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