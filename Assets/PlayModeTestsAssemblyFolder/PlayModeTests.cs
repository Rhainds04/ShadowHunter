using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

public class PlayModeTests
{
    [UnityTest]
    public IEnumerator TestEnemyCollisionWithPlayer()
    {
        //Arrange
        var EnemyPrefab = Resources.Load<GameObject>("Enemy");
        var PlayerPrefab = Resources.Load<GameObject>("Player");
        var GameManagerPrefab = Resources.Load<GameObject>("GameManager");

        var gameManager = GameObject.Instantiate(GameManagerPrefab);

        var enemy = GameObject.Instantiate(EnemyPrefab);
        var player = GameObject.Instantiate(PlayerPrefab);

        enemy.transform.position = player.transform.position;

        //Act
        yield return null;

        //Assert
        Assert.AreEqual(9, GameManager.Instance._health, $"the player health should be equal to 9");

        //Destroy
        Object.Destroy(enemy);
        Object.Destroy(player);
        Object.Destroy(gameManager);
    }

    [UnityTest]
    public IEnumerator TestEnemyMovingRight()
    {
        //Arrange
        var EnemyPrefab = Resources.Load<GameObject>("Enemy");
        var PlayerPrefab = Resources.Load<GameObject>("Player");

        var enemy = GameObject.Instantiate(EnemyPrefab);
        var player = GameObject.Instantiate(PlayerPrefab);

        enemy.transform.position = new Vector3(player.transform.position.x + 5, player.transform.position.y, player.transform.position.z);

        var enemyInitialPosX = enemy.transform.position.x;
        Debug.Log(enemy.transform.position);
        //Act
        yield return new WaitForSeconds(1f);
        Debug.Log(enemy.transform.position);

        //Assert
        Assert.IsTrue(enemyInitialPosX > enemy.transform.position.x, "the enemy did not move as expected");
        Assert.IsTrue(enemy.GetComponent<EnemyController>().spriteRenderer.flipX == true);//checking if sprite is flipped

        //Destroy
        Object.Destroy(enemy);
        Object.Destroy(player);
    }

    [UnityTest]
    public IEnumerator TestEnemyMovingLeft()
    {
        //Arrange
        var EnemyPrefab = Resources.Load<GameObject>("Enemy");
        var PlayerPrefab = Resources.Load<GameObject>("Player");

        var enemy = GameObject.Instantiate(EnemyPrefab);
        var player = GameObject.Instantiate(PlayerPrefab);

        enemy.transform.position = new Vector3(player.transform.position.x - 5, player.transform.position.y, player.transform.position.z);

        var enemyInitialPosX = enemy.transform.position.x;
        Debug.Log(enemy.transform.position);
        //Act
        yield return new WaitForSeconds(1f);
        Debug.Log(enemy.transform.position);

        //Assert
        Assert.IsTrue(enemyInitialPosX < enemy.transform.position.x, "the enemy did not move as expected");
        Assert.IsTrue(enemy.GetComponent<EnemyController>().spriteRenderer.flipX == false);//checking if sprite is flipped

        //Destroy
        Object.Destroy(enemy);
        Object.Destroy(player);
    }

    [UnityTest]
    public IEnumerator TestEnemyDead()
    {
        //Arrange
        var enemyPrefab = Resources.Load<GameObject>("Enemy");

        var enemy = GameObject.Instantiate(enemyPrefab);

        enemy.GetComponent<EnemyController>()._IsDead = true;

        //Act
        yield return new WaitForSeconds(.1f);

        //Assert
        Assert.IsTrue(enemy == null, $"{enemy} n'a pas été -> Destroy");

        //Destroy
        if(enemy != null)
        {
            Object.Destroy(enemy);
        }
    }

    [UnityTest]
    public IEnumerator TestEnemyTakeDamage()
    {
        //Arrange
        var enemyPrefab = Resources.Load<GameObject>("Enemy");
        var playerPrefab = Resources.Load<GameObject>("Player");

        var player = GameObject.Instantiate(playerPrefab);
        player.transform.position = new Vector3(10, 0, 0);//s'assurer que Enemy ne perd pas de vie à cause du player.
        var enemy = GameObject.Instantiate(enemyPrefab);

        var enemyInitialHealth = enemy.GetComponent<EnemyController>()._health;
        enemy.GetComponent<EnemyController>().OnEnemyHit(enemy, 1f);//enemy will take 1 damage

        //Act
        yield return new WaitForSeconds(.1f);

        var enemyHealthAfterHit = enemy.GetComponent<EnemyController>()._health;

        //Assert
        Assert.AreNotEqual(enemyInitialHealth, enemyHealthAfterHit, $"{enemyInitialHealth} should not be equal {enemyHealthAfterHit}");

        //Destroy
        Object.Destroy(enemy);
    }
}
