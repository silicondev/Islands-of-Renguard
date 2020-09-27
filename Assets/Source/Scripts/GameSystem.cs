using dEvine_and_conquer.Base;
using dEvine_and_conquer.Entity;
using dEvine_and_conquer.World;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace dEvine_and_conquer.Scripts
{
    public class GameSystem : MonoBehaviour
    {
        public GameSystem Instance { get; private set; }

        private Dictionary<string, GameObject> _prefabs;
        private GameObject _prefabObj;

        public PlayerManager Player;
        public Generator Generator;
        public WorldMapperSettings Mapper;

        public List<Chunk> GeneratedChunks = new List<Chunk>();
        public List<Chunk> LoadedChunks = new List<Chunk>();

        public bool TileSelected { get; set; } = false;
        public Point SelectedTile { get; private set; }

        public XYContainer<Chunk> LoadedChunks2D
        {
            get
            {
                if (_loadedChunks2DHold == null)
                {
                    int lowestX = (int)LoadedChunks.MinValue(c => c.ID.X);
                    int lowestY = (int)LoadedChunks.MinValue(c => c.ID.Y);
                    int heighestX = (int)LoadedChunks.MaxValue(c => c.ID.X);
                    int heighestY = (int)LoadedChunks.MaxValue(c => c.ID.Y);

                    XYContainer<Chunk> chunks2D = new List<List<Chunk>>();
                    for (int y = lowestY; y < heighestY; y++)
                    {
                        chunks2D.AddLine();
                        for (int x = lowestX; x < heighestX; x++)
                        {
                            chunks2D.Add(LoadedChunks.GetChunk(x, y));
                        }
                    }

                    _loadedChunks2DHold = chunks2D;
                }

                return _loadedChunks2DHold;
            }
        }

        private XYContainer<Chunk> _loadedChunks2DHold = null;

        private Chunk _current;

        public Tile CurrentTile => LoadedChunks.GetTileFromID(new Point((int)Math.Floor(Player.Location.X), (int)Math.Floor(Player.Location.Y)));
        public Tile StartTile = null;
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
            _prefabObj = Instantiate(new GameObject(), transform);
            _prefabObj.name = "Prefabs";
            _prefabObj.SetActive(false);
            _prefabObj.transform.position = new Vector3(0, 0, 10);

            _prefabs = new Dictionary<string, GameObject>() {
                { "tile:grass", (GameObject)Instantiate(Resources.Load("Prefabs/Tile/Tile_Env_Grass"), _prefabObj.transform) },
                { "tile:stone", (GameObject)Instantiate(Resources.Load("Prefabs/Tile/Tile_Env_Stone"), _prefabObj.transform) },
                { "tile:water", (GameObject)Instantiate(Resources.Load("Prefabs/Tile/Tile_Env_Water"), _prefabObj.transform) },
                { "tile:sand", (GameObject)Instantiate(Resources.Load("Prefabs/Tile/Tile_Env_Sand"), _prefabObj.transform) },

                { "overlay:tree", (GameObject)Instantiate(Resources.Load("Prefabs/Overlay/Overlay_Env_Tree"), _prefabObj.transform) },

                { "entity:human", (GameObject)Instantiate(Resources.Load("Prefabs/Entity/Human/Entity_Human_Male"), _prefabObj.transform) },

                { "ui:selector", (GameObject)Instantiate(Resources.Load("Prefabs/UI/UI_Selector"), _prefabObj.transform) }
            };

            foreach (var obj in _prefabs)
            {
                obj.Value.transform.position = new Vector3(0, 0, 10);
                obj.Value.SetActive(false);
                obj.Value.name = obj.Key + ";PREFAB";
            }

            var seed = Random.Range(0.0F, 10000000.0F);
            Mapper = new WorldMapperSettings(80, 100, 105, 200);
            Generator = new Generator(0.01F, seed, 16, new WorldMapper(Mapper));
            Debug.Log("Seed: " + seed.ToString());

            var playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
            {
                Player = playerObj.GetComponent<PlayerManager>();
                Player.Location = new Point(0, 0);
            }
            else
                Debug.LogError("Could not find Player Object");

            //var hud = playerObj.GetComponentsInChildren()

            StartupGenerate();

            InputEvents input = GetComponent<InputEvents>();
            input.OnMovementKeyPressed += OnMovement;
            input.OnScroll += OnScroll;
            input.OnKeyPressed += OnKeyPress;
            input.OnMouseClick += OnClick;
        }

        // Update is called once per frame
        void Update()
        {
            LoadedChunks.UpdateAll(this);
        }

        /// <summary>
        /// Event calls every time the player moves.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMovement(object sender, EventArgs e)
        {
            Player.OnMove(sender, e);
            RegenChunks();
        }

        private void OnScroll(object sender, EventArgs e)
        {
            Player.OnScroll(sender, e);
            RegenChunks();
        }

        private void OnKeyPress(object sender, EventArgs e)
        {
            KeyEventArgs args = (KeyEventArgs)e;
            if (args.KeyPressed == KeyCode.G)
            {
                if (TileSelected && LoadedChunks.AddEntityToWorld(new HumanEntity(SelectedTile, 1)))
                {
                    Debug.Log("Entity Human Added!");
                    ForceRegen();
                    
                } else 
                    Debug.Log("ERROR: Could not add entity to world.");
            } else if (args.KeyPressed == KeyCode.H && TileSelected)
            {
                var entities = LoadedChunks.GetAllEntities();
                var entity = entities[Random.Range(0, entities.Count - 1)];
                entity.GoTo(SelectedTile, this);
            } else if (args.KeyPressed == KeyCode.Escape)
            {
                TileSelected = false;
                ForceRegen();
            }
        }

        private void OnClick(object sender, EventArgs e)
        {
            ClickEventArgs args = (ClickEventArgs)e;
            if (args.Button == MouseButton.LEFT)
            {
                var worldPos = Camera.main.ScreenToWorldPoint(args.Position);

                Debug.Log(string.Format("Attempting to find GameObject at {0},{1} ({2})", worldPos.x.ToString(), worldPos.y.ToString(), worldPos.z.ToString()));

                var flatPos = new Point(worldPos.x, worldPos.y);

                List<Chunk> found = LoadedChunks.Where(x => x.Contains(flatPos)).ToList();

                if (found.Any())
                {
                    var chunk = found.First();
                    var x = flatPos.X.Floor();
                    var y = flatPos.Y.Floor();
                    var tile = chunk.Tiles.GetWorldPoint(x, y);
                    var overlay = chunk.Overlays.GetWorldPoint(x, y);
                    Debug.Log(string.Format("Tile: {0}. Overlay: {1}", tile.Type.Name, overlay.Type.Name));

                    SelectedTile = tile.Location;
                    TileSelected = true;
                    ForceRegen();
                } else
                    Debug.Log("ERROR: Could not find object.");
            }
        }

        private void StartupGenerate()
        {
            RegenChunks();
            StartTile = CurrentTile;
        }

        public void CreateEntity(GenericEntity entity)
        {
            LoadedChunks.GetChunkWithPosition(entity.Location)?.Entities.Add(entity);
        }

        private void ForceRegen()
        {
            List<Chunk> remove = new List<Chunk>(LoadedChunks);
            RemoveChunks(remove);
            RegenChunks();
        }

        /// <summary>
        /// Either generates or reloads chunks coming into view and unloads chunks going out of view.
        /// </summary>
        private void RegenChunks()
        {
            _loadedChunks2DHold = null;
            int posX = (int)Player.Location.X;
            int posY = (int)Player.Location.Y;

            var topLeftDraw = new Point(posX - Player.ViewDis.X, posY + Player.ViewDis.Y);
            var bottomRightDraw = new Point(posX + Player.ViewDis.X, posY - Player.ViewDis.Y);

            bool hasNewChunks = false;

            List<Chunk> foundChunks = new List<Chunk>();
            for (int y = (int)bottomRightDraw.Y; y < (int)topLeftDraw.Y; y++)
            {
                for (int x = (int)topLeftDraw.X; x < (int)bottomRightDraw.X; x++)
                {
                    // If chunk is already loaded, then skip this loop
                    if (LoadedChunks.GetChunkWithPosition(x, y) != null)
                    {
                        foundChunks.Add(LoadedChunks.GetChunkWithPosition(x, y));
                        continue;
                    }

                    // Find chunk if exists
                    var id = new Point((int)Math.Floor(x / 16d), (int)Math.Floor(y / 16d));
                    var exists = GeneratedChunks.GetChunk(id) != null;
                    var newChunk = exists ? GeneratedChunks.GetChunk(id) : new Chunk(id, Generator, this);

                    // Chunk does not already exist, generate a new one.
                    if (!exists)
                    {
                        hasNewChunks = true;
                        newChunk.Generate();
                        GeneratedChunks.Add(newChunk);
                    }

                    // Load chunk
                    LoadedChunks.Add(newChunk);
                    LoadChunk(newChunk);
                }
            }

            if (hasNewChunks) 
                Debug.Log("Current Generated Chunks: " + GeneratedChunks.Count);

            // Unload unneeded chunks
            List<Chunk> removeChunks = new List<Chunk>();
            foreach (var chunk in LoadedChunks)
            {
                if (foundChunks.GetChunk(chunk.ID) == null)
                {
                    removeChunks.Add(chunk);
                }
            }
            RemoveChunks(removeChunks);

            foreach (var chunk in LoadedChunks)
            {
                if (chunk.Contains(posX, posY))
                {
                    _current = chunk;
                    break;
                }
            }

            LoadedChunks.UpdateAll(this);
        }

        private void RemoveChunks(List<Chunk> chunks)
        {
            foreach (var chunk in chunks)
            {
                LoadedChunks.Remove(chunk);
                UnloadChunk(chunk);
            }
        }

        private GameObject CreateObject(string id, string name, Point loc, float z, GameObject parent)
        {
            GameObject obj = Instantiate(_prefabs[id], parent.transform);
            obj.transform.position = new Vector3(loc.X + 0.5f, loc.Y + 0.5f, z);
            obj.name = name;
            obj.SetActive(true);
            return obj;
        }

        private GameObject CreateObject(string id, Point loc, float z, GameObject parent) => CreateObject(id, string.Format("{0};{1},{2}", id, loc.X.ToString(), loc.Y.ToString()), loc, z, parent);

        /// <summary>
        /// Loads the chunk into GameObjects.
        /// </summary>
        /// <param name="chunk">The chunk to load.</param>
        private void LoadChunk(Chunk chunk)
        {
            var chunkObj = new GameObject("Chunk:" + chunk.IDStr);
            chunk.Object = Instantiate(chunkObj, transform);
            chunk.Object.name = chunk.Object.name.Substring(0, chunk.Object.name.Length - 7);

            if (TileSelected && chunk.Contains(SelectedTile))
            {
                GameObject selector = CreateObject("ui:selector", SelectedTile, -1.1f, chunk.Object);
                chunk.Objects.Add(selector);
            }

            for (int y = 0; y < chunk.Tiles.Count(false); y++)
            {
                for (int x = 0; x < chunk.Tiles.Count(true); x++)
                {
                    var tile = chunk.Tiles.Get(x, y);
                    var overlay = chunk.Overlays.Get(x, y);


                    GameObject obj = CreateObject(tile.Type.Name, tile.Location, 0, chunk.Object);
                    chunk.Objects.Add(obj);

                    if (overlay.Type != TileID.ENV_OVERLAY.VOID)
                    {
                        GameObject overlayObj = CreateObject(overlay.Type.Name, overlay.Location, -0.1f, chunk.Object);
                        chunk.Objects.Add(overlayObj);
                    }
                }
            }

            foreach (var entity in chunk.Entities)
            {
                GameObject entityObj = CreateObject(entity.Type.Name, entity.Location, -0.2f, chunk.Object);
                entity.Instance = entityObj.AddComponent<EntityManager>();
                entity.Instance.Entity = entity;

                chunk.Objects.Add(entityObj);
            }
            Destroy(chunkObj);
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
            Destroy(chunk.Object);
            chunk.Object = null;
        }
    }
}