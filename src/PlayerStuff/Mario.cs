using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System;

/*
 * -------------------------------------------------------
 * Character Mario also has a cooldown for its special effect
 *      This cooldown is not neccessary for the specialeffect itself
 *      -- but for the sound that gets played
 *         otherwise sound gets played multiple times du to the fact that the button is pressed >=2 frames
 * 
 * -------------------------------------------------------
 */

public class Mario : Player
{
    float cooldown = 1.1f;



    public Mario(GraphicsDevice graphicsDevice, Vector2 position1, Texture2D texture1, Texture2D special_move_texture, int player, PlayerControls controls)
              : base(graphicsDevice, position1, texture1, special_move_texture, player, controls)
    { }

    public override void do_special_effect(float delta)
    {
        if (can_do_special_effect() == false)     { return; }

        velocity.Y = 500f; // erh�hte Gravitation f�r schnelleren Fall
        is_using_specialeffect = true;
        last_time_used = DateTime.Now;
        GameLogic_object.playMarioSound();
    }

    public override bool can_do_special_effect()
    {
        TimeSpan timeSinceLastUse = DateTime.Now - last_time_used;
        return timeSinceLastUse.TotalSeconds >= cooldown;
    }
}
