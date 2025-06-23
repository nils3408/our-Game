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


    public override void do_special_effect()
    {
        // spidermas special effect is: multiple jump;
        velocity.Y = jump_velocity;
    }

}