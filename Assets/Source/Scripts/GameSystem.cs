using dEvine_and_conquer.Base;
using dEvine_and_conquer.Entity;
using dEvine_and_conquer.World;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using Random = UnityEngine.Random;

namespace dEvine_and_conquer.Scripts
{
    public class GameSystem : MonoBehaviour
    {
        public GameSystem Instance { get; private set; }

        private static Dictionary<string, GameObject> _prefabs;
        private static GameObject _prefabObj;

        public static PlayerManager Player;
        public static Generator Generator;
        public static WorldMapperSettings Mapper;

        public static List<Chunk> GeneratedChunks = new List<Chunk>();
        public static List<Chunk> LoadedChunks = new List<Chunk>();
        public static List<GenericEntity> GeneratedEntities = new List<GenericEntity>();
        public static List<GenericEntity> LoadedEntities = new List<GenericEntity>();

        public static bool TileSelected { get; set; } = false;
        public static Point SelectedTile { get; private set; }

        private static Chunk _current;

        public Block CurrentBlock => LoadedChunks.GetBlockFromID(Player.Location);
        public static Block StartBlock = null;
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

                { "entity:human.friendly.male", (GameObject)Instantiate(Resources.Load("Prefabs/Entity/Human/Entity_Human_Male"), _prefabObj.transform) },

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
            LoadedChunks.UpdateAll();
            LoadedEntities.UpdateAll();
        }

        /// <summary>
        /// Event calls every time the player moves.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMovement(object sender, EventArgs e)
        {
            Player.OnMove(sender, e);

            var sev = new Vector3(0, 0, 0);
            var nev = new Vector3(0, Screen.height, 0);
            var swv = new Vector3(Screen.width, 0, 0);
            var nwv = new Vector3(Screen.width, Screen.height, 0);

            var se = GetBlockFromScreen(sev);
            var ne = GetBlockFromScreen(nev);
            var sw = GetBlockFromScreen(swv);
            var nw = GetBlockFromScreen(nwv);

            if (se == null ||
                ne == null ||
                sw == null ||
                nw == null)
            {
                RegenChunks();
                RegenEntities();
            }
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
                if (TileSelected)
                {
                    Debug.Log("Entity Human Added!");
                    GeneratedEntities.Add(new HumanEntity(SelectedTile, 1));
                    RegenEntities();
                } else
                    Debug.Log("ERROR: Could not add entity to world.");
            }
            else if (args.KeyPressed == KeyCode.H && TileSelected)
            {
                var entity = LoadedEntities[Random.Range(0, LoadedEntities.Count - 1)];
                entity.GoTo(SelectedTile);
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
                var block = GetBlockFromScreen(args.Position);
                if (block == null)
                    Debug.Log("ERROR: Could not find object.");
                else
                {
                    Debug.Log(string.Format("Tile: {0}. Overlay: {1}", block.Tile.Type.Name, block.Overlay.Type.Name));

                    SelectedTile = block.Location;
                    TileSelected = true;
                    ForceRegen(false);
                }
            }
        }

        private static Block GetBlockFromScreen(Vector3 position)
        {
            var flat = ScreenToWorld(position);
            var chunk = LoadedChunks.GetContains(flat);
            if (chunk == null)
                return null;
            var x = flat.X.Floor();
            var y = flat.Y.Floor();
            var block = chunk.Blocks.Get(x, y);
            return block;
        }

        private static Point ScreenToWorld(Vector3 position)
        {
            var vPos = Camera.main.ScreenToWorldPoint(position);
            return new Point(vPos.x, vPos.y);
        }

        private void StartupGenerate()
        {
            RegenChunks();
            RegenEntities();
            StartBlock = CurrentBlock;
        }

        //public void CreateEntity(GenericEntity entity)
        //{
        //    LoadedChunks.GetChunkWithPosition(entity.Location)?.Entities.Add(entity);
        //}

        private void ForceRegen(bool incEntities = true)
        {
            List<Chunk> remove = new List<Chunk>(LoadedChunks);
            RemoveChunks(remove);
            RegenChunks();
            if (incEntities) RegenEntities();
        }

        /// <summary>
        /// Either generates or reloads chunks coming into view and unloads chunks going out of view.
        /// </summary>
        private void RegenChunks()
        {
            int posX = (int)Player.Location.X;
            int posY = (int)Player.Location.Y;

            var bottomLeft = ScreenToWorld(new Vector3(0, 0, 0));
            var topRight = ScreenToWorld(new Vector3(Screen.width, Screen.height, 0));

            bool hasNewChunks = false;
            List<Chunk> foundChunks = new List<Chunk>();
            for (int y = (bottomLeft.Y - Player.ViewDis.Y).Floor(); y < (topRight.Y + Player.ViewDis.Y).Floor(); y++)
            {
                for (int x = (bottomLeft.X.Floor() - Player.ViewDis.X).Floor(); x < (topRight.X.Floor() + Player.ViewDis.X).Floor(); x++)
                {
                    // If chunk is already loaded, then skip this loop
                    if (!foundChunks.Contains(x, y))
                    {
                        var loaded = LoadedChunks.GetChunkWithPosition(x, y);
                        if (loaded != null)
                        {
                            foundChunks.Add(loaded);
                            continue;
                        }
                    } else
                        continue;

                    // Find chunk if exists
                    var id = new Point((int)Math.Floor(x / 16d), (int)Math.Floor(y / 16d));
                    var newChunk = GeneratedChunks.GetChunk(id) ?? new Chunk(id, Generator, this);

                    // Chunk does not already exist, generate a new one.
                    if (!newChunk.IsGenerated)
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

            _current = LoadedChunks.GetContains(new Point(posX, posY));
            LoadedChunks.UpdateAll();
        }

        private void RegenEntities()
        {
            RemoveEntities(new List<GenericEntity>(LoadedEntities));
            List<GenericEntity> addEntities = new List<GenericEntity>(GeneratedEntities);
            foreach (var chunk in LoadedChunks)
            {
                List<GenericEntity> edit = new List<GenericEntity>(addEntities);
                foreach (var entity in edit)
                {
                    if (chunk.Contains(entity.Location))
                    {
                        LoadedEntities.Add(entity);
                        addEntities.Remove(entity);
                    }
                }
            }
            LoadEntities();
        }

        private void RemoveChunks(List<Chunk> chunks)
        {
            foreach (var chunk in chunks)
            {
                LoadedChunks.Remove(chunk);
                UnloadChunk(chunk);
            }
        }

        private void RemoveEntities(List<GenericEntity> entities)
        {
            foreach (var entity in entities)
            {
                LoadedEntities.Remove(entity);
                UnloadEntity(entity);
            }
        }

        private GameObject CreateObject(string id, string name, Point loc, float z, Transform parent)
        {
            GameObject obj = Instantiate(_prefabs[id], parent);
            obj.transform.position = new Vector3(loc.X + 0.5f, loc.Y + 0.5f, z);
            obj.name = name;
            obj.SetActive(true);
            return obj;
        }

        private GameObject CreateObject(string id, Point loc, float z, Transform parent) => CreateObject(id, string.Format("{0};{1},{2}", id, loc.X.ToString(), loc.Y.ToString()), loc, z, parent);

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
                GameObject selector = CreateObject("ui:selector", SelectedTile, -1.1f, chunk.Object.transform);
                chunk.Objects.Add(selector);
            }

            foreach (var block in chunk.Blocks)
            {
                GameObject obj = CreateObject(block.Tile.Type.IdName, block.Location, 0, chunk.Object.transform);
                chunk.Objects.Add(obj);

                if (block.Overlay.Type != ObjectID.ENV.VOID)
                {
                    GameObject overlayObj = CreateObject(block.Overlay.Type.IdName, block.Location, -0.1f, chunk.Object.transform);
                    chunk.Objects.Add(overlayObj);
                }
            }
            
            Destroy(chunkObj);
        }

        private void LoadEntity(GenericEntity entity)
        {
            GameObject entityObj = CreateObject(entity.Type.IdName, entity.Location, -0.2f, transform);
            
            entity.Instance = entityObj.AddComponent<EntityManager>();
            entity.Object = entityObj;
            entity.Instance.Entity = entity;
        }

        private void LoadEntities()
        {
            Debug.Log(string.Format("Loading all {0} entities.", LoadedEntities.Count().ToString()));
            foreach (var entity in LoadedEntities)
            {
                LoadEntity(entity);
            }
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

        private void UnloadEntity(GenericEntity entity)
        {
            Destroy(entity.Object);
        }
    }
}