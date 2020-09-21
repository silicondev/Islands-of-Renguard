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
        public GenericEntity Entity { get; set; }
        public SpriteRenderer Renderer;

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
            Renderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            transform.position = new Vector3(Entity.Location.X, Entity.Location.Y, -0.2F);
        }
    }
}
