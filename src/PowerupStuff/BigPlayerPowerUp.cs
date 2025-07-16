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



public class BigPlayerPowerUp : PowerUp
{
    public BigPlayerPowerUp(Ball ball) : base(ball) {
        cooldown = 3.5f;
    }

    
    public override void activate(Player player)
    {
        player.RectangleHeight = 250;
        player.RectangleWidth = 250;

        // would be in the Ground -> update Position to avoid this
        player.groundY = 410;  //hardcoded
    }

}