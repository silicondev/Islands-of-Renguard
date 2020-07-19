using IslandsOfRenguard.Scripts.Universal;
using IslandsOfRenguard.Scripts.WorldGen;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

namespace IslandsOfRenguard.Scripts.Frontend
{
    public class GridManager : MonoBehaviour
    {
        private int _rows = 13;
        private int _cols = 25;
        private float _tileSize = 1;
        private World world;
        public List<List<GameObject>> Tiles { get; } = new List<List<GameObject>>();

        // Start is called before the first frame update
        void Start()
        {
            GenerateGrid();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void GenerateGrid()
        {
            //Debug.Log(Mathf.PerlinNoise(2.1F, 0.1F));

            //return;
            world = new Generator(new GeneratorSettings(1000, 1000, 1), new WorldMapperSettings(0, 0, 300)).Generate();

            GameObject grassRef = (GameObject)Instantiate(Resources.Load("Tile_Env_Grass"));
            GameObject stoneRef = (GameObject)Instantiate(Resources.Load("Tile_Env_Stone"));
            GameObject waterRef = (GameObject)Instantiate(Resources.Load("Tile_Env_Water"));

            int gridStartX = (int)(_cols / 2);
            int gridStartY = (int)(_rows / 2);

            int gridCenterX = (int)(world.GetSize().X / 2);
            int gridCenterY = (int)(world.GetSize().Y / 2);

            int xStart = gridCenterX - gridStartX;
            int yStart = gridCenterY - gridStartY;

            for (int y = 0; y < _rows; y++)
            {
                Tiles.Add(new List<GameObject>());
                for (int x = 0; x < _cols; x++)
                {
                    var tile = world.GetTile(xStart + x, yStart + y);
                    GameObject tileObj =
                        tile == Tile.ENV.STONE ? Instantiate(stoneRef, transform) :
                        tile == Tile.ENV.WATER ? Instantiate(waterRef, transform) :
                        Instantiate(grassRef, transform);

                    tileObj.name = string.Format("TILE_{0},{1}", x.ToString(), y.ToString());
                    float posX = (x - gridStartX) * _tileSize;
                    float posY = (y - gridStartY) * -_tileSize;
                    tileObj.transform.position = new Vector2(posX, posY);
                    Tiles[y].Add(tileObj);
                }
            }

            Debug.Log((Tiles.Count * Tiles[0].Count).ToString());

            Destroy(grassRef);
            Destroy(stoneRef);
            Destroy(waterRef);
        }
    }
}