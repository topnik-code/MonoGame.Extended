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
    public class EntityLoader : ITextureRegionService
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
                    new EntityJsonConveter(_world),
                    new Vector2JsonConverter()
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
    }
}