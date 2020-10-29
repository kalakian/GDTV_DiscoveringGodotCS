using Godot;
using System;

public class player : Node
{
    bulletBrain bulletBrain;
    public bool canShoot = true;
    public int health = 3;
    public int score = 0;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        bulletBrain = (bulletBrain)GetNode("/root/game/bullets/bulletBrain");
        updateUI();
    }

    public void hitPlayer(int damageAmount = 1)
    {
        health = Math.Max(health - damageAmount, 0);
        updateUI();
    }

    public void addScore(int scoreAmount = 1)
    {
        score += scoreAmount;
        updateUI();
    }

    public void updateUI()
    {
        var healthAndScore = (Label)GetNode("/root/game/hud/healthAndScore");
        var newHudText = "Health: " + health + "     Score: " + score;
        healthAndScore.Text = newHudText;
    }

    public void _on_playerHitZone_area_entered(Area2D bullet)
    {
        var bulletType = (AnimatedSprite)bullet.GetNodeOrNull("AnimatedSprite");
        if ((bulletType != null) && (bulletType.Animation == "enemy") && (bullet is bullet))
        {
            bulletBrain.spawnExplosion(bullet.GlobalPosition, "enemy");
            bullet.QueueFree();
            hitPlayer();
        }
    }
}
