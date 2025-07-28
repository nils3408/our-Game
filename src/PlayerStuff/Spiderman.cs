using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

public class Spiderman: Player
{

    //Konstruktor for Spiderman
    public Spiderman(GraphicsDevice graphicsDevice, Vector2 position1, Texture2D texture1, int player, PlayerControls controls)
              : base(graphicsDevice, position1, texture1, player, controls)
    {}


    public override void do_special_effect(float delta)
    {
        // spidermas special effect is: multiple jump;

        if (can_do_specialeffect == false) { return; }
        if (can_move == false)             { return; }

        float newPositionY = position.Y - jump_velocity * delta;
        Vector2 newPosition = new Vector2(position.X, newPositionY);

        velocity.Y = jump_velocity;
        can_do_specialeffect = false;
    }

    public override void update_vertical(float delta)
    {
        //a bit different to the "normal player" ones 
        // when he is on ground, he can do its special_effect again
        velocity.Y += gravity * delta;
        float newY = Math.Max(position.Y + velocity.Y * delta, maxHeightY);

        // Neue Position vorbereiten
        Vector2 newPos = new Vector2(position.X, newY);
        Rectangle testRect = new Rectangle((int)newPos.X, (int)newPos.Y, RectangleWidth, RectangleHeight);

        // Pr�fe Kollision mit anderem Spieler
        if (testRect.Intersects(otherPlayer.currentRect))
        {
            // Pr�fe ob der Spieler von oben auf den anderen Spieler f�llt
            if (velocity.Y > 0 && position.Y + RectangleHeight <= otherPlayer.position.Y + 10) // 10 ist Toleranz
            {
                // Spieler landet auf dem anderen Spieler
                position.Y = otherPlayer.position.Y - RectangleHeight;
                velocity.Y = 0;
                can_do_specialeffect = true; // Spiderman kann wieder springen wenn er auf anderem Spieler steht
            }
            else if (velocity.Y < 0 && position.Y >= otherPlayer.position.Y + otherPlayer.RectangleHeight - 10)
            {
                // Spieler st��t von unten gegen den anderen Spieler
                position.Y = otherPlayer.position.Y + otherPlayer.RectangleHeight;
                velocity.Y = 0;
            }
            else
            {
                // Seitliche Kollision - stoppe die Bewegung
                velocity.Y = 0;
            }
        }
        else
        {
            // Keine Kollision mit anderem Spieler
            position.Y = newY;
        }

        // Pr�fe Kollision mit Boden
        if (position.Y >= groundY)
        {
            position.Y = groundY;
            velocity.Y = 0;
            can_do_specialeffect = true; // Spiderman kann wieder springen wenn er auf dem Boden ist
        }

        update_rectangles();
    }


}