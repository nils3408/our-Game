using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

public class Sonic: Player
{
    private int counter;

    //Konstruktor for Sonic
    public Sonic(GraphicsDevice graphicsDevice, Vector2 position1, Texture2D texture1, int player)
              : base(graphicsDevice, position1, texture1, player)
    {}


    public override void do_special_effect(float delta)
    {
        if (can_do_specialeffect == false) { return; }

        // sonics: special effect:  super fast 
        move_speed = 600f;
    }

    public override void update_vertical(float delta, float groundY)
    {
        //a bit different to the "normal player" ones 
        // when he is on ground, he can do its special_effect again

        velocity.Y += gravity * delta;
        position.Y = Math.Max(position.Y + velocity.Y * delta, maxHeightY);

        if (position.Y >= groundY)
        {
            position.Y = groundY;
            velocity.Y = 0;
            can_do_specialeffect= true; 
        }

        update_rectangles();
    }


}