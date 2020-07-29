//using IslandsOfRenguard.Scripts.Player;
//using IslandsOfRenguard.Scripts.Universal;
//using IslandsOfRenguard.Scripts.WorldGen;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Net.Http.Headers;
//using UnityEngine;

//namespace IslandsOfRenguard.Scripts.Frontend
//{
//    public class GridManager : MonoBehaviour
//    {
//        private int _rows = 13;
//        private int _cols = 25;
//        private float _tileSize = 1;
//        public List<List<GameObject>> Tiles { get; } = new List<List<GameObject>>();

//        // Start is called before the first frame update
//        void Start()
//        {
//            GenerateGrid();
//        }

//        // Update is called once per frame
//        void Update()
//        {

//        }

//        public void Regenerate(object sender, EventArgs e)
//        {
//            GenerateGrid();
//        }

//        private void GenerateGrid()
//        {
//            var systemObj = GameObject.FindWithTag("GameController");
//            if (systemObj == null)
//            {
//                Debug.LogError("Could not reference GameSystem");
//                return;
//            }
//            GameSystem system = systemObj.GetComponent<GameSystem>();

//            var playerObj = GameObject.FindWithTag("Player");
//            if (playerObj == null)
//            {
//                Debug.LogError("Could not find Player");
//                return;
//            }
//            PlayerManager player = playerObj.GetComponent<PlayerManager>();

//            World world = system.World; 

//            GameObject grassRef = (GameObject)Instantiate(Resources.Load("Tile_Env_Grass"));
//            GameObject stoneRef = (GameObject)Instantiate(Resources.Load("Tile_Env_Stone"));
//            GameObject waterRef = (GameObject)Instantiate(Resources.Load("Tile_Env_Water"));

//            //if (player.Location == null) player.Location = new Point((int)(world.GetSize().X / 2), (int)(world.GetSize().Y / 2));
//            if (player.Location == null) player.Location = new Point(0, 0);

//            int gridStartX = (int)(_cols / 2);
//            int gridStartY = (int)(_rows / 2);

//            int gridCenterX = (int)player.Location.X;
//            int gridCenterY = (int)player.Location.Y;

//            int xStart = gridCenterX - gridStartX;
//            int yStart = gridCenterY - gridStartY;

//            Tiles.Clear();
//            for (int y = 0; y < _rows; y++)
//            {
//                Tiles.Add(new List<GameObject>());
//                for (int x = 0; x < _cols; x++)
//                {
//                    var tile = world.GetTile(xStart + x, yStart + y);
//                    GameObject tileObj =
//                        tile.ID == TileID.ENV.STONE ? Instantiate(stoneRef, transform) :
//                        tile.ID == TileID.ENV.WATER ? Instantiate(waterRef, transform) :
//                        Instantiate(grassRef, transform);

//                    tileObj.name = string.Format("TILE_{0},{1}", x.ToString(), y.ToString());
//                    float posX = ((x - gridStartX) * _tileSize) + 0.5F;
//                    float posY = ((y - gridStartY) * -_tileSize) + 0.5F;
//                    tileObj.transform.position = new Vector2(posX, posY);
//                    Tiles[y].Add(tileObj);
//                }
//            }

//            Destroy(grassRef);
//            Destroy(stoneRef);
//            Destroy(waterRef);
//        }
//    }
//}