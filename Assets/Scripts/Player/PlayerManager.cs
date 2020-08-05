using IslandsOfRenguard.Scripts.Universal;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandsOfRenguard.Scripts.Player
{
    public class PlayerManager : MonoBehaviour
    {
        private float _defaultZoom = 7F;
        private float _defaultMoveSpeed = 0.2F;
        private int _viewDisModX = 18;
        private int _viewDisModY = 9;


        public bool CanMove { get; set; } = true;
        public Point ViewDis { 
            get 
            {
                var zoomMod = (CurrentZoom / _defaultZoom);
                return new Point((int)(_viewDisModX * zoomMod), (int)(_viewDisModY * zoomMod));
            }        
        }
        public float MoveSpeed { 
            get
            {
                return (_defaultMoveSpeed / _defaultZoom) * CurrentZoom;
            }
        }
        public float ZoomSpeed { get; private set; } = 0.7F;
        public float SmoothSpeed { get; private set; } = 10.0F;
        public float MinZoom { get; private set; } = 1.0F;
        public float MaxZoom { get; private set; } = 15.0F;
        public float TargetZoom { get; private set; }
        public float CurrentZoom => Camera.main.orthographicSize;

        // Start is called before the first frame update
        void Start()
        {
            TargetZoom = _defaultZoom;
        }

        // Update is called once per frame
        void Update()
        {
            Camera.main.orthographicSize = Mathf.MoveTowards(Camera.main.orthographicSize, TargetZoom, SmoothSpeed * Time.deltaTime);
        }

        public void OnMove(object sender, EventArgs args)
        {
            KeyEventArgs e = (KeyEventArgs)args;
            if (CanMove)
            {
                switch (e.KeyPressed)
                {
                    case KeyCode.W:
                        Move(0, MoveSpeed);
                        break;
                    case KeyCode.A:
                        Move(MoveSpeed / -1, 0);
                        break;
                    case KeyCode.S:
                        Move(0, MoveSpeed / -1);
                        break;
                    case KeyCode.D:
                        Move(MoveSpeed, 0);
                        break;
                }
            }
        }

        public void OnScroll(object sender, EventArgs args)
        {
            ScrollEventArgs e = (ScrollEventArgs)args;

            TargetZoom -= e.IsUp ? ZoomSpeed : ZoomSpeed / -1;
            TargetZoom = Mathf.Clamp(TargetZoom, MinZoom, MaxZoom);
        }

        private void Move(float x, float y)
        {
            float newX = transform.position.x + x;
            float newY = transform.position.y + y;
            transform.position = new Vector2(newX, newY);
        }
    }
}
