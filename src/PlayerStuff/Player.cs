//nils, Lukas


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Runtime.CompilerServices;


    public class Player
    {

        public Texture2D texture;
        public Player otherPlayer;
        public int playerGroup;
        
        public float move_speed = 380f;
        public float jump_velocity = -500f;
        public float gravity = 500f;
        public Vector2 velocity;

        public Rectangle currentRect;
        public Rectangle futureRect;
        public const int RectangleWidth = 150;
        public const int RectangleHeight = 150;

        public Vector2 position;
        public float maxHeightY =3 ;
        
        

    /*
     * -------------------------------------------------
     * heredity
     *      each player has a special effect that gets executed through function do_special_effect()
     *      this function gets overwritten in each child-class inherting from this one
     *      
     * --------------------------------------------------
     */


        public Player(GraphicsDevice graphicsDevice, Vector2 position1, Texture2D texture1, int player)
        {
            position = position1;
            playerGroup = player;
            texture = texture1;
            currentRect = new Rectangle((int)position.X, (int)position.Y, RectangleWidth, RectangleHeight);
            futureRect = new Rectangle((int)position.X, (int)position.Y, RectangleWidth, RectangleHeight);
        }

        
        public void Set_other_Player(Player otherPlayer1)
        {
            otherPlayer = otherPlayer1;
        }

        
        public virtual void do_special_effect(float delta)
        {
            // this player does nothing special;
            return;
        }


        public void draw(SpriteBatch spritebatch)
        {
            if (playerGroup == 2)
            {
                spritebatch.Draw(texture,
                                 currentRect, null, Color.White, 0f, Vector2.Zero,
                                 SpriteEffects.FlipHorizontally, 0f
                                 );
            }
            else
            {
                spritebatch.Draw(texture,
                             currentRect, null, Color.White, 0f, Vector2.Zero,
                             SpriteEffects.None, 0f
                             );

            }
        }

  
        public void move_left(float delta)
        {
                float newPositionX = position.X - delta * move_speed;
                Vector2 newPosition = new Vector2(newPositionX, position.Y);
                
                futureRect = new Rectangle((int)newPositionX, (int)position.Y, RectangleWidth, RectangleHeight);

                if (!(futureRect.Intersects(otherPlayer.currentRect)))
                {
                    if (out_of_bounds_both_scales (newPosition) == false)
                    { 
                        position.X -= move_speed * delta;
                        update_rectangles();
                    }
                }
        }


        public void move_right(float delta)
        {
            float newPositionX = position.X + delta * move_speed;
            Vector2 newPosition = new Vector2(newPositionX, position.Y);

            futureRect = new Rectangle((int)newPositionX, (int)position.Y, RectangleWidth, RectangleHeight);

            if (!(futureRect.Intersects(otherPlayer.currentRect)))
            {
                if (out_of_bounds_both_scales(newPosition) == false)
                {
                    position.X += move_speed * delta;
                    update_rectangles();
                }
            }   
        }


        public void jump(float delta, float groundY)
        {
                float newPositionY = position.Y - jump_velocity * delta;
                Vector2 newPosition = new Vector2 (position.X, newPositionY);

                if (!(IsOnGround(position, groundY))) return; 
                velocity.Y = jump_velocity;
                

        }


        public void update_vertical(float delta, float groundY)
        {
            velocity.Y += gravity * delta;
            position.Y = Math.Max(position.Y + velocity.Y * delta, maxHeightY);

            if (position.Y >= groundY)
            {
                position.Y = groundY;
                velocity.Y = 0;
            }

            update_rectangles();
        }



        public void update_rectangles()
        {
            //update current_rectangle and future_rectangle
            currentRect = new Rectangle((int)position.X, (int)position.Y, RectangleWidth, RectangleHeight);
            futureRect = new Rectangle((int)position.X, (int)position.Y, RectangleWidth, RectangleHeight);

        }

        
        
        public bool out_of_bounds_both_scales(Vector2 newPosition1)
        {
            return (out_of_bounds_X_Scale(newPosition1) || out_of_bounds_Y_Scale(newPosition1));
        }
        
        
        public bool out_of_bounds_X_Scale(Vector2 newPosition1)
        {
            if (newPosition1.X >= 1800) return true;
            if (newPosition1.X < 0) return true;
            return false;
        }
        
        public bool out_of_bounds_Y_Scale(Vector2 newPosition1)
        {
            if (newPosition1.Y < 0) return true;
            return false;
        }


        public bool IsOnGround(Vector2 position, float groundY)
        {
            return position.Y >= groundY;
        }


    }
