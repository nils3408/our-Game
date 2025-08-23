using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace our_Game
{
    public class Goomba
    {
        Texture2D texture;

        public Vector2 position;
        public Vector2 velocity = new Vector2(280, 0);
        public float direction;

        public Rectangle current_rect;
        public Rectangle future_rect;
        public int sizeX = 35;
        public int sizeY = 50;

        public int left_bounder  = 200;
        public int right_bounder = 1700;


        public Goomba(Texture2D texture, float xpos, float groundY, float dir)
        {
            this.texture = texture;

            float y = 610;
            this.position = new Vector2(xpos, y);

            //direction must be 1 or -1
            if (dir == 1 || dir == -1)  { this.direction = dir; }
            else                        { this.direction = 1;   }
        }


        public void move(float delta)
        {
            position.X = position.X + (delta * velocity.X) * direction;
            update_rectangle();
        }

        public void update_rectangle()
        {
            current_rect = new Rectangle((int)position.X, (int)position.Y, sizeX, sizeY);
        }

        public void change_direction()
        {
            direction = direction * -1;
        }


        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, current_rect, Color.White);
        }
    }

}