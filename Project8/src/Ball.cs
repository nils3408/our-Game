//nils, Lukas


using System;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.XAudio2;


namespace Project8
{

    public class Ball
    {

        private Texture2D texture;
        public Vector2 position;
        public Vector2 startingPosition;  //used for Ball movement

        public Vector2 velocity = new Vector2(0, 0);
        public Vector2 starting_velocity = new Vector2(180f, 50f);
        public float v_sim;

        private const int BallSize = 50;
        private const float BallFriction = 100f;
        private const int BallMargin = 100;

        private const float g = (float)9.81;   //gravity
        public float alpha = MathHelper.ToRadians(25f);

        private Rectangle Rect;
        private float groundY;



        //-------------------------------------------------------------------
        //  - Velocity
        //      - startingVelocity/ velocity is the velocity of the ball, the players can see
        //      - simulationVelocity is the velocity we use for our calculations
        // 
        //  - ball movemetn calculation
        //     - in order to get the same parable for different velocitys we have to modify the formula quantily
        //     - example:  2* velocity -> 4* gravity
        //-------------------------------------------------------------------



        public Ball(GraphicsDevice graphicsDevice, Vector2 position2, Texture2D texture1, float groundY)
        {
            position = position2;
            texture = texture1;
            Rect = new Rectangle((int)position.X, (int)position.Y, BallSize, BallSize);
            this.groundY = groundY - BallSize + 80;
            startingPosition = position;
        }

        public Rectangle getRect()
        {
            return Rect;
        }

        public void Set_velocity(Vector2 velocity1)
        {
            velocity = velocity1;
        }




        public void move(float deltaTime)
        {
            if (velocity == Vector2.Zero) return;



            // schräger Wurf
            // https://www.leifiphysik.de/mechanik/waagerechter-und-schraeger-wurf/grundwissen/schraeger-wurf-nach-oben-mit-anfangshoehe

            position.X += velocity.X * deltaTime;
            float dx = Math.Abs(position.X- startingPosition.X);

            v_sim = velocity.X;

            float x = position.X;

            float cosAlpha = (float)Math.Cos(alpha);
            float tanAlpha = (float)Math.Tan(alpha);

            float y_pos = (-0.5f * (4*g / (v_sim * v_sim* cosAlpha * cosAlpha)) * (dx * dx)
                           + tanAlpha * dx
                           + startingPosition.Y);

            //opposite direction as in Grafik G1
            float distance_to_middle = y_pos - startingPosition.Y;
            float y2 = startingPosition.Y - distance_to_middle;

            position.X = x;
            position.Y = y2;
            Rect.X = (int)position.X;
            Rect.Y = (int)position.Y;

            // todo
            // velocity gets faster by deltaime
            // therefore we have to modify the formula autmaticely by deltaTime
        }





        public void move_linar()
        {

            position.X += velocity.X;
            position.Y += velocity.Y;
        }





        private void ApplyFriction(float deltaTime)
        {
            float frictionForce = BallFriction * deltaTime;

            // Reibung auf X
            if (Math.Abs(velocity.X) > 0.01f)
            {
                velocity.X -= Math.Sign(velocity.X) * frictionForce;

                if (Math.Sign(velocity.X) != Math.Sign(velocity.X - Math.Sign(velocity.X) * frictionForce))
                    velocity.X = 0;
            }

            // Reibung auf Y (z.B. Luftwiderstand)
            if (Math.Abs(velocity.Y) > 0.01f)
            {
                velocity.Y -= Math.Sign(velocity.Y) * (frictionForce * 0.2f); // Y-Reibung schwächer
                if (Math.Sign(velocity.Y) != Math.Sign(velocity.Y - Math.Sign(velocity.Y) * (frictionForce * 0.2f)))
                    velocity.Y = 0;
            }
        }

        public void change_direction()
        {
            //linar to linear
            // simply return the direction in comes from 
            velocity.X *= -1;
            velocity.Y *= -1;
        }

        public void reset_starting_position()
        {
            startingPosition = position;
        }

        public void draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(texture, Rect, Color.White);
        }

    }
}

