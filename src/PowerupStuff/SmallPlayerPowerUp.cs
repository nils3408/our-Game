//nils 



using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

//using SharpDX.XAudio2;
//using System;
//using System.Security.Cryptography.X509Certificates;
//using static System.Windows.Forms.Design.AxImporter;



public class SmallPlayerPowerUp : PowerUp
{
    public SmallPlayerPowerUp(Ball ball) : base(ball) {
        cooldown = 4f;
    }

    
    public override void activate(Player player)
    {
        player.otherPlayer.RectangleHeight = 100;
        player.otherPlayer.RectangleWidth = 100;

        // would be in the Ground -> update Position to avoid this
        player.otherPlayer.groundY = 550;  //hardcoded
    }

}