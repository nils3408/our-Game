//Nils 

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
    public Vector2 position = new Vector2(100, 100);

    private Texture2D current_texture;
    private Texture2D[] textures;

    public Rectangle current_Rect;
    public int rect_size = 60;

    private ContentManager content;

    //values for the animation
    private float framerate = 0.08f;
    private int  current_frame = 0;
    private float animation_time_counter = 0f;

    // position boundaries for the item in the map -> values got hardcoded 
    private float min_Y = 30;
    private float max_Y = 550;
    private float min_X = 190;
    private float max_X = 1800;

    //random value for ranom position asociating 
    Random rand = new Random();

    public Item[] all_items;  //beeinhaltet alle Items -> auch sich selber 

    public PowerUp linked_powerup;
    public PowerUpFactory pf;

    public Ball ball; //reference on ball is important so PowerUp can be used on it


    public Item(GraphicsDevice graphicsDevice, ContentManager content, Ball ball1)
    {
        this.content = content;

        textures = LoadTextures();
        current_texture = textures[0];

        current_Rect = new Rectangle((int)position.X, (int)position.Y, rect_size, rect_size);
        
        ball = ball1;
        pf = new PowerUpFactory(ball);
        set_new_powerUP();

    }


    public void draw(SpriteBatch spritebatch, GameTime gameTime)
    {
        // get the right texturefor the animation
        // get newPosition if choosen
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
        spritebatch.Draw(current_texture, current_Rect, Color.White);
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


    public void set_random_position()
    {
        position.X = rand.Next((int)min_X, (int)max_X);
        position.Y = rand.Next((int)min_Y, (int)max_Y);
        current_Rect = new Rectangle((int)position.X, (int)position.Y, rect_size, rect_size);

        if (Rect_intersect_with_one_of_the_other_items(current_Rect))
        {
            // set a new random position -> can end in endless loop in worst case but statistically very unlikely
            set_random_position();
        }
    }

    public bool Rect_intersect_with_one_of_the_other_items(Rectangle Rect)
    {
        //carefull all_items_ include this object as well !!
        foreach(Item other in all_items)
        {
            if (other != this && other.current_Rect.Intersects(Rect))
            {
                return true;
            }
        }

        return false;
    }



    public void set_all_items(Item[] items)
    {
        all_items = items;
    }

    public void set_new_powerUP()
    {
        // todo
        linked_powerup = pf.random_powerUP(content);
    }

    
}
