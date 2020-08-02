using IslandsOfRenguard.Assets.Scripts.World;
using IslandsOfRenguard.Scripts.Player;
using IslandsOfRenguard.Scripts.World;
using IslandsOfRenguard.Scripts.WorldGen;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace IslandsOfRenguard.Scripts.Universal
{
    public class GameSystem : MonoBehaviour
    {
        public GameSystem Instance { get; private set; }

        private PlayerManager _player;
        private GeneratorSettings _generator;
        private WorldMapperSettings _mapper;

        private List<Chunk> _generatedChunks = new List<Chunk>();
        private List<Chunk> _loadedChunks = new List<Chunk>();
        private Chunk _current;

        void Awake()
        {
            Instance = this;
        }

        void OnDestroy()
        {
            Instance = null;
        }

        // Start is called before the first frame update
        void Start()
        {
            _generator = new GeneratorSettings(1000, 1000, 0.05F, Random.Range(0.0F, 10000.0F), 16);
            _mapper = new WorldMapperSettings(80, 100, 200);

            var playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
                _player = playerObj.GetComponent<PlayerManager>();
            else
                Debug.LogError("Could not find Player Object");

            StartupGenerate();

            InputEvents input = GetComponent<InputEvents>();
            input.OnMovementKeyPressed += OnMovement;
        }

        // Update is called once per frame
        void Update()
        {

        }

        /// <summary>
        /// Event calls every time the player moves.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMovement(object sender, EventArgs e)
        {
            _player.CheckMove(sender, e);
            RegenChunks();
        }

        private void StartupGenerate()
        {
            RegenChunks();
        }

        /// <summary>
        /// Either generates or reloads chunks coming into view and unloads chunks going out of view.
        /// </summary>
        private void RegenChunks()
        {
            int posX = (int)_player.transform.position.x;
            int posY = (int)_player.transform.position.y;

            foreach (var chunk in _loadedChunks)
            {
                if (chunk.Contains(posX, posY))
                {
                    _current = chunk;
                    break;
                }
            }

            var topLeftDraw = new Point(posX - _player.ViewDis, posY + _player.ViewDis);
            var bottomRightDraw = new Point(posX + _player.ViewDis, posY - _player.ViewDis);

            bool hasNewChunks = false;

            List<Chunk> foundChunks = new List<Chunk>();
            for (int y = (int)bottomRightDraw.Y; y < (int)topLeftDraw.Y; y++)
            {
                for (int x = (int)topLeftDraw.X; x < (int)bottomRightDraw.X; x++)
                {
                    if (_loadedChunks.GetChunkWithTile(x, y) != null)
                    {
                        foundChunks.Add(_loadedChunks.GetChunkWithTile(x, y));
                        continue;
                    }

                    var id = new Point((int)Math.Floor(x / 16d), (int)Math.Floor(y / 16d));
                    var exists = _generatedChunks.GetChunk(id) != null;
                    var newChunk = exists ? _generatedChunks.GetChunk(id) : new Chunk(id, _generator, _mapper);
                    if (!exists)
                    {
                        hasNewChunks = true;
                        newChunk.Generate();
                        _generatedChunks.Add(newChunk);
                    }
                    _loadedChunks.Add(newChunk);
                    LoadChunk(newChunk);
                    if (newChunk.Contains(posX, posY)) _current = newChunk;
                }
            }
            if (hasNewChunks) Debug.Log("Current Generated Chunks: " + _generatedChunks.Count);

            List<Chunk> removeChunks = new List<Chunk>();
            foreach (var chunk in _loadedChunks)
            {
                if (foundChunks.GetChunk(chunk.ID) == null)
                {
                    UnloadChunk(chunk);
                    removeChunks.Add(chunk);
                }
            }
            foreach (var chunk in removeChunks)
            {
                if (_loadedChunks.GetChunk(chunk.ID) != null)
                {
                    _loadedChunks.Remove(chunk);
                }
            }
        }

        /// <summary>
        /// Loads the chunk into user viewable Objects.
        /// </summary>
        /// <param name="chunk">The chunk to load.</param>
        private void LoadChunk(Chunk chunk)
        {
            GameObject grassRef = (GameObject)Instantiate(Resources.Load("Tile_Env_Grass"));
            GameObject stoneRef = (GameObject)Instantiate(Resources.Load("Tile_Env_Stone"));
            GameObject waterRef = (GameObject)Instantiate(Resources.Load("Tile_Env_Water"));

            foreach (var tileList in chunk.Tiles)
            {
                foreach (var tile in tileList)
                {
                    GameObject obj =
                        tile.ID == TileID.ENV.STONE ? Instantiate(stoneRef, transform) :
                        tile.ID == TileID.ENV.WATER ? Instantiate(waterRef, transform) :
                        Instantiate(grassRef, transform);

                    obj.transform.position = new Vector2(tile.Location.X + 0.5F, tile.Location.Y + 0.5F);
                    chunk.Objects.Add(obj);
                }
            }

            Destroy(grassRef);
            Destroy(stoneRef);
            Destroy(waterRef);
        }

        /// <summary>
        /// Deletes all the user viewable Objects in a chunk.
        /// </summary>
        /// <param name="chunk">The chunk to unload.</param>
        private void UnloadChunk(Chunk chunk)
        {
            foreach (var obj in chunk.Objects)
            {
                Destroy(obj);
            }
            chunk.Objects.Clear();
        }
    }
}
