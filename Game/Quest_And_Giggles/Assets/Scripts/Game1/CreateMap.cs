using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CreateMap : NetworkBehaviour
{
    private NetworkVariable<ulong> targetTileId = new NetworkVariable<ulong>();
    private NetworkVariable<Color> color = new NetworkVariable<Color>();

    public GameObject tile;
    public float duration = 3f;
    GameObject selectedTile;

    public List<GameObject> tiles = new List<GameObject>();

    private NetworkVariable<bool> loaded = new NetworkVariable<bool>();


    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            loaded.Value = true;
            GenerateMap();
            StartCoroutine(Randomizer());
        }

        Debug.Log("OnNetworkSpawn called on: " + (IsServer ? "SERVER" : "CLIENT"));

        targetTileId.OnValueChanged += OnTileChanged;
    }

    void GenerateMap()
    {
        float x = 0;
        float z = 0;
        float y = -3;

        for (int i = 1; i <= 60; i++)
        {
            Vector3 pos = new Vector3(x, y, z);

            GameObject obj = Instantiate(tile, pos, Quaternion.identity);

            // IMPORTANT: spawn over network
            obj.GetComponent<NetworkObject>().Spawn();

            tiles.Add(obj);

            if (i % 10 == 0)
            {
                x += 3.5f;
                z = 0;
            }
            else
            {
                z += 3.5f;
            }
        }
    }

    IEnumerator Randomizer()
    {
        while (true)
        {
            Debug.Log("Loop tick");

            yield return new WaitForSeconds(5f);

            Debug.Log(tiles.Count);

            int newIndex = UnityEngine.Random.Range(0, tiles.Count);

            selectedTile = tiles[newIndex];

            Debug.Log(newIndex);
            var netObj = tiles[newIndex].GetComponent<NetworkObject>();

            targetTileId.Value = netObj.NetworkObjectId;

            OnTileChanged(0, targetTileId.Value);
        }
    }

    void OnTileChanged(ulong oldId, ulong newId)
    {
        if (!NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(newId, out NetworkObject netObj))
        {
            Debug.LogError("Tile not found on this client!");
            return;
        }

        GameObject tile = netObj.gameObject;

        StartCoroutine(ChangeColor(tile));

    }


    IEnumerator ChangeColor(GameObject tile)
    {
        Debug.Log("Coroutine started on: " + (IsServer ? "SERVER" : "CLIENT"));

        float timeElapsed = 0f;
        MeshRenderer rend = tile.GetComponentInChildren<MeshRenderer>();

        if (rend == null)
        {
            Debug.LogError("Renderer missing!");
            yield break;
        }

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            float t = timeElapsed / duration;
            t = Mathf.SmoothStep(0f, 1f, t);

            Color newColor = Color.Lerp(Color.gray, Color.red, t);

            if (IsServer)
            {
                color.Value = newColor;
            }

            rend.material.color = newColor;

            yield return null;
        }

        if (IsServer)
        {
            tile.GetComponent<NetworkObject>().Despawn();
        }

        if (IsOwner)
        {
            tiles.Remove(selectedTile);
        }
    }
}