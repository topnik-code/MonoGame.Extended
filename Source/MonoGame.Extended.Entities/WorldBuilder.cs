using System;
using System.Collections.Generic;
using MonoGame.Extended.Entities.Systems;

namespace MonoGame.Extended.Entities
{
    public class WorldBuilder
    {
        private readonly List<Func<World, ISystem>> _createSystemFuncs = new List<Func<World, ISystem>>();

        public WorldBuilder AddSystem(ISystem system)
        {
            _createSystemFuncs.Add(w => system);
            return this;
        }

        public WorldBuilder AddSystem(Func<World, ISystem> onBuild)
        {
            _createSystemFuncs.Add(onBuild);
            return this;
        }


        public World Build()
        {
            var world = new World();

            foreach (var createSystem in _createSystemFuncs)
            {
                var system = createSystem(world);
                world.RegisterSystem(system);
            }

            return world;
        }
    }
}