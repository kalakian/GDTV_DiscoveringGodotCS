using Godot;
using System;

public class bulletBrain : Node
{
    scenes scenes = new scenes();
    Timer enemySpawner;
    [Export] public float maxSpawnInterval = 4;
    [Export] public float minSpawnInterval = 0.5f;
    [Export] public float spawnIntervalDecrease = 0.2f;
    public float spawnInterval = 0;
    [Export] public int playerBulletSpeed = 300;
    [Export] public int enemyBulletSpeed = 250;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        enemySpawner = (Timer)GetNode("enemySpawner");
        spawnInterval = maxSpawnInterval;
        enemySpawner.WaitTime = spawnInterval;
        enemySpawner.Start();
    }

    public void increaseDifficulty()
    {
        var newSpawnInterval = spawnInterval - spawnIntervalDecrease;
        spawnInterval = Math.Max(newSpawnInterval, minSpawnInterval);
        enemySpawner.WaitTime = spawnInterval;
        enemySpawner.Start();
    }

    public void _on_enemySpawner_timeout()
    {
        spawnEnemy();
    }

    public void spawnEnemy()
    {
        Vector2 spawnPosition = new Vector2(Convert.ToSingle(GD.RandRange(0, 1024)), -30);
        Vector2 targetPosition = new Vector2(Convert.ToSingle(GD.RandRange(0, 1024)), 550);
        spawnBullet(spawnPosition, targetPosition, "enemy");
    }

    public void spawnBullet(Vector2 spawnPosition, Vector2 targetPosition, string animationName)
    {
        // Spawn bullet at position, looking at target position
        var bullet = (bullet)scenes._sceneBullet.Instance();
        GetNode("/root/game/bullets").AddChild(bullet);
        bullet.GlobalPosition = spawnPosition;
        bullet.LookAt(targetPosition);

        // Set the bullet animation
        var bulletSprite = (AnimatedSprite)bullet.GetNode("AnimatedSprite");
        bulletSprite.Play(animationName);

        if (animationName == "player")
        {
            bullet.speed = playerBulletSpeed;
        }
        else
        {
            bullet.speed = enemyBulletSpeed;
        }
    }

    public void spawnExplosion(Vector2 spawnPosition, string animationName)
    {
        // Spawn explosion at position
        var explosion = (Area2D)scenes._sceneExplosion.Instance();
        GetNode("/root/game/bullets").AddChild(explosion);
        explosion.GlobalPosition = spawnPosition;

        // Set the explosion animation
        var explosionSprite = (AnimatedSprite)explosion.GetNode("AnimatedSprite");
        explosionSprite.Play(animationName);
    }
}
