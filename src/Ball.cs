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
 * 
 * ------------------------------------------------------------------------------------------------------------
 */

using System.ComponentModel.Design;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
//using SharpDX.XAudio2;
//using System;
//using System.Security.Cryptography.X509Certificates;
//using static System.Windows.Forms.Design.AxImporter;



public class Ball
{



    private Texture2D texture;
    public Vector2 position;

    public Vector2 velocity = new Vector2(0, 0);
    public Vector2 starting_velocity = new Vector2(500f, -230f);
    public Vector2 v_sim;
    public Vector2 shooting_velocity = new Vector2(550, -50);
    

    private const int BallSize = 100;
    private const float BallFriction = (float)1;

    private const float g = 120f;                        // (float)9.81 * (float)3.5; //gravity
    private Rectangle Rect;



    public Ball(GraphicsDevice graphicsDevice, Vector2 position2, Texture2D texture1)
    {
        position = position2;
        texture = texture1;
        Rect = new Rectangle((int)position.X, (int)position.Y, BallSize, BallSize);
    }

    public Rectangle getRect()
    {
        return Rect;
    }

    public void Set_velocity(Vector2 velocity1)
    {
        velocity = velocity1;
    }

    public void reset_velocity()
    {
        if (velocity == Vector2.Zero)
        {
            velocity = starting_velocity;
            return;
        }


        Vector2 direction = getDirection(velocity);
        velocity = direction * starting_velocity;
    }

    public void getShooted()
    {
        if (velocity == Vector2.Zero)
        {
            velocity = shooting_velocity;
            return;
        }

        velocity.X = shooting_velocity.X * getDirection(velocity).X;
        velocity.Y = shooting_velocity.Y;
    }

    public void reduce_velocity_due_to_friction()
    {
        velocity *= 0.9f;
    }




    public void change_direction()
    {
        change_direction_x_scale();
        change_direction_y_scale();
    }

    public void change_direction_x_scale()
    {
        velocity.X *= -1;
    }

    public void change_direction_y_scale()
    {
        velocity.Y *= -1;
    }




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



    public void Reset_Position(Vector2 newPosition)
    {
        position = newPosition;
        velocity = Vector2.Zero;
        Rect.X = (int)position.X;
        Rect.Y = (int)position.Y;
    }

    public void draw(SpriteBatch spritebatch)
    {
        spritebatch.Draw(texture, Rect, Color.White);
    }



    public Vector2 getDirection(Vector2 velocity1)
    {
        float xDir = velocity.X >= 0 ? 1 : -1;
        float yDir = velocity.Y >= 0 ? -1 : 1;
        return new Vector2(xDir, yDir);
    }


 //----------------------------------------------------------------------------
 //main function 

    public void move(float deltaTime, float groundY)
    {
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
}