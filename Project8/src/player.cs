using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Runtime.CompilerServices;

namespace Project8
{
    public class Player
    {

        private Texture2D texture;
        public float move_speed;
        public Vector2 position;
        private float jump_velocity= -400f;
        public  Rectangle currentRect;
        public  Rectangle futureRect;
        private const int RectangleWidth = 50;
        private const int RectangleHeight = 50;
        public Player otherPlayer;
        public float newPositionX;
        public float gravity = 9.81f;
        public Vector2 newPositionY;  



        public Player(GraphicsDevice graphicsDevice, Vector2 position1, Texture2D texture1)
        {
            position = position1;
            texture = texture1;
            currentRect = new Rectangle ((int)position.X, (int)position.Y, RectangleWidth, RectangleHeight);
            futureRect = new Rectangle  ((int)position.X, (int)position.Y, RectangleWidth, RectangleHeight);
        }

        public void Set_other_Player(Player otherPlayer1)
        {
            otherPlayer= otherPlayer1;
        }




        public void move_left(float delta)
        {
            newPositionX = position.X - delta *move_speed;
            futureRect = new Rectangle((int)newPositionX, (int)position.Y, RectangleWidth, RectangleHeight);

            if (!(futureRect.Intersects(otherPlayer.currentRect)) && (out_of_bounds()==false))
            {
                position.X -= move_speed * delta;
                update_rectangles();
            }     
        }


        public void move_right(float delta)
        {
            newPositionX = position.X + delta * move_speed;
            futureRect = new Rectangle ((int)newPositionX, (int)position.Y, RectangleWidth, RectangleHeight);

            if (!(futureRect.Intersects(otherPlayer.currentRect)) && (out_of_bounds() == false))
            {
                position.X += move_speed * delta;
                update_rectangles();    
            }
        }


        public void jump(float delta)
        {
            // todo 

        }



        public void update_rectangles()
        {
            //update current_rectangle and future_rectangle
            currentRect = new Rectangle( (int) position.X, (int) position.Y, RectangleWidth, RectangleHeight);
            futureRect = new Rectangle ( (int)  position.X, (int)position.Y, RectangleWidth, RectangleHeight);

        }

        public bool out_of_bounds()
        {
            // todo
            return true;
        }


        public void draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(texture, 
                             currentRect, null, Color.White, 0f, Vector2.Zero, 
                             SpriteEffects.None, 0f
                             );
            // todo;
        }


    }


    
}

