using System;
using System.Collections.Generic;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Sprites;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Spiders
{
    public class EntityJsonConveter : JsonConverter<Entity>
    {
        private readonly World _world;
        private readonly Dictionary<string, Type> _typeLookup = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase)
        {
            { "sprite", typeof(Sprite) },
            { "transform", typeof(Transform2) }
        };

        public EntityJsonConveter(World world)
        {
            _world = world;
        }

        public override void WriteJson(JsonWriter writer, Entity entity, JsonSerializer serializer)
        {
        }

        public override Entity ReadJson(JsonReader reader, Type objectType, Entity existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var entity = _world.CreateEntity();
            var jObject = JObject.Load(reader);

            foreach (var keyValuePair in jObject)
            {
                if (_typeLookup.TryGetValue(keyValuePair.Key, out var componentType))
                {
                    var component = keyValuePair.Value.ToObject(componentType, serializer);
                    var attachMethod = typeof(Entity)
                        .GetMethod(nameof(Entity.Attach))
                        .MakeGenericMethod(componentType);
                    attachMethod.Invoke(entity, new[] { component });
                }
            }

            return entity;
        }
    }
}