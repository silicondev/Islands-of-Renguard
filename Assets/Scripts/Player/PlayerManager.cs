using IslandsOfRenguard.Scripts.Universal;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
            Debug.Log(e.KeyPressed.ToString());
            if (CanMove)
            {
                switch (e.KeyPressed)
                {
                    case KeyCode.W:
                        //MoveY(-1);
                        Move(0, -1);
                        break;
                    case KeyCode.A:
                        //MoveX(-1);
                        Move(-1, 0);
                        break;
                    case KeyCode.S:
                        //MoveY(1);
                        Move(0, 1);
                        break;
                    case KeyCode.D:
                        //MoveX(1);
                        Move(1, 0);
                        break;
                }
            }
        }

        //private void MoveY(float amount) => Location.Y += amount;

        //private void MoveX(float amount) => Location.X += amount;

        private void Move(float x, float y)
        {
            float newX = transform.position.x + x;
            float newY = transform.position.y + y;
            transform.position = new Vector2(newX, newY);
        }
    }
}
