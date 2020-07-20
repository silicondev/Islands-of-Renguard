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

        // Start is called before the first frame update
        void Start()
        {
            InputEvents input = GetComponent<InputEvents>();
            input.OnKeyPressed += CheckMove;
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void CheckMove(object sender, EventArgs args)
        {
            KeyEventArgs e = (KeyEventArgs)args;
            if (CanMove)
            {
                switch (e.KeyPressed)
                {
                    case KeyCode.W:
                        MoveY(1);
                        break;
                    case KeyCode.A:
                        MoveX(-1);
                        break;
                    case KeyCode.S:
                        MoveY(-1);
                        break;
                    case KeyCode.D:
                        MoveX(1);
                        break;
                }
            }
        }

        private void MoveY(float amount)
        {
            float Y = transform.position.y + amount;
            float currentX = transform.position.x;
            transform.position = new Vector2(currentX, Y);
        }

        private void MoveX(float amount)
        {
            float X = transform.position.x + amount;
            float currentY = transform.position.y;
            transform.position = new Vector2(X, currentY);
        }
    }
}
