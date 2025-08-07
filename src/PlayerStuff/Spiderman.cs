using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

public class Spiderman: Player
{

    //Konstruktor for Spiderman
    public Spiderman(GraphicsDevice graphicsDevice, Vector2 position1, Texture2D texture1, Texture2D special_move_texture, int player, PlayerControls controls)
              : base(graphicsDevice, position1, texture1, special_move_texture, player, controls)
    {}


    public override void do_special_effect(float delta)
    {
        // spidermas special effect is: multiple jump;

        if (can_do_specialeffect == false) { return; }
        if (can_move == false)             { return; }

        velocity.Y = jump_velocity;
        can_do_specialeffect = false;
    }

    public override void update_vertical(float delta)
    {
        base.update_vertical(delta);

        //can double jump (again) when he is on the ground
        if (position.Y >= groundY)
        {
            can_do_specialeffect = true;
        }


        //can jump again when he stands on an other player
        Rectangle testRect = new Rectangle((int)position.X, (int)position.Y, currentRect.Width, currentRect.Height + 10);
        if (testRect.Intersects(otherPlayer.currentRect))
        {
            can_do_specialeffect = true;
        }
    }


}