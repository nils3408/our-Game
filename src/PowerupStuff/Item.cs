using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

/* 
 * --------------------------------------------
 * Item that can be seen in the map
 * linked to the PowerUp that can be used when the item is selected
 * 
 * animation:
 *   animation_time_counter counts up
 *   if it is >= framerate:
 *      it gets resetted to zero 
 *      and the next texture is loaded
 * 
 * --------------------------------------------
 */



public class Item
{
    public Vector2 position = new Vector2(400, 300);

    private Texture2D current_texture;
    private Texture2D[] textures;

    public Rectangle Rect;

    private ContentManager content;

    //values for the animation
    private float framerate = 0.08f;
    private int  current_frame = 0;
    private float animation_time_counter = 0f;


    public Item(GraphicsDevice graphicsDevice, ContentManager content)
    {
        this.content = content;

        textures = LoadTextures();
        current_texture = textures[0];

        Rect = new Rectangle((int)position.X, (int)position.Y, 60, 60);
    }


    public void draw(SpriteBatch spritebatch, GameTime gameTime)
    {
        // get the right texture for the animation
        // and draw the texture 

        update_texture(gameTime);
        draw_current_texture(spritebatch);
    }

    public void update_texture(GameTime gameTime)
    {
        //update texture for animation if "the time is come"
        animation_time_counter += (float) gameTime.ElapsedGameTime.TotalSeconds;

        if (animation_time_counter >= framerate)
        {
            current_frame = (current_frame + 1) % textures.Length;
            current_texture = textures[current_frame];
            animation_time_counter = 0;
        }
    }

    
    public void draw_current_texture(SpriteBatch spritebatch)
    {
        spritebatch.Draw(current_texture, Rect, Color.White);
    }


    private Texture2D[] LoadTextures()
    {
        Texture2D[] textures = new Texture2D[25];

        for (int i = 0; i <= 24; i++)
        {
            string path = $"Coin/coin-{i}";
            textures[i] = content.Load<Texture2D>(path);
        }

        return textures;
    }
}
