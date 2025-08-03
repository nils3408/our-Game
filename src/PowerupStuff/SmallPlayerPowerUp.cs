//nils 



using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

//using SharpDX.XAudio2;
//using System;
//using System.Security.Cryptography.X509Certificates;
//using static System.Windows.Forms.Design.AxImporter;



public class SmallPlayerPowerUp : PowerUp
{
    public SmallPlayerPowerUp(Ball ball, ContentManager content) : base(ball, content) {
        cooldown = 4f;
        powerUp_texture = content.Load<Texture2D> ("powerUp_textures/SmallPlayer_texture");
    }

    
    public override void activate(Player player, int powerUp)
    {
        player.otherPlayer.RectangleHeight = 100;
        player.otherPlayer.RectangleWidth = 100;

        // would be in the Ground -> update Position to avoid this
        player.otherPlayer.groundY = 550;  //hardcoded
    }

}