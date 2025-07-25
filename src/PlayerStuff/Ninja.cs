using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System;

public class Ninja : Player
{

    public int cooldown = 11; //can activate its special effect all x seconds
    public DateTime last_time_used = DateTime.MinValue; //last time the special effect got used


    public Ninja(GraphicsDevice graphicsDevice, Vector2 position1, Texture2D texture1, int player, PlayerControls controls)
              : base(graphicsDevice, position1, texture1, player, controls)
    { }

    public override void do_special_effect(float delta)
    {
        if (can_do_specialeffect() == false) { return; }
        
        //error prevention
        if (GameLogic_object == null) {return; }

        GameLogic_object.add_Schuriken(new Vector2(position.X, position.Y+ 30), this, moving_direction);

    }

    
    public bool can_do_specialeffect()
    {
        TimeSpan timeSinceLastUse = DateTime.Now - last_time_used;
        return timeSinceLastUse.TotalSeconds >= cooldown;
    }
}
