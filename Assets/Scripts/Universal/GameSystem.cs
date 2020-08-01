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

            StartupGenerate();

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

        private void StartupGenerate()
        {
            var blankChunk = new Chunk(0, 0, null, null);

            _current = new Chunk(0, 0, _generator, _mapper);
            _current.Generate();
            _generatedChunks.Add(_current);
            CheckChunk(_current, false, blankChunk);

            var chunkId = _current.ID;

            var northId = new Point((int)chunkId.X, (int)chunkId.Y + 1);
            var eastId = new Point((int)chunkId.X + 1, (int)chunkId.Y);
            var southId = new Point((int)chunkId.X, (int)chunkId.Y - 1);
            var westId = new Point((int)chunkId.X - 1, (int)chunkId.Y);

            

            var newChunk = new Chunk(northId, _generator, _mapper);
            _northChunk = newChunk;
            _northChunk.Generate();
            _generatedChunks.Add(_northChunk);
            CheckChunk(_northChunk, false, blankChunk);

            newChunk = new Chunk(eastId, _generator, _mapper);
            _eastChunk = newChunk;
            _eastChunk.Generate();
            _generatedChunks.Add(_eastChunk);
            CheckChunk(_eastChunk, false, blankChunk);

            newChunk = new Chunk(southId, _generator, _mapper);
            _southChunk = newChunk;
            _southChunk.Generate();
            _generatedChunks.Add(_southChunk);
            CheckChunk(_southChunk, false, blankChunk);

            newChunk = new Chunk(westId, _generator, _mapper);
            _westChunk = newChunk;
            _westChunk.Generate();
            _generatedChunks.Add(_westChunk);
            CheckChunk(_westChunk, false, blankChunk);
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
                var id = northId;

                var exists = _generatedChunks.GetChunk(id) != null;
                var newChunk = exists ? _generatedChunks.GetChunk(id) : new Chunk(id, _generator, _mapper);

                _northChunk = newChunk;
                _northChunk.Generate();

                if (!exists)
                {
                    _generatedChunks.Add(_northChunk);
                }

                CheckChunk(_northChunk, true, _southChunk);
            }

            if (_eastChunk == null)
            {
                var id = eastId;

                var exists = _generatedChunks.GetChunk(id) != null;
                var newChunk = exists ? _generatedChunks.GetChunk(id) : new Chunk(id, _generator, _mapper);

                _eastChunk = newChunk;
                _eastChunk.Generate();

                if (!exists)
                {
                    _generatedChunks.Add(_eastChunk);
                }

                CheckChunk(_eastChunk, true, _westChunk);
            }

            if (_southChunk == null)
            {
                var id = southId;

                var exists = _generatedChunks.GetChunk(id) != null;
                var newChunk = exists ? _generatedChunks.GetChunk(id) : new Chunk(id, _generator, _mapper);

                _southChunk = newChunk;
                _southChunk.Generate();

                if (!exists)
                {
                    _generatedChunks.Add(_southChunk);
                }

                CheckChunk(_southChunk, true, _northChunk);
            }

            if (_westChunk == null)
            {
                var id = westId;

                var exists = _generatedChunks.GetChunk(id) != null;
                var newChunk = exists ? _generatedChunks.GetChunk(id) : new Chunk(id, _generator, _mapper);

                _westChunk = newChunk;
                _westChunk.Generate();

                if (!exists)
                {
                    _generatedChunks.Add(_westChunk);
                }

                CheckChunk(_westChunk, true, _eastChunk);
            }
        }

        private void CheckChunk(Chunk newChunk, bool deleteOldChunk, Chunk oldChunk)
        {
            newChunk.Generate();
            _loadedChunks.Add(newChunk);
            LoadChunk(newChunk);
            if (deleteOldChunk && oldChunk != null)
            {
                UnloadChunk(oldChunk);
                _loadedChunks.Remove(oldChunk);
            }
        }

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
