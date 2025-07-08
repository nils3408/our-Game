//nils, Lukas


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Runtime.CompilerServices;


    public class Player
    {

        public Texture2D texture;
        public Player otherPlayer;
        public int playerGroup;
        
        public float move_speed = 380f;
        public float move_speed2;  //copy if move_speed gets changed (for example in sonic character)
        public float jump_velocity = -500f;
        public float jump_velocity2;  //copy if jump_velocity gets changed(for example in sonic character)
        public float gravity = 500f;
        public Vector2 velocity;

        public Rectangle currentRect;
        public Rectangle futureRect;
        public const int RectangleWidth = 150;
        public const int RectangleHeight = 150;

        public Vector2 position;
        public float maxHeightY =3 ;
        
        public bool can_do_specialeffect;
        public int strength;
        
        

    /*
     * -------------------------------------------------
     * heredity
     *      each player has a special effect that gets executed through function do_special_effect()
     *      this function gets overwritten in each child-class inherting from this one
     *      
     * --------------------------------------------------
     */


        public Player(GraphicsDevice graphicsDevice, Vector2 position1, Texture2D texture1, int player)
        {
            position = position1;
            playerGroup = player;
            texture = texture1;
            currentRect = new Rectangle((int)position.X, (int)position.Y, RectangleWidth, RectangleHeight);
            futureRect = new Rectangle((int)position.X, (int)position.Y, RectangleWidth, RectangleHeight);
            can_do_specialeffect = true;
            strength = 1;
            move_speed2= move_speed;
        }

        
        public void Set_other_Player(Player otherPlayer1)
        {
            otherPlayer = otherPlayer1;
        }

        
        public virtual void do_special_effect(float delta)
        {
            // this player does nothing special;
            return;
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

  
       public virtual void move(float delta, float dir)
       {
            //dir must be -1 or 1
            if ( dir != -1 && dir != 1 ) { throw new Exception("error in move() function. Dir is not -1 or 1"); }

                float newPositionX = position.X + (delta * move_speed)*dir;
                Vector2 newPosition = new Vector2(newPositionX, position.Y);

            futureRect = new Rectangle((int)newPositionX, (int)position.Y, RectangleWidth, RectangleHeight);

            if (!(futureRect.Intersects(otherPlayer.currentRect)))
            {
                if (out_of_bounds_both_scales(newPosition) == false)
                {
                    position.X += (move_speed * delta)*dir;
                    update_rectangles();
                }
            }

              
                 
            // future rect overlaps with oponent
            if (is_stronger_than_oponent(otherPlayer))
            {
                if (oponent_is_in_the_way(otherPlayer, dir)) otherPlayer.move(delta, dir);

            
                newPosition = new Vector2(newPositionX, position.Y);
                futureRect = new Rectangle((int)newPositionX, (int)position.Y, RectangleWidth, RectangleHeight);

                if (!futureRect.Intersects(otherPlayer.currentRect) && !out_of_bounds_both_scales(newPosition))
                {
                    position.X += move_speed * delta * dir;
                    update_rectangles();
                }
            }
       }



        public void jump(float delta, float groundY)
        {
                float newPositionY = position.Y - jump_velocity * delta;
                Vector2 newPosition = new Vector2 (position.X, newPositionY);

                if (!(IsOnGround(position, groundY))) return; 
                velocity.Y = jump_velocity;
                

        }

    // TODO P1 schiebt P2 in den Boden eventuell weil stärker ?
    public virtual void update_vertical(float delta, float groundY)
    {
        velocity.Y += gravity * delta;
        float newY = Math.Max(position.Y + velocity.Y * delta, maxHeightY);

       
        Vector2 newPos = new Vector2(position.X, newY);
        Rectangle testRect = new Rectangle((int)newPos.X, (int)newPos.Y, RectangleWidth, RectangleHeight);

        // Prüfe Kollision mit anderem Spieler
        if (testRect.Intersects(otherPlayer.currentRect))
        {
            
            bool horizontalOverlap = (testRect.X < otherPlayer.currentRect.X + RectangleWidth &&
                                      testRect.X + RectangleWidth > otherPlayer.currentRect.X);

            if (horizontalOverlap)
            {
                
                float previousY = position.Y;
                float otherPlayerTop = otherPlayer.position.Y;
                float otherPlayerBottom = otherPlayer.position.Y + RectangleHeight;

                
                if (velocity.Y > 0)
                {
                    // Prüfe ob Spieler von oben kommt
                    if (previousY + RectangleHeight <= otherPlayerTop + 5) 
                    {
                        
                        position.Y = otherPlayerTop - RectangleHeight;
                        velocity.Y = 0;
                    }
                    else
                    {
                        // Spieler kommt von der Seite - stoppe Bewegung
                        velocity.Y = 0;
                    }
                }
                // Spieler springt nach oben (negative velocity.Y)
                else if (velocity.Y < 0)
                {
                    // Prüfe ob Spieler von unten kommt
                    if (previousY >= otherPlayerBottom - 5) 
                    {
                        // Spieler stößt von unten gegen den anderen Spieler
                        position.Y = otherPlayerBottom;
                        velocity.Y = 0;
                    }
                    else
                    {
                        // Spieler kommt von der Seite - stoppe Bewegung
                        velocity.Y = 0;
                    }
                }
                else
                {
                    // Keine vertikale Geschwindigkeit - stoppe Bewegung
                    velocity.Y = 0;
                }
            }
            else
            {
                // Keine horizontale Überlappung - normale Bewegung
                position.Y = newY;
            }
        }
        else
        {
            // Keine Kollision mit anderem Spieler
            position.Y = newY;
        }

        // Prüfe Kollision mit Boden
        if (position.Y >= groundY)
        {
            position.Y = groundY;
            velocity.Y = 0;
        }

        update_rectangles();

        // Sicherheitsprüfung: Verhindere dass Spieler sich überlappen
        if (currentRect.Intersects(otherPlayer.currentRect))
        {
            // Notfall-Trennung basierend auf der vorherigen Position
            if (position.Y < otherPlayer.position.Y)
            {
                // Dieser Spieler ist höher - setze ihn auf den anderen
                position.Y = otherPlayer.position.Y - RectangleHeight;
            }
            else
            {
                // Dieser Spieler ist tiefer - setze ihn unter den anderen
                position.Y = otherPlayer.position.Y + RectangleHeight;
            }
            velocity.Y = 0;
            update_rectangles();
        }
    }

    public void update_rectangles()
        {
            //update current_rectangle and future_rectangle
            currentRect = new Rectangle((int)position.X, (int)position.Y, RectangleWidth, RectangleHeight);
            futureRect = new Rectangle((int)position.X, (int)position.Y, RectangleWidth, RectangleHeight);

        }

        
        
        public bool out_of_bounds_both_scales(Vector2 newPosition1)
        {
            return (out_of_bounds_X_Scale(newPosition1) || out_of_bounds_Y_Scale(newPosition1));
        }
        
        
        public bool out_of_bounds_X_Scale(Vector2 newPosition1)
        {
            if (newPosition1.X >= 1800) return true;
            if (newPosition1.X < 0) return true;
            return false;
        }
        
        public bool out_of_bounds_Y_Scale(Vector2 newPosition1)
        {
            if (newPosition1.Y < 0) return true;
            return false;
        }


        public bool IsOnGround(Vector2 position, float groundY)
        {
            return position.Y >= groundY;
        }

        public bool is_stronger_than_oponent(Player otherPlayer1)
        {
            return (strength > otherPlayer1.strength);
        }

        public bool oponent_is_in_the_way(Player otherPlayer1, float direction)
        {
            // check if other Player is in the given direction compared to the position of the own player
            // otherPlayer would be_in_the_way 

            if (direction != -1 && direction != 1) { throw new Exception("error in in_the_way() function. Dir is not -1 or 1");}

            
            if (direction == 1)   {return otherPlayer.position.X >= position.X; } // right
            return otherPlayer.position.X <= position.X; //to the left

        }

        public virtual bool can_do_special_effect()
        {
            return can_do_specialeffect;
        }
    }

