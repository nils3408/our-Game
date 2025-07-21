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



public class BiggerGoalPowerUp : PowerUp
{
    public BiggerGoalPowerUp(Ball ball) : base(ball)
    {
        cooldown = 2.5f;
    }

    public override void activate(Player player)
    {
        ball.reset_values();
        ball.activation_time_powerUp = DateTime.Now;
        ball.powerUp_cooldown = cooldown;

        ball.texture = ball.powerUp_textures["icefootball"];
        ball.ice_powerUp_in_use = true;
    }
}