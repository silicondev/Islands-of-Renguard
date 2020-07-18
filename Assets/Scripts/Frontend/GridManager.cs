using IslandsOfRenguard.Scripts.WorldGen;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandsOfRenguard.Scripts.Frontend
{
    public class GridManager : MonoBehaviour
    {
        private int _rows = 13;
        private int _cols = 25;
        private float _tileSize = 1;
        private World world;

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

        }
    }
}