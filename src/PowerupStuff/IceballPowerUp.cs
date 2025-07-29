//nils 


using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

//using SharpDX.XAudio2;
//using System;
//using System.Security.Cryptography.X509Certificates;
//using static System.Windows.Forms.Design.AxImporter;



public class IceballPowerUp : PowerUp
{
    public IceballPowerUp(Ball ball, ContentManager content) : base(ball, content)
    {
        cooldown = 2.5f;
        powerUp_texture = content.Load<Texture2D>("powerUp_textures/iceball_texture");
    }

    public override void activate(Player player)
    {
        ball.reset_values();
        ball.activation_time_powerUp = DateTime.Now;
        ball.powerUp_cooldown = cooldown;

        ball.current_texture = ball.textures["icefootball"];
        ball.ice_powerUp_in_use = true;
    }
}