using IslandsOfRenguard.Assets.Scripts.World;
//using IslandsOfRenguard.Scripts.Frontend;
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
        //private GridManager _grid;
        private GeneratorSettings _generator;
        private WorldMapperSettings _mapper;

        private List<Chunk> _generatedChunks = new List<Chunk>();
        private List<Chunk> _loadedChunks = new List<Chunk>();
        private Chunk _current;
        private Chunk _northChunk = null;
        private Chunk _eastChunk = null;
        private Chunk _southChunk = null;
        private Chunk _westChunk = null;

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

            //var gridObj = GameObject.FindWithTag("Grid");
            //if (gridObj != null)
            //    _grid = gridObj.GetComponent<GridManager>();
            //else
            //    Debug.LogError("Could not find Grid Object");

            var playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
                _player = playerObj.GetComponent<PlayerManager>();
            else
                Debug.LogError("Could not find Player Object");

            DoChunkTest();

            InputEvents input = GetComponent<InputEvents>();
            input.OnMovementKeyPressed += OnMovement;
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnMovement(object sender, EventArgs e)
        {
            _player.CheckMove(sender, e);
            //_grid.Regenerate(sender, e);
            DoChunkTest();
        }

        private void DoChunkTest()
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

            if (_current == null)
            {
                _current = new Chunk(0, 0, _generator, _mapper);
                _current.Generate();
                _loadedChunks.Add(_current);
                LoadChunk(ref _current);
            }

            var chunkId = _current.ID;

            var northId = new Point((int)chunkId.X, (int)chunkId.Y + 1);
            var eastId = new Point((int)chunkId.X + 1, (int)chunkId.Y);
            var southId = new Point((int)chunkId.X, (int)chunkId.Y - 1);
            var westId = new Point((int)chunkId.X - 1, (int)chunkId.Y);

            _northChunk = _loadedChunks.GetChunk(northId);
            _eastChunk = _loadedChunks.GetChunk(eastId);
            _southChunk = _loadedChunks.GetChunk(southId);
            _westChunk = _loadedChunks.GetChunk(westId);

            
            if (_northChunk == null)
            {
                var newChunk = new Chunk(northId, _generator, _mapper);
                _northChunk = newChunk;
                CheckChunk(ref _northChunk, ref _southChunk);
            }

            if (_eastChunk == null)
            {
                var newChunk = new Chunk(eastId, _generator, _mapper);
                _eastChunk = newChunk;
                CheckChunk(ref _eastChunk, ref _westChunk);
            }

            if (_southChunk == null)
            {
                var newChunk = new Chunk(southId, _generator, _mapper);
                _southChunk = newChunk;
                CheckChunk(ref _southChunk, ref _northChunk);
            }

            if (_westChunk == null)
            {
                var newChunk = new Chunk(westId, _generator, _mapper);
                _westChunk = newChunk;
                CheckChunk(ref _westChunk, ref _eastChunk);
            }
        }

        private void CheckChunk(ref Chunk newChunk, ref Chunk oldChunk)
        {
            newChunk.Generate();
            _loadedChunks.Add(newChunk);
            LoadChunk(ref newChunk);
            if (oldChunk != null)
            {
                UnloadChunk(ref oldChunk);
                _loadedChunks.Remove(oldChunk);
            }
        }

        private void LoadChunk(ref Chunk chunk)
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

        private void UnloadChunk(ref Chunk chunk)
        {
            foreach (var obj in chunk.Objects)
            {
                Destroy(obj);
            }
            chunk.Objects.Clear();
        }
    }
}
