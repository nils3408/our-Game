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



public class BiggerGoalPowerUp : PowerUp
{
    public BiggerGoalPowerUp(Ball ball, ContentManager content) : base(ball, content)
    {
        cooldown = 3.5f;
        powerUp_texture = content.Load<Texture2D>("powerUp_textures/BigGoal_texture");
    }

    public override void activate(Player player)
    {
        player.GameLogic_object.goalScale = 0.55f;
        player.GameLogic_object.set_goal_size();
    }
}