using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System;

public class Schuriken
{
    private Texture2D texture;
    public int texture_width = 75;
    public int texture_height = 75;


    public Vector2 velocity = new Vector2(550, 0);
    public Vector2 position;
    public Rectangle current_Rect;
    private int direction;
    public Player owner;

    //values for the animation
    private float framerate = 0.08f;
    private float animation_time_counter = 0f;
    private float rotation = 0f;
    private float rotationrate = 2.35f;


    public Schuriken( Texture2D texture1, Vector2 position1, Player owner1, int dir )
    {
        texture = texture1;
        position = position1;
        current_Rect = new Rectangle((int)position.X, (int)position.Y, texture_width, texture_height);
        owner = owner1;

        //direction must be 1  or -1  
        //otherwise we use one as default value
        if (dir == 1 || dir == -1) { direction = dir; }
        else                       { direction = 1; }
    }
   

    public void move(float delta)
    {
        if (velocity == Vector2.Zero) return;

        position.X = position.X + (delta * velocity.X) * direction; 
        update_rectangle();
    }


    public void update_rectangle()
    {
        current_Rect = new Rectangle((int)position.X, (int)position.Y, texture_width, texture_height);
    }


    public void draw(SpriteBatch spritebatch, GameTime gameTime)
    {
        // get the right texture for the animation
        // get newPosition if choosen
        // and draw the texture 

        update_texture(gameTime);
        draw_current_texture(spritebatch);
    }

    public void update_texture(GameTime gameTime)
    {
        //update texture for animation if "the time is come"

        animation_time_counter += (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (animation_time_counter >= framerate)
        {
            rotation = (rotation + rotationrate*direction) % 360f;
            animation_time_counter = 0;
        }

    }


    public void draw_current_texture(SpriteBatch spritebatch)
    {
        spritebatch.Draw
       (
           texture,
           position: new Vector2(current_Rect.X + current_Rect.Width / 2, current_Rect.Y + current_Rect.Height / 2),
           sourceRectangle: null,
           color: Color.White,
           rotation: rotation,
           origin: new Vector2(texture.Width / 2, texture.Height / 2),
           scale: new Vector2((float)current_Rect.Width / texture.Width, (float)current_Rect.Height / texture.Height),
           effects: SpriteEffects.None,
           layerDepth: 0f
       );
    }
}
