using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.Events;

public class MyNetworkManager : NetworkManager
{

    public Transform player1SpawnPoint;
    public Transform player2SpawnPoint;
    public Transform enemySpawnPoint;

    void SpawnEnemy()
    {
        GameObject new_enemy = Instantiate(
                spawnPrefabs.Find(prefab => prefab.name == "EnemyFollower"),
                enemySpawnPoint.position, enemySpawnPoint.rotation);

        NetworkServer.Spawn(new_enemy);
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {

        Transform startPoint;

        if (numPlayers == 0)
        {
            startPoint = player1SpawnPoint;
        }
        else
        {
            startPoint = player2SpawnPoint;
            Invoke("SpawnEnemy", 5);
        }

        GameObject player =
            Instantiate(playerPrefab, startPoint.position, startPoint.rotation);

        NetworkServer.AddPlayerForConnection(conn, player);

    }


}
