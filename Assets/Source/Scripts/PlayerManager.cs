using dEvine_and_conquer.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace dEvine_and_conquer.Scripts
{
    public class PlayerManager : Script
    {
        private float _defaultZoom = 10F;
        private float _defaultMoveSpeed = 0.2F;
        private int _viewDisModX = 1;
        private int _viewDisModY = 1;
        public readonly int ViewBuffer = 5;

        public bool CanMove { get; set; } = true;
        public Point ViewDis { 
            get 
            {
                var zoomMod = CurrentZoom / _defaultZoom;
                return (((_viewDisModX * zoomMod) + ViewBuffer).Floor(), ((_viewDisModY * zoomMod) + ViewBuffer).Floor());
            }
        }
        public float MoveSpeed => (_defaultMoveSpeed / (_defaultZoom * 2)) * (CurrentZoom * 2);
        public float ZoomSpeed { get; private set; } = 0.7F;
        public float SmoothSpeed { get; private set; } = 10.0F;
        public float MinZoom { get; private set; } = 5.0F;
        public float MaxZoom { get; private set; } = 20.0F;
        public float TargetZoom { get; private set; }
        public float CurrentZoom
        {
            get
            {
                return Camera.main.orthographicSize;
            }
            set
            {
                Camera.main.orthographicSize = value;
            }
        }
        public Point Location { get; set; }

        private void MoveUp() => Location.Move(0, MoveSpeed);
        private void MoveRight() => Location.Move(MoveSpeed, 0);
        private void MoveLeft() => Location.Move(MoveSpeed / -1, 0);
        private void MoveDown() => Location.Move(0, MoveSpeed / -1);

        private void Start()
        {
            TargetZoom = _defaultZoom;
            Camera.main.orthographicSize = _defaultZoom;
        }

        private void Update()
        {
            if (CurrentZoom != TargetZoom) CurrentZoom = Mathf.MoveTowards(CurrentZoom, TargetZoom, SmoothSpeed * Time.deltaTime);
            Move(Location);
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
    }
}
