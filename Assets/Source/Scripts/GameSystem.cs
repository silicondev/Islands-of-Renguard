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
                //var renderer = obj.Value.GetComponent<SpriteRenderer>();
                //var clr = renderer.color;
                //renderer.color = new Color(clr.r, clr.g, clr.b, 0.2f);
                obj.Value.SetActive(false);
                obj.Value.name = obj.Key + ";PREFAB";
            }

            Entities = new EntityContainer(transform);
            Chunks = new ChunkContainer(transform);

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
            Chunks.GetLoaded().UpdateAll();
            Entities.GetLoaded().UpdateAll();
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
                    Entities.Generate(new HumanEntity(SelectedTile, 1));
                    RegenEntities();
                } else
                    Debug.Log("ERROR: Could not add entity to world.");
            }
            else if (args.KeyPressed == KeyCode.H && TileSelected)
            {
                var entity = Entities.GetLoaded()[Random.Range(0, Entities.LoadedCount() - 1)];
                entity.GoTo(SelectedTile);
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
            return new Point(vPos.x, vPos.y);
        }

        private void StartupGenerate()
        {
            RegenChunks();
            RegenEntities();
            StartBlock = CurrentBlock;
        }

        private void ForceRegen(bool incEntities = true)
        {
            Chunks.UnloadAll();
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
                        var loaded = Chunks.GetLoaded().GetChunkWithPosition(x, y);
                        if (loaded != null)
                        {
                            foundChunks.Add(loaded);
                            continue;
                        }
                    } else
                        continue;

                    // Find chunk if exists
                    var id = new Point((int)Math.Floor(x / 16d), (int)Math.Floor(y / 16d));
                    //var newChunk = Chunks.GetGenerated().GetChunk(id) ?? new Chunk(id, Generator);
                    Chunk newChunk;
                    if (Chunks.GetGenerated().GetChunk(id) != null)
                    {
                        newChunk = new Chunk(id, Generator, Chunks.GetGenerated().GetChunk(id).Blocks);
                        Debug.Log(string.Format("Found generated Chunk ID {0} with {1} blocks.", newChunk.IDStr, newChunk.Blocks.Length.ToString()));
                    } else
                    {
                        newChunk = new Chunk(id, Generator);
                        hasNewChunks = true;
                        Chunks.Generate(newChunk);
                    }

                    // Chunk does not already exist, generate a new one.
                    //if (!newChunk.IsGenerated)
                    //{
                    //    hasNewChunks = true;
                    //    Chunks.Generate(newChunk);
                    //}

                    // Load chunk
                    Chunks.Load(newChunk, transform);
                }
            }

            if (hasNewChunks) 
                Debug.Log("Current Generated Chunks: " + Chunks.GeneratedCount());

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

            _current = Chunks.GetLoaded().GetContains(new Point(posX, posY));
            Chunks.GetLoaded().UpdateAll();
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