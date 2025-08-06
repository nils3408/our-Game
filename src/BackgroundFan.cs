using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace our_Game
{
    public class BackgroundFan
    {
        private Texture2D normalTexture;
        private Texture2D excitedTexture; // Für die Toranimation
        private Vector2 position;
        private float scale;
        private float animationSpeed;
        private float animationTime;
        private float maxBobbingHeight;
        private Vector2 originalPosition;

        // Toranimation Eigenschaften
        private bool isPlayingGoalAnimation;
        private float goalAnimationTimer;
        private float goalAnimationDuration;
        private float textureSwapInterval;
        private bool useExcitedTexture;
        private float goalAnimationIntensity;
        private float originalScale;

        public BackgroundFan(Texture2D normalTexture, Texture2D excitedTexture, Vector2 position, float scale, float animationSpeed, float bobbingHeight)
        {
            this.normalTexture = normalTexture;
            this.excitedTexture = excitedTexture;
            this.position = position;
            this.originalPosition = position;
            this.scale = scale;
            this.originalScale = scale;
            this.animationSpeed = animationSpeed;
            this.maxBobbingHeight = bobbingHeight * 20;
            this.animationTime = 0f;

            // Toranimation Parameter
            this.isPlayingGoalAnimation = false;
            this.goalAnimationDuration = 5.0f; // 5 Sekunden Animation
            this.textureSwapInterval = 0.5f; // Alle 0.2 Sekunden zwischen Texturen wechseln
            this.useExcitedTexture = false;
            this.goalAnimationIntensity = 2.0f; // Verstärkte Animation bei Toren
        }

        // Überladener Konstruktor für Rückwärtskompatibilität
        public BackgroundFan(Texture2D texture, Vector2 position, float scale, float animationSpeed, float bobbingHeight)
            : this(texture, texture, position, scale, animationSpeed, bobbingHeight)
        {
        }

        public void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            animationTime += deltaTime * animationSpeed;

            // Normale Bobbing Animation
            float currentBobbingHeight = maxBobbingHeight;
            float currentAnimationSpeed = animationSpeed;

            // Toranimation Update
            if (isPlayingGoalAnimation)
            {
                goalAnimationTimer += deltaTime;

                // Debug-Ausgabe alle 0.5 Sekunden
                if ((int)(goalAnimationTimer * 2) != (int)((goalAnimationTimer - deltaTime) * 2))
                {
                    System.Diagnostics.Debug.WriteLine($"Toranimation läuft: {goalAnimationTimer:F1}s von {goalAnimationDuration}s");
                }

                // Verstärkte Animation während Torjubel
                currentBobbingHeight *= goalAnimationIntensity;
                currentAnimationSpeed *= goalAnimationIntensity;

                // Textur-Wechsel für aufgeregte Fans
                float textureTimer = goalAnimationTimer % textureSwapInterval;
                useExcitedTexture = textureTimer < (textureSwapInterval / 2);

                

                // Animation beenden
                if (goalAnimationTimer >= goalAnimationDuration)
                {
                    System.Diagnostics.Debug.WriteLine("Toranimation beendet!");
                    StopGoalAnimation();
                }
            }

            // Bobbing Animation berechnen
            float bobbingOffset = (float)Math.Sin(animationTime) * currentBobbingHeight;
            position.Y = originalPosition.Y + bobbingOffset;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Texture2D currentTexture = (isPlayingGoalAnimation && useExcitedTexture && excitedTexture != null)
                ? excitedTexture
                : normalTexture;

            if (currentTexture != null)
            {
                int width = (int)(currentTexture.Width * scale);
                int height = (int)(currentTexture.Height * scale);

                Rectangle destRect = new Rectangle(
                    (int)position.X,
                    (int)position.Y,
                    width,
                    height
                );

                
                Color drawColor = Color.White;

                spriteBatch.Draw(currentTexture, destRect, drawColor);
            }
        }

        // Toranimation starten
        public void StartGoalAnimation()
        {
            System.Diagnostics.Debug.WriteLine("StartGoalAnimation aufgerufen!");
            isPlayingGoalAnimation = true;
            goalAnimationTimer = 0f;
            useExcitedTexture = false;
        }

        // Toranimation stoppen
        public void StopGoalAnimation()
        {
            isPlayingGoalAnimation = false;
            goalAnimationTimer = 0f;
            useExcitedTexture = false;
            // Skalierung ist bereits konstant, kein Reset nötig
        }

        // Eigenschaften
        public Vector2 Position => position;
        public float Scale => scale;
        public Texture2D NormalTexture => normalTexture;
        public Texture2D ExcitedTexture => excitedTexture;
        public bool IsPlayingGoalAnimation => isPlayingGoalAnimation;

        // Animationsparameter anpassen
        public void SetGoalAnimationDuration(float duration)
        {
            goalAnimationDuration = duration;
        }

        public void SetTextureSwapInterval(float interval)
        {
            textureSwapInterval = interval;
        }

        public void SetGoalAnimationIntensity(float intensity)
        {
            goalAnimationIntensity = intensity;
        }
    }
}