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
        public Point Location { get; set; }

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
                        MoveY(-1);
                        break;
                    case KeyCode.A:
                        MoveX(-1);
                        break;
                    case KeyCode.S:
                        MoveY(1);
                        break;
                    case KeyCode.D:
                        MoveX(1);
                        break;
                }
            }
        }

        private void MoveY(float amount) => Location.Y += amount;

        private void MoveX(float amount) => Location.X += amount;
    }
}
