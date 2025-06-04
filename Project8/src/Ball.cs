//nils, Lukas


using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.XAudio2;


namespace Project8
{

    public class Ball
    {

        private Texture2D texture;
        public Vector2 position;
        public Vector2 velocity = new Vector2(0, 0);
        public Vector2 starting_velocity = new Vector2(150f, 50f);

        private const int BallSize = 50;
        private const float BallFriction = 100f;
        private const int BallMargin = 100;
        private const float Gravity = 50f;
        private Rectangle Rect;
        private float groundY;

        public Ball(GraphicsDevice graphicsDevice, Vector2 position2, Texture2D texture1, float groundY)
        {
            position = position2;
            texture = texture1;
            Rect = new Rectangle((int)position.X, (int)position.Y, BallSize, BallSize);
            this.groundY = groundY - BallSize + 80;
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

        public void move_parabel(float deltaTime)
        {
            // Schwerkraft anwenden
            velocity.Y += Gravity * deltaTime;

            // Position aktualisieren
            position += velocity * deltaTime;

            // Am Boden stoppen oder abprallen
            if (position.Y >= groundY)
            {
                position.Y = groundY;

                // Ball prallt leicht ab – oder stoppt (je nach gewünschtem Verhalten):
                velocity.Y *= -0.5f;

                // Wenn fast still, dann ganz stoppen
                if (Math.Abs(velocity.Y) < 1f)
                    velocity.Y = 0;
            }

            // Rechteck aktualisieren
            Rect.X = (int)position.X;
            Rect.Y = (int)position.Y;

            // Reibung auf X
            ApplyFriction(deltaTime);
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

