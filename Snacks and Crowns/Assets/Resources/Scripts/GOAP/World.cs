using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GOAP
{
    public sealed class World
    {
        private static readonly World instance = new World();
        private static WorldState worldState;
        static World()
        {
            worldState = new WorldState();
        }
        private World()
        {

        }
        public static World Instance
        {
            get { return instance; }
        }
        public WorldState GetWorld()
        {
            return worldState;
        }
    }
}
