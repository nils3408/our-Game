


using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

public static class PrimitiveDrawer
{
    private static Texture2D pixelTexture;
    private static SpriteFont font;

    public static void Initialize(GraphicsDevice graphicsDevice, ContentManager Content)
    {
        pixelTexture = new Texture2D(graphicsDevice, 1, 1);
        pixelTexture.SetData(new[] { Color.White });

        font = Content.Load<SpriteFont>("Arial");
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

    public static void DrawTriangle(SpriteBatch spriteBatch, Vector2 a, Vector2 b, Vector2 c, Color color)
    {

        Vector2 AtoB = new Vector2(b.X - a.X, b.Y - a.Y);
        float stepAmount = 1 / (AtoB.Length() * 3f);

        for (float p = 0; p <= 1; p += stepAmount)
        {
            float curX = (a.X + p * AtoB.X);
            float curY = (a.Y + p * AtoB.Y);
            Vector2 start = new Vector2(curX, curY);
            Vector2 end = c;

            DrawLine(spriteBatch, start, c, color, 0.5f);
        }

    }

    public static void DrawText(SpriteBatch spriteBatch, Point position, string text, int height, Color color)
    {
        if (text != "")
        {
            Vector2 textSize = font.MeasureString(text);

            float scaling = height / (float)textSize.X;


            spriteBatch.DrawString(font, text, PointToVector2(position), Color.Black, 0f, Vector2.Zero, scaling, SpriteEffects.None, 0);
        }
    }

    public static void DrawText(SpriteBatch spriteBatch, Rectangle bounds, string text, Color color)
    {
        if (text != "")
        {
            Vector2 textSize = font.MeasureString(text);

            Vector2 textPosition = new Vector2(
                bounds.Left + (bounds.Width - textSize.X) / 2,
                bounds.Top + (bounds.Height - textSize.Y) / 2
            );

            spriteBatch.DrawString(font, text, textPosition, Color.Black);
        }
    }

    public static Vector2 PointToVector2(Point point)
    {
        return new Vector2(point.X, point.Y);
    }

    
}