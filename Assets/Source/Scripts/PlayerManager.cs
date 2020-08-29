using dEvine_and_conquer.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace dEvine_and_conquer.Scripts
{
    public class PlayerManager : MonoBehaviour
    {
        private float _defaultZoom = 7F;
        private float _defaultMoveSpeed = 0.2F;
        private int _viewDisModX = 18;
        private int _viewDisModY = 18;
        private int _viewBuffer = 5;

        public bool CanMove { get; set; } = true;
        public Point ViewDis { 
            get 
            {
                var zoomMod = (CurrentZoom / _defaultZoom);
                return new Point((int)(_viewDisModX * zoomMod) + _viewBuffer, (int)(_viewDisModY * zoomMod) + _viewBuffer);
            }
        }
        public float MoveSpeed => (_defaultMoveSpeed / _defaultZoom) * CurrentZoom;
        public float ZoomSpeed { get; private set; } = 0.7F;
        public float SmoothSpeed { get; private set; } = 10.0F;
        public float MinZoom { get; private set; } = 1.0F;
        public float MaxZoom { get; private set; } = 10.0F;
        public float TargetZoom { get; private set; }
        public float CurrentZoom => Camera.main.orthographicSize;
        public Point Location => new Point((int)Math.Floor(transform.position.x), (int)Math.Floor(transform.position.y));

        private void MoveUp() => Move(MoveSpeed, MoveSpeed);
        private void MoveRight() => Move(MoveSpeed, MoveSpeed / -1);
        private void MoveLeft() => Move(MoveSpeed / -1, MoveSpeed);
        private void MoveDown() => Move(MoveSpeed / -1, MoveSpeed / -1);

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
                        MoveUp();
                        break;
                    case KeyCode.A:
                        MoveLeft();
                        break;
                    case KeyCode.S:
                        MoveDown();
                        break;
                    case KeyCode.D:
                        MoveRight();
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

        private void Move(float x, float y) => transform.position = new Vector3(transform.position.x + x, transform.position.y + y, -1.5F);
    }
}
