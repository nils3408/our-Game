//nils, Lukas


/*-------------------------------------------------------------------------------------------------------------
 * - Ball movement 
 *      the ball movement is based on its velocity. velocity is a 2D Vector
 *      with the gravity parameter we can simulate a realistic parabel for ballmovement
 *      
 *      when the ball collides a wall its velocity gets reset due to Reibung
 *      when player and Ball collides velocity gets resetted
 *      When player is  actively shooting: Ball becomes faster 
 *      
 *  - Ball Position
 *      whem a goal is made the Ball gets reseted to its starting position
 *      
 *  - ball_powerup_textures
 *      textures as fireball/ iceball can not be loaded in the corresponsing PowerUp if we do not
 *      give them a reference on the Contentmanager -- something we want to avoid cause we want to load 
 *      all textures in gamelogic 
 *      solution: 
            create Dictionary in gamelogic.s with the powerUp_textures and give it to ball during its initialization
            -> Powerup can access it then as it has a reference on the ball 
 * 
 * ------------------------------------------------------------------------------------------------------------
 */

using System;
using System.ComponentModel.Design;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;


//using SharpDX.XAudio2;
//using System;
//using System.Security.Cryptography.X509Certificates;
//using static System.Windows.Forms.Design.AxImporter;



public class Ball
{


    public  Texture2D current_texture;
    private Texture2D texture_copy;
    public Dictionary<string, Texture2D> textures;

    public Vector2 position;

    public Vector2 velocity = new Vector2(0, 0);
    public Vector2 starting_velocity = new Vector2(500f, -230f);
    public Vector2 shooting_horizontal_velocity = new Vector2(900, -20);
    public Vector2 shooting_diagonally_velocity = new Vector2(900, -250);
    public Vector2 shooting_lupfer_velocity = new Vector2(800, -750);


    private const int BallSize = 100;
    private const float BallFriction = (float)10;

    private const float g = 250f;               
    private Rectangle Rect;

    public bool fire_powerUp_in_use = false;
    public bool ice_powerUp_in_use = false;
    public DateTime activation_time_powerUp = DateTime.MinValue;
    public float powerUp_cooldown = 0;

    //values for the animation
    private float framerate = 0.08f;
    private float animation_time_counter = 0f;
    private float rotation = 0f;
    private float rotationrate = 2.35f;




    public Ball(GraphicsDevice graphicsDevice, Vector2 position2, Dictionary<string, Texture2D> textures1)
    {
        this.textures = textures1;
        current_texture = textures["football"];
        texture_copy =    textures["football"];
        
        position = position2;
        Rect = new Rectangle((int)position.X, (int)position.Y, BallSize, BallSize);
        //new Vector2(_graphics.PreferredBackBufferWidth / 2f, groundY)
    }

    public Rectangle getRect()
    {
        return Rect;
    }

    public void set_texture_back_to_original() {
        current_texture = texture_copy; 
    }


// -----------------------------------------------------------------------------
// velocity

    public void set_velocity(Vector2 velocity1)
    {
        velocity = velocity1;
    }

    public void reset_velocity()
    {
        if (velocity == Vector2.Zero)
        {
            velocity = starting_velocity * transform_direction(velocity);
            return;
        }

        Vector2 direction = transform_direction(velocity);
        velocity = direction * starting_velocity;
    }

    public void reduce_velocity_due_to_friction()
    {
        velocity *= 0.8f;
    }

 // ---------------------------------------------------------------------------------------
    
    public void reset_values()
    {
        set_texture_back_to_original();
        fire_powerUp_in_use = false;
        ice_powerUp_in_use = false;
        powerUp_cooldown = 0;
        activation_time_powerUp = DateTime.MinValue;

    }


    public void reset_powerUps_if_time_is_over()
    {

        DateTime current_time = DateTime.Now;
        double vergangene_zeit = (current_time - activation_time_powerUp).TotalSeconds;

        if (vergangene_zeit > powerUp_cooldown)
        {
            reset_values();
        }
    }

// -----------------------------------------------------------------------------------
// shooting functions

    public void get_shooted_horizontal()
    {
        if (fire_powerUp_in_use) { return; }
        if (ice_powerUp_in_use) { return; }
        
        if (velocity == Vector2.Zero)
        {
            velocity = shooting_horizontal_velocity;
            return;
        }

        velocity.X = shooting_horizontal_velocity.X * transform_direction(velocity).X;
        velocity.Y = shooting_horizontal_velocity.Y;
    }

    
    public void get_shooted_diagonal()
    {
        if (fire_powerUp_in_use) { return; }
        if (ice_powerUp_in_use) { return; }


        if (velocity == Vector2.Zero)
        {
            velocity = shooting_diagonally_velocity;
            return;
        }

        velocity.X = shooting_diagonally_velocity.X * transform_direction(velocity).X;
        velocity.Y = shooting_diagonally_velocity.Y;
    }

    public void get_shooted_lupfer()
    {
        if (fire_powerUp_in_use) { return; }
        if (ice_powerUp_in_use)  { return; }

        if (velocity == Vector2.Zero)
        {
            velocity = shooting_diagonally_velocity;
            return;
        }

        velocity.X = shooting_lupfer_velocity.X * transform_direction(velocity).X;
        velocity.Y = shooting_lupfer_velocity.Y;
    }


// -----------------------------------------------------------------------------------

    public void change_direction(Vector2 dir)
    {
        // dir is defined as ball.position - player.position in GameLogic
        change_direction_x_scale(dir);
        change_direction_y_scale();
    }

    public void change_direction_x_scale(Vector2 dir)
    {
        System.Diagnostics.Debug.WriteLine(dir);
        velocity.X = Math.Abs(velocity.X);
        velocity.X = velocity.X * transform_direction(dir).X;
    }

    public void change_direction_y_scale()
    {
        velocity.Y *= -1;
    }

    public void change_direction_x_scale()
    {
        velocity.X *= -1;
    }

    public void Reset_Position(Vector2 newPosition)
    {
        position = newPosition;
        velocity = Vector2.Zero;
        Rect.X = (int)position.X;
        Rect.Y = (int)position.Y;
    }


    public Vector2 transform_direction(Vector2 velocity1)
    {
        //only return -1 or 1
        float xDir = velocity1.X >= 0 ? 1 : -1;
        float yDir = velocity1.Y >= 0 ? -1 : 1;

        return new Vector2(xDir, yDir);
    }


// -------------------------------------------------------------------------------------
// out of bounds 

    public bool out_of_bounds_on_right_side(Vector2 newPosition)
    {
        return ((newPosition.X + BallSize) >= 1960);
    }

    public bool out_of_bounds_on_left_side(Vector2 newPosition)
    {
        return (newPosition.X <= 0);
    }

    public bool out_of_bounds_on_upper_side(Vector2 newPosition)
    {
        return (newPosition.Y <= 0);
    }

    public bool out_of_bounds_on_down_side(Vector2 newPosition, float groundY)
    {
        return ((newPosition.Y) >= groundY);
    }


    public bool out_of_bounds_in_generell(Vector2 newPosition, float groundY)
    {
        return (out_of_bounds_on_down_side(newPosition, groundY) || out_of_bounds_on_upper_side(newPosition) ||
                 out_of_bounds_on_right_side(newPosition) || out_of_bounds_on_left_side(newPosition)
               );
    }


 // -------------------------------------------------------------------------------------------------
 // -------------------------------------------------------------------------------------------------
 //--------------------------------------------------------------------------------------------------
 // draw functions

    public void draw(SpriteBatch spritebatch, GameTime gameTime)
    {
        update_texture(spritebatch, gameTime);
        draw_current_texture(spritebatch);  
    }

    public void update_texture(SpriteBatch spritebatch , GameTime gameTime)
    {
        //update texture for animation if "the time is come"
        
        if (velocity.X == 0)         { return; }
        if (ice_powerUp_in_use)      { return; }

        animation_time_counter += (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (animation_time_counter >= framerate)
        {
            rotation= (rotation + rotationrate) % 360f;
            animation_time_counter = 0;
        }

    }

    public void draw_current_texture(SpriteBatch spritebatch)
    {
        //spritebatch.Draw(current_texture, Rect, Color.White);
        spritebatch.Draw
        (
            current_texture,
            position: new Vector2(Rect.X + Rect.Width / 2, Rect.Y + Rect.Height / 2),
            sourceRectangle: null,
            color: Color.White,
            rotation: rotation,
            origin: new Vector2(current_texture.Width / 2, current_texture.Height / 2),
            scale: new Vector2((float)Rect.Width / current_texture.Width, (float)Rect.Height / current_texture.Height),
            effects: SpriteEffects.None,
            layerDepth: 0f
        );
    }


// -----------------------------------------------------------------------------
//----------------------------------------------------------------------------
//main function 

    public void move(float deltaTime, float groundY)
    {
        if (ice_powerUp_in_use) return;
        if (velocity == Vector2.Zero) return;


        velocity.Y += g * deltaTime;
        position = position + velocity * deltaTime;

        if (out_of_bounds_on_right_side(position))
        {
            change_direction_x_scale();
            position.X = 1960 - BallSize - 5;
            reduce_velocity_due_to_friction();
        }

        if (out_of_bounds_on_left_side(position))
        {
            change_direction_x_scale();
            position.X = 0 + 5;
            reduce_velocity_due_to_friction();
        }

        if (out_of_bounds_on_upper_side(position))
        {
            change_direction_y_scale();
            position.Y = 0 + 5;
            reduce_velocity_due_to_friction();
        }

        if (out_of_bounds_on_down_side(position, groundY))
        {
            change_direction_y_scale();
            position.Y = groundY;
            reduce_velocity_due_to_friction();
        }


        Rect.X = (int)position.X;
        Rect.Y = (int)position.Y;

    }

    public bool handle_crossbar_collision(Rectangle crossbar)
    {
        Rectangle ballRect = getRect();

        if (ballRect.Intersects(crossbar))
        {
            // Ball aus der Kollisionszone herausbewegen BEVOR wir die Geschwindigkeit ändern
            Vector2 newPos = position;

            // Prüfen, ob Ball von oben oder unten kommt (VORHER die Geschwindigkeit umkehren!)
            if (velocity.Y > 0) // Ball bewegt sich nach unten (kommt von oben)
            {
                // Ball ÜBER die Torlatte positionieren
                newPos.Y = crossbar.Top - BallSize - 5;
            }
            else // Ball bewegt sich nach oben (kommt von unten)
            {
                // Ball UNTER die Torlatte positionieren
                newPos.Y = crossbar.Bottom + 5;
            }

            position = newPos;

            // DANACH erst die Geschwindigkeit umkehren
            change_direction_y_scale();

            // Geschwindigkeit durch Reibung reduzieren
            reduce_velocity_due_to_friction();

            // Rect aktualisieren
            Rect.X = (int)position.X;
            Rect.Y = (int)position.Y;

            return true; // Kollision erkannt
        }

        return false; // Keine Kollision
    }
}