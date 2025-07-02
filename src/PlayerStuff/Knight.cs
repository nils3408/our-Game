using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

public class Knight: Player
{
    private int counter;

    //Konstruktor for Spiderman
    public Knight(GraphicsDevice graphicsDevice, Vector2 position1, Texture2D texture1, int player)
              : base(graphicsDevice, position1, texture1, player)
    {}


    public override void do_special_effect(float delta)
    {
        strength = 2;   //current normal value = 1
    }



    public override void move(float delta, float dir)
    {
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
        }


        // future rect overlaps with oponent
        //new code of the overwritten function
        if (is_stronger_than_oponent(otherPlayer))
        {
            otherPlayer.move(delta, dir);

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
}