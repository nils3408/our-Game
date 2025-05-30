using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.XAudio2;


namespace Project8
{

    public class Ball
    {

        private Texture2D texture;
        public Vector2 position;
        public Vector2 velocity = new Vector2(0,0);
        public Vector2 starting_velocity= new Vector2(200f,100f);

        private const int BallSize = 32;
        private const float BallFriction = 500f;
        private const int BallMargin = 100;
        private const float Gravity = 9.81f;
        private Rectangle Rect;


        public Ball(GraphicsDevice graphicsDevice, Vector2 position2, Texture2D texture1)
        {
            position = position2;
            texture = texture1;
            Rect= new Rectangle((int)position.X, (int) position.Y, BallSize, BallSize);
        }

        public Rectangle getRect()
        {
            return Rect;
        }

        public void Set_velocity(Vector2 velocity1)
        {
            velocity = velocity1;
        }
       
        public void move_linar()
        {
            
            position.X += velocity.X;
            position.Y += velocity.Y;
        }

        public void move_parabel()
        {
            // todo
        }

        public void change_direction1()
        {
            //linar to linear
            // simply return the direction in comes from 
            velocity.X *= -1;
            velocity.Y *= -1;

        }

        public void draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(texture, Rect, Color.White);
        }

    }
}


