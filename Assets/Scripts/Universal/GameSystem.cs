using IslandsOfRenguard.Scripts.Frontend;
using IslandsOfRenguard.Scripts.Player;
using IslandsOfRenguard.Scripts.WorldGen;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandsOfRenguard.Scripts.Universal
{
    public class GameSystem : MonoBehaviour
    {
        public World World;
        private GameSystem _instance;
        public GameSystem Instance { get { return _instance;  } }

        private PlayerManager _player;
        private GridManager _grid;

        void Awake()
        {
            _instance = this;
        }

        void OnDestroy()
        {
            _instance = null;
        }

        // Start is called before the first frame update
        void Start()
        {
            World = new Generator(new GeneratorSettings(1000, 1000, 0.05F), new WorldMapperSettings(80, 100, 200)).Generate();

            var gridObj = GameObject.FindWithTag("Grid");
            if (gridObj != null)
                _grid = gridObj.GetComponent<GridManager>();
            else
                Debug.LogError("Could not find Grid Object");

            var playerObj = GameObject.FindWithTag("Player");
            if (gridObj != null)
                _player = playerObj.GetComponent<PlayerManager>();
            else
                Debug.LogError("Could not find Player Object");

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
            _grid.Regenerate(sender, e);
        }
    }
}
