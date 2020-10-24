using dEvine_and_conquer.Base;
using dEvine_and_conquer.Entity;
using dEvine_and_conquer.Object;
using dEvine_and_conquer.World;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using Random = UnityEngine.Random;

namespace dEvine_and_conquer.Scripts
{
    public class GameSystem : MonoBehaviour
    {
        public GameSystem Instance { get; private set; }

        public static Dictionary<string, GameObject> Prefabs;
        private static GameObject _prefabObj;

        public static PlayerManager Player;
        public static Generator Generator;
        public static WorldMapperSettings Mapper;

        public static ChunkContainer Chunks;
        public static EntityContainer Entities;

        public static bool TileSelected { get; set; } = false;
        public static Point SelectedTile { get; private set; }

        private static Chunk _current;

        public Block CurrentBlock => Chunks.GetLoaded().GetBlockFromID(Player.Location);
        public static Block StartBlock = null;

        private bool _loopDebug = false;
        public static bool DebugMode = true;

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

            Prefabs = new Dictionary<string, GameObject>() {
                { "tile:grass", (GameObject)Instantiate(Resources.Load("Prefabs/Tile/Tile_Env_Grass"), _prefabObj.transform) },
                { "tile:stone", (GameObject)Instantiate(Resources.Load("Prefabs/Tile/Tile_Env_Stone"), _prefabObj.transform) },
                { "tile:water", (GameObject)Instantiate(Resources.Load("Prefabs/Tile/Tile_Env_Water"), _prefabObj.transform) },
                { "tile:sand", (GameObject)Instantiate(Resources.Load("Prefabs/Tile/Tile_Env_Sand"), _prefabObj.transform) },

                { "overlay:tree", (GameObject)Instantiate(Resources.Load("Prefabs/Overlay/Overlay_Env_Tree"), _prefabObj.transform) },

                { "entity:human.friendly.male", (GameObject)Instantiate(Resources.Load("Prefabs/Entity/Human/Entity_Human_Male"), _prefabObj.transform) },

                { "ui:selector", (GameObject)Instantiate(Resources.Load("Prefabs/UI/UI_Selector"), _prefabObj.transform) }
            };

            foreach (var obj in Prefabs)
            {
                obj.Value.transform.position = new Vector3(0, 0, 10);
                if (_loopDebug)
                {
                    var renderer = obj.Value.GetComponent<SpriteRenderer>();
                    var clr = renderer.color;
                    renderer.color = new Color(clr.r, clr.g, clr.b, 0.2f);
                }
                obj.Value.SetActive(false);
                obj.Value.name = obj.Key + ";PREFAB";
            }

            Entities = new EntityContainer(transform);
            Chunks = new ChunkContainer(transform);

            var seed = Random.Range(0.0F, 10000000.0F);
            Mapper = new WorldMapperSettings(80, 100, 105, 200);
            Generator = new Generator(0.01F, seed, 16, new WorldMapper(Mapper));
            DevLogger.Log($"Seed: {seed}");

            var playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
            {
                Player = playerObj.GetComponent<PlayerManager>();
                Player.Location = (0, 0);
            }
            else
                DevLogger.Log("Could not find Player Object");

            StartupGenerate();

            InputEvents input = GetComponent<InputEvents>();
            input.OnMovementKeyPressed += Player.OnMove;
            input.OnMovementKeyPressed += RefreshScreenEvent;
            input.OnScroll += Player.OnScroll;
            input.OnScroll += RefreshScreenEvent;
            input.OnKeyPressed += OnKeyPress;
            input.OnMouseClick += OnClick;
        }

        // Update is called once per frame
        void Update()
        {
            Chunks.GetLoaded().UpdateAll();
            Entities.GetLoaded().UpdateAll();
            if (_loopDebug) ForceRegen(true);
        }

        private void RefreshScreenEvent(object sender, EventArgs e)
        {
            var w = Screen.width;
            var h = Screen.height;
            var sev = new Vector3(0, 0, 0);
            var nev = new Vector3(0, h, 0);
            var swv = new Vector3(w, 0, 0);
            var nwv = new Vector3(w, h, 0);

            var se = GetBlockFromScreen(sev);
            var ne = GetBlockFromScreen(nev);
            var sw = GetBlockFromScreen(swv);
            var nw = GetBlockFromScreen(nwv);

            if (se == null ||
                ne == null ||
                sw == null ||
                nw == null)
            {
                RegenEntities();
                RegenChunks();
            }
        }

        private void OnKeyPress(object sender, EventArgs e)
        {
            KeyEventArgs args = (KeyEventArgs)e;
            if (args.KeyPressed == KeyCode.G)
            {
                if (TileSelected)
                {
                    DevLogger.Log("Entity Human Added!");
                    var entity = new HumanEntity(SelectedTile, 1);
                    entity.OnDestinationReach += OnEntityReach;
                    Entities.Generate(entity);
                    RegenEntities();
                } else
                    DevLogger.Log("ERROR: Could not add entity to world.");
            }
            else if (args.KeyPressed == KeyCode.T && TileSelected)
            {
                var entity = Entities.GetLoaded()[Random.Range(0, Entities.LoadedCount())];
                _ = entity.GoTo(SelectedTile);
            } else if (args.KeyPressed == KeyCode.H && TileSelected)
            {
                var block = Chunks.GetLoaded().GetBlockFromID(SelectedTile);
                var items = Harvest(block);
                foreach (var item in items)
                {
                    DevLogger.Log($"Harvested {item.Id.Name} x {item.Amount} from {block.Overlay.Type.Name}");
                }
            } else if (args.KeyPressed == KeyCode.Q)
            {
                ForceRegen(true);
            } else if (args.KeyPressed == KeyCode.Escape)
            {
                TileSelected = false;
                ForceRegen(false);
            }
        }

        private void OnClick(object sender, EventArgs e)
        {
            ClickEventArgs args = (ClickEventArgs)e;
            if (args.Button == MouseButton.LEFT)
            {
                var block = GetBlockFromScreen(args.Position);
                if (block == null)
                    DevLogger.Log("ERROR: Could not find object.");
                else
                {
                    DevLogger.Log($"Tile: {block.Tile.Type.Name}. Overlay: {block.Overlay.Type.Name}");

                    SelectedTile = block.Location;
                    TileSelected = true;
                    ForceRegen(false);
                }
            }
        }

        private void OnEntityReach(object sender, EventArgs e)
        {
            GenericEntity entity = (GenericEntity)sender;
            TargetReachArgs args = (TargetReachArgs)e;
            var items = Harvest(args.Destination);
            foreach (var item in items)
            {
                DevLogger.Log($"Harvested {item.Id.Name} x {item.Amount} to {entity.Type.Name}");
            }
            entity.Inventory.AddItems(items);
        }

        private List<ItemBag> Harvest(Point tileLoc)
        {
            var block = Chunks.GetLoaded().GetBlockFromID(tileLoc);
            return Harvest(block);
        }

        private List<ItemBag> Harvest(Block block)
        {
            var overlay = block.Overlay;
            var items = overlay.Harvest();
            overlay.Type = ObjectID.ENV.VOID;
            ForceRegen(false);
            return items;
        }

        private static Block GetBlockFromScreen(Vector3 position)
        {
            var flat = ScreenToWorld(position);
            var chunk = Chunks.GetLoaded().GetContains(flat);
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
            return (vPos.x, vPos.y);
        }

        private void StartupGenerate()
        {
            RegenEntities();
            RegenChunks();
            StartBlock = CurrentBlock;
        }

        private void ForceRegen(bool incEntities = true)
        {
            Chunks.UnloadAll();
            if (incEntities) RegenEntities();
            RegenChunks();
        }

        /// <summary>
        /// Either generates or reloads chunks coming into view and unloads chunks going out of view.
        /// </summary>
        private void RegenChunks()
        {
            Timer.Start("RegenChunks");
            var pos = Player.Location.Flatten();

            var bottomLeft = ScreenToWorld(new Vector3(0, 0, 0));
            var topRight = ScreenToWorld(new Vector3(Screen.width, Screen.height, 0));

            Point buffer = (Player.ViewBuffer, Player.ViewBuffer);

            bool hasNewChunks = false;
            List<Chunk> foundChunks = new List<Chunk>();
            for (int y = (bottomLeft.Y - buffer.Y).Floor(); y < (topRight.Y + buffer.Y).Floor(); y++)
            {
                for (int x = (bottomLeft.X.Floor() - buffer.X).Floor(); x < (topRight.X.Floor() + buffer.X).Floor(); x++)
                {
                    // If chunk is already loaded, then skip this loop
                    if (!foundChunks.Contains(x, y))
                    {
                        var loaded = Chunks.GetLoaded().GetChunkWithPosition(x, y);
                        if (loaded != null)
                        {
                            foundChunks.Add(loaded);
                            continue;
                        }
                    } else
                        continue;

                    // Find chunk if exists
                    Point id = ((x / 16f).Floor(), (y / 16f).Floor());
                    var newChunk = Chunks.GetGenerated().GetChunk(id) ?? new Chunk(id, Generator);

                    // Chunk does not already exist, generate a new one.
                    if (!newChunk.IsGenerated)
                    {
                        hasNewChunks = true;
                        Chunks.Generate(newChunk);
                    }

                    // Load chunk
                    Chunks.Load(newChunk, transform);
                }
            }

            if (hasNewChunks) 
                DevLogger.Log("Current Generated Chunks: " + Chunks.GeneratedCount());

            // Unload unneeded chunks
            List<Chunk> removeChunks = new List<Chunk>();
            foreach (var chunk in Chunks.GetLoaded())
            {
                if (foundChunks.GetChunk(chunk.ID) == null)
                {
                    removeChunks.Add(chunk);
                }
            }
            Chunks.UnloadAll(removeChunks);

            _current = Chunks.GetLoaded().GetContains(pos);
            Chunks.GetLoaded().UpdateAll();
            Timer.StopAndLog("RegenChunks");
        }

        private void RegenEntities()
        {
            Entities.UnloadAll();
            List<GenericEntity> existingEntities = new List<GenericEntity>(Entities.GetGenerated());
            List<GenericEntity> toAdd = new List<GenericEntity>();
            foreach (var chunk in Chunks.GetLoaded())
            {
                List<GenericEntity> edit = new List<GenericEntity>(existingEntities);
                foreach (var entity in edit)
                {
                    if (chunk.Contains(entity.Location))
                    {
                        toAdd.Add(entity);
                        existingEntities.Remove(entity);
                    }
                }
            }
            Entities.LoadAll(toAdd);
        }
    }
}