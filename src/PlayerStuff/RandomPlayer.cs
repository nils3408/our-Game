//Nils


using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;



public class RandomPlayer: Player
{
    //Konstruktor for RandomPlayer
    public RandomPlayer(GraphicsDevice graphicsDevice, Vector2 position1, Texture2D texture1, Texture2D special_move_texture, int player, PlayerControls controls)
              : base(graphicsDevice, position1, texture1, special_move_texture, player, controls)
    {}


    //class is just a space holder for clear code
}