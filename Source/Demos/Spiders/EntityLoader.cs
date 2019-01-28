using System;
using System.IO;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.TextureAtlases;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Spiders
{
    public class TextureAtlasJsonConveter : JsonConverter<TextureAtlas>
    {
        private readonly ITextureAtlasResolver _resolver;

        public TextureAtlasJsonConveter(ITextureAtlasResolver resolver)
        {
            _resolver = resolver;
        }

        public override void WriteJson(JsonWriter writer, TextureAtlas textureAtlas, JsonSerializer serializer)
        {
            writer.WriteValue(textureAtlas.Name);
        }

        public override TextureAtlas ReadJson(JsonReader reader, Type objectType, TextureAtlas existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var name = reader.Value.ToString();
            return _resolver.Resolve(name);
        }
    }

    public interface ITextureAtlasResolver
    {
        TextureAtlas Resolve(string name);
    }

    public class EntityLoader : ITextureRegionService, ITextureAtlasResolver
    {
        private readonly World _world;
        private readonly ContentManager _contentManager;
        private readonly string _rootPath;

        public EntityLoader(World world, ContentManager contentManager, string rootPath)
        {
            _world = world;
            _contentManager = contentManager;
            _rootPath = rootPath;
        }

        private string GetEntityFilePath(string name) => $"{Path.Combine(_contentManager.RootDirectory, _rootPath, name)}.json";

        public Entity Load(string name)
        {
            var filePath = GetEntityFilePath(name);
            var serializer = new JsonSerializer
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters =
                {
                    new TextureRegion2DJsonConverter(this),
                    new TextureAtlasJsonConveter(this),
                    new EntityJsonConveter(_world),
                    new Vector2JsonConverter(),
                    new ColorJsonConverter()
                }
            };

            using (var streamReader = new StreamReader(filePath))
            using (var jsonReader = new JsonTextReader(streamReader))
            {
                return serializer.Deserialize<Entity>(jsonReader);
            }
        }

        public TextureRegion2D GetTextureRegion(string name)
        {
            var texture = _contentManager.Load<Texture2D>(name);
            return new TextureRegion2D(texture);
        }

        public TextureAtlas Resolve(string name)
        {
            var texture = _contentManager.Load<Texture2D>(name);
            return TextureAtlas.Create(name, texture, 32, 32);
        }
    }
}