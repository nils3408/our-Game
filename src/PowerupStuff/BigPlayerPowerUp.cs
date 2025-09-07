//nils 



using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

//using SharpDX.XAudio2;
//using System;
//using System.Security.Cryptography.X509Certificates;
//using static System.Windows.Forms.Design.AxImporter;



public class BigPlayerPowerUp : PowerUp
{
    int bigSize = 250;

    public BigPlayerPowerUp(Ball ball, ContentManager content) : base(ball, content) {
        cooldown = 3.5f;
        powerUp_texture = content.Load<Texture2D>("powerUp_textures/BigPlayer_texture");
        
    }

    
    public override void activate(Player player, int powerUp)
    {

        player.RectangleHeight = bigSize;
        player.RectangleWidth  = bigSize;

        player.activation_time_BigPlayer_powerup = DateTime.Now;
        player.cooldown_BigPlayer_powerup = cooldown;
        player.BigPlayer_powerup_in_use = true;

        // would be in the Ground -> update Position to avoid this
        player.groundY = 410;  //hardcoded
    }


    public override int getNewSize()
    {
        return bigSize;
    }
}