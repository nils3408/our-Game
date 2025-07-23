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
        cooldown = 3.5f;
    }

    public override void activate(Player player)
    {
        player.GameLogic_object.goalScale = 0.55f;
        player.GameLogic_object.set_goal_size();
    }
}