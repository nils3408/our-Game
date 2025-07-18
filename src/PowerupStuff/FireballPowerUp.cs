//nils 



using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

//using SharpDX.XAudio2;
//using System;
//using System.Security.Cryptography.X509Certificates;
//using static System.Windows.Forms.Design.AxImporter;



public class FireballPowerUp : PowerUp
{
    float fireball_velocityX = 1600;

    public FireballPowerUp(Ball ball) : base(ball) 
    {
        cooldown = 3f;

    }

    public override void activate(Player player)
    {
        ball.activation_time_powerUp = DateTime.Now;
        ball.powerUp_cooldown = cooldown;

        ball.texture = ball.powerUp_textures["firefootball"];
        ball.fire_powerUp_in_use = true;
        Vector2 newVelocity = new Vector2 (fireball_velocityX, ball.velocity.Y*2);
        ball.set_velocity(newVelocity * ball.transform_direction(ball.velocity));
        
    }
}