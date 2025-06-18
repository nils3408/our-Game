//nils, Lukas


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;


    public class Player
    {

        private Texture2D texture;
        public float move_speed = 300f;
        public Vector2 position;
        private float jump_velocity = -400f;
        public Rectangle currentRect;
        public Rectangle futureRect;
        private const int RectangleWidth = 100;
        private const int RectangleHeight = 100;
        public Player otherPlayer;
        public float newPositionX;
        public float gravity = 500f;
        public Vector2 newPositionY;
        public Vector2 velocity;
        int playerGroup;
       


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
            newPositionX = position.X - delta * move_speed;
            futureRect = new Rectangle((int)newPositionX, (int)position.Y, RectangleWidth, RectangleHeight);

            if (!(futureRect.Intersects(otherPlayer.currentRect)) && (out_of_bounds(newPositionX) == false))
            {
                position.X -= move_speed * delta;
                update_rectangles();
            }
        }


        public void move_right(float delta)
        {
            newPositionX = position.X + delta * move_speed;
            futureRect = new Rectangle((int)newPositionX, (int)position.Y, RectangleWidth, RectangleHeight);

            if (!(futureRect.Intersects(otherPlayer.currentRect)) && (out_of_bounds(newPositionX) == false))
            {

                position.X += move_speed * delta;
                update_rectangles();
            }
        }


        public void jump(float delta, float groundY)
        {
            System.Diagnostics.Debug.WriteLine("player ground: " + groundY);
            if (IsOnGround(position, groundY))
            {
                velocity.Y = jump_velocity;
            }
        }

        public void update_vertical(float delta, float groundY)
        {
            velocity.Y += gravity * delta;
            position.Y += velocity.Y * delta;

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


        public bool out_of_bounds(float newPositioX)
        {
            if (newPositionX >= 760) return true;
            if (newPositionX < 0) return true;
            return false;
        }

        public bool IsOnGround(Vector2 position, float groundY)
        {
            return position.Y >= groundY;
        }
    }
