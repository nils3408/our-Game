using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class Spiderman: Player
{

    //Konstruktor for Spiderman
    public Spiderman(GraphicsDevice graphicsDevice, Vector2 position1, Texture2D texture1, int player)
              : base(graphicsDevice, position1, texture1, player)
    {}


    public override void do_special_effect(float delta)
    {
        // spidermas special effect is: multiple jump;

        float newPositionY = position.Y - jump_velocity * delta;
        Vector2 newPosition = new Vector2(position.X, newPositionY);

        if (out_of_bounds_Y_Scale(newPosition))
        {
            newPosition.Y = maxPosY;
            return;
        }

        velocity.Y = jump_velocity;
    }

}