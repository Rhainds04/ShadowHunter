using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class EnemyControllerConfigTest
{
    [Test]
    public void EnemyControllerConfig()
    {
        GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
        Assert.IsNotNull(enemy, $"{enemy} est null");

        var enemyController = enemy.GetComponent<EnemyController>();
        Assert.IsNotNull(enemyController, $"{enemyController} est null");

        var enemySpriteRenderer = enemy.GetComponent<SpriteRenderer>();
        Assert.IsNotNull(enemySpriteRenderer, $"{enemySpriteRenderer} est null");

        var enemyBoxCollider = enemy.GetComponent<BoxCollider2D>();
        Assert.IsNotNull(enemyBoxCollider, $"{enemyBoxCollider} est null");

        var enemyAnimator = enemy.GetComponent<Animator>();
        Assert.IsNotNull(enemyAnimator, $"{enemyAnimator} est null");

        var enemyRigidbody = enemy.GetComponent<Rigidbody2D>();
        Assert.IsNotNull(enemyRigidbody, $"{enemyRigidbody} est null");
    }
}
