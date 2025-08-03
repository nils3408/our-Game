using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace our_Game
{
    public class BackgroundFan
    {
        private Texture2D texture;
        private Vector2 position;
        private float scale;
        private float animationSpeed;
        private float animationTime;
        private float maxBobbingHeight;
        private Vector2 originalPosition;

        public BackgroundFan(Texture2D texture, Vector2 position, float scale, float animationSpeed, float bobbingHeight)
        {
            this.texture = texture;
            this.position = position;
            this.originalPosition = position;
            this.scale = scale;
            this.animationSpeed = animationSpeed;
            this.maxBobbingHeight = bobbingHeight * 20; // Convert to pixels
            this.animationTime = 0f;
        }

        public void Update(GameTime gameTime)
        {
            animationTime += (float)gameTime.ElapsedGameTime.TotalSeconds * animationSpeed;

            // Create bobbing animation
            float bobbingOffset = (float)Math.Sin(animationTime) * maxBobbingHeight;
            position.Y = originalPosition.Y + bobbingOffset;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (texture != null)
            {
                int width = (int)(texture.Width * scale);
                int height = (int)(texture.Height * scale);

                Rectangle destRect = new Rectangle(
                    (int)position.X,
                    (int)position.Y,
                    width,
                    height
                );

                spriteBatch.Draw(texture, destRect, Color.White);
            }
        }

        // Optional: Properties to access fan information
        public Vector2 Position => position;
        public float Scale => scale;
        public Texture2D Texture => texture;
    }
}