//nils 



using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;



public class Panzer
{
    Texture2D texture; 
    public int texture_width  = 65;
    public int texture_height = 53;

    Vector2 velocity = new Vector2(800, 0);
    int direction;
    public Vector2 position;
    public Player owner;
    public Rectangle current_Rect;


    public Panzer(Texture2D texture1, Vector2 pos, Player owner1, int dir)
    {
        this.texture = texture1;
        this.position = pos;
        this.current_Rect = new Rectangle((int)position.X, (int)position.Y, texture_width, texture_height);
        this.owner = owner1;

        //direction must be 1  or -1  
        //otherwise we use one as default value
        if (dir == 1 || dir == -1) { direction = dir; }
        else { direction = 1; }
    }


    public void move(float delta)
    {
        if (velocity == Vector2.Zero)   { return; }

        position.X = position.X + (delta * velocity.X) * direction;
        update_rectangle();
    }


    public void update_rectangle()
    {
        current_Rect = new Rectangle((int)position.X, (int)position.Y, texture_width, texture_height);
    }


    public void draw(SpriteBatch spritebatch)
    {
        spritebatch.Draw(texture, current_Rect, Color.White);
    }
}