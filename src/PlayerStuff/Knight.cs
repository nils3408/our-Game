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
        System.Diagnostics.Debug.WriteLine(strength);


    }

    public override void move_right(float delta)
    {
        float newPositionX = position.X + delta * move_speed;
        Vector2 newPosition = new Vector2(newPositionX, position.Y);
        futureRect = new Rectangle((int)newPositionX, (int)position.Y, RectangleWidth, RectangleHeight);

        
        if (!futureRect.Intersects(otherPlayer.currentRect))
        {
            if (!out_of_bounds_both_scales(newPosition))
            {
                position.X += move_speed * delta;
                update_rectangles();
            }
            return;
        }

        // future rect overlaps with oponent
        if (is_stronger_than_oponent(otherPlayer))
        {
            otherPlayer.move_right(delta);

            // Recalculate future position
            newPosition = new Vector2(newPositionX, position.Y);
            futureRect = new Rectangle((int)newPositionX, (int)position.Y, RectangleWidth, RectangleHeight);

            if (!futureRect.Intersects(otherPlayer.currentRect) && !out_of_bounds_both_scales(newPosition))
            {
                position.X += move_speed * delta;
                update_rectangles();
            }
        }



    public override void move_left(float delta)
    {
        float newPositionX = position.X - delta * move_speed;
        Vector2 newPosition = new Vector2(newPositionX, position.Y);
        futureRect = new Rectangle((int)newPositionX, (int)position.Y, RectangleWidth, RectangleHeight);

        if (!futureRect.Intersects(otherPlayer.currentRect))
        {
            if (!out_of_bounds_both_scales(newPosition))
            {
                position.X -= move_speed * delta;
                update_rectangles();
            }
            return;
        }

        if (is_stronger_than_oponent(otherPlayer))
        {
            otherPlayer.move_left(delta);

            // WICHTIG: NEU berechnen nach Gegner bewegen!
            newPositionX = position.X - delta * move_speed;
            newPosition = new Vector2(newPositionX, position.Y);
            futureRect = new Rectangle((int)newPositionX, (int)position.Y, RectangleWidth, RectangleHeight);

            if (!futureRect.Intersects(otherPlayer.currentRect) && !out_of_bounds_both_scales(newPosition))
            {
                position.X -= move_speed * delta;
                update_rectangles();
            }
        }
    }



}