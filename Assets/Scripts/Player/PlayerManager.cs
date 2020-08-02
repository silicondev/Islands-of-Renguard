using IslandsOfRenguard.Scripts.Universal;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandsOfRenguard.Scripts.Player
{
    public class PlayerManager : MonoBehaviour
    {
        public bool CanMove { get; set; } = true;
        //public Point Location { get; set; }
        public int ViewDis { get; private set; } = 20;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void CheckMove(object sender, EventArgs args)
        {
            KeyEventArgs e = (KeyEventArgs)args;
            if (CanMove)
            {
                switch (e.KeyPressed)
                {
                    case KeyCode.W:
                        Move(0, 1);
                        break;
                    case KeyCode.A:
                        Move(-1, 0);
                        break;
                    case KeyCode.S:
                        Move(0, -1);
                        break;
                    case KeyCode.D:
                        Move(1, 0);
                        break;
                }
            }
        }

        private void Move(float x, float y)
        {
            float newX = transform.position.x + x;
            float newY = transform.position.y + y;
            transform.position = new Vector2(newX, newY);
        }
    }
}
