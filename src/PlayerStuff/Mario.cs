using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System;

public class Mario : Player
{
   

    public Mario(GraphicsDevice graphicsDevice, Vector2 position1, Texture2D texture1, int player)
              : base(graphicsDevice, position1, texture1, player)
    { }

    public override void do_special_effect(float delta)
    {

        velocity.Y = 500f; // erhöhte Gravitation für schnelleren Fall
     
    }
}
