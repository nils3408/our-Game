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



public class FireballPowerUp : PowerUp
{
    public FireballPowerUp(Ball ball) : base(ball) { }

    public override void activate(Player player)
    {
        Debug.WriteLine("Fireball-PowerUp activated");
    }
}