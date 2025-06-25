


using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public static class Geometry
{
    private static Texture2D pixelTexture;

    public static void Initialize(GraphicsDevice graphicsDevice)
    {
        pixelTexture = new Texture2D(graphicsDevice, 1, 1);
        pixelTexture.SetData(new[] { Color.White });
    }

    public static void DrawPixel(SpriteBatch spriteBatch, Vector2 position, Color color)
    {
        spriteBatch.Draw(pixelTexture, position, color);
    }

    public static void DrawLine(SpriteBatch spriteBatch, Vector2 start, Vector2 end, Color color, float thickness = 1f)
    {
        Vector2 direction = end - start;
        float length = direction.Length();

        if (length < 0.001f) return; // Zu kurze Linie

        float angle = (float)Math.Atan2(direction.Y, direction.X);

        spriteBatch.Draw(pixelTexture,
            start,
            null,
            color,
            angle,
            Vector2.Zero,
            new Vector2(length, thickness),
            SpriteEffects.None,
            0f);
    }

    public static void DrawRectangle(SpriteBatch spriteBatch, Rectangle rectangle, Color color)
    {
        spriteBatch.Draw(pixelTexture, rectangle, color);
    }

    public static void DrawRectangleOutline(SpriteBatch spriteBatch, Rectangle rect, Color color, int thickness = 1)
    {
        // Oben
        spriteBatch.Draw(pixelTexture, new Rectangle(rect.X, rect.Y, rect.Width, thickness), color);
        // Unten  
        spriteBatch.Draw(pixelTexture, new Rectangle(rect.X, rect.Y + rect.Height - thickness, rect.Width, thickness), color);
        // Links
        spriteBatch.Draw(pixelTexture, new Rectangle(rect.X, rect.Y, thickness, rect.Height), color);
        // Rechts
        spriteBatch.Draw(pixelTexture, new Rectangle(rect.X + rect.Width - thickness, rect.Y, thickness, rect.Height), color);
    }

    public static void DrawRectangleWithOutline(SpriteBatch spriteBatch, Rectangle rect, Color color, Color colorOutline, int thickness = 1)
    {
        DrawRectangle(spriteBatch, rect, color);
        DrawRectangleOutline(spriteBatch, rect, colorOutline, thickness);
    }

}