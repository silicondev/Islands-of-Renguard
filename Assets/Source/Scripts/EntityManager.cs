using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using dEvine_and_conquer.Entity;

namespace dEvine_and_conquer.Scripts
{
    public class EntityManager : MonoBehaviour
    {
        public EntityManager Instance { get; private set; }
        public GenericEntity Entity { get; }
        private int _animationTime = 200;

        public EntityManager(GenericEntity entity)
        {
            Entity = entity;
        }

        private void Awake()
        {
            Instance = this;
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        private void Start()
        {

        }

        private int timer = 0;
        private void Update()
        {
            timer++;
            if (timer >= _animationTime)
            {
                UpdateAnimation();
                timer = 0;
            }
        }

        private int currentFrame = 0;
        private void UpdateAnimation()
        {
            currentFrame++;
            if (currentFrame >= Entity.Textures.Count) currentFrame = 0;
            GetComponent<Renderer>().material.SetTexture("_MainTex", Entity.Textures[currentFrame]);
        }
    }
}
