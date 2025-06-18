//nils, Lukas


/*-------------------------------------------------------------------------------------------------------------
 * - Ball movement 
 *      the ball movement is based on its velocity. velocity is a 2D Vector
 *      with the gravity parameter we can simulate a realistic parabel (=ballmovement)
 * 
 * ------------------------------------------------------------------------------------------------------------
 */

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
    public Vector2 starting_velocity = new Vector2(180f, -100f);
    public Vector2 v_sim;

    private const int BallSize = 50;
    private const float BallFriction = (float)1;

    private const float g = 100f;                        // (float)9.81 * (float)3.5; //gravity
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



    public void move(float deltaTime, float groundY)
    {
        if (velocity == Vector2.Zero) return;


        velocity.Y += g * deltaTime;
        position = position + velocity * deltaTime;

        if (out_of_bounds_on_right_side(position))
        {
            change_direction_x_scale();
            position.X = 800 - BallSize - 5;
        }

        if (out_of_bounds_on_left_side(position))
        {
            change_direction_x_scale();
            position.X = 0 + 5;
        }

        if (out_of_bounds_on_upper_side(position))
        {
            change_direction_y_scale();
            position.Y = 0 + 5;
        }

        if (out_of_bounds_on_down_side(position, groundY))
        {
            change_direction_y_scale();
            position.Y = groundY;
        }


        Rect.X = (int)position.X;
        Rect.Y = (int)position.Y;

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


    public void reset_velocity()
    {
        // while moving the ball loses velocity due to friction
        // when the ball gets kicked again, it should become faster again
        // however we have to take care of the current direction, because velocity is a vector

        if (velocity == Vector2.Zero)
        {
            velocity = starting_velocity;
        }

        else
        {
            Vector2 direction = Vector2.Normalize(velocity);
            velocity = direction * starting_velocity.Length();
        }
    }


    public bool out_of_bounds_on_right_side(Vector2 newPosition)
    {
        return ((newPosition.X + BallSize) >= 800);
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
        System.Diagnostics.Debug.WriteLine("ball on ground : " + groundY);
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


}