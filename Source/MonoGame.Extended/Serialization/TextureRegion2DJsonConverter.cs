using System;
using MonoGame.Extended.TextureAtlases;
using Newtonsoft.Json;

namespace MonoGame.Extended.Serialization
{
    public class TextureRegion2DJsonConverter : JsonConverter<TextureRegion2D>
    {
        private readonly ITextureRegionService _textureRegionService;

        public TextureRegion2DJsonConverter(ITextureRegionService textureRegionService)
        {
            _textureRegionService = textureRegionService;
        }

        public override void WriteJson(JsonWriter writer, TextureRegion2D region, JsonSerializer serializer)
        {
            writer.WriteValue(region.Name);
        }

        public override TextureRegion2D ReadJson(JsonReader reader, Type objectType, TextureRegion2D existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.Value is string regionName)
                return _textureRegionService.GetTextureRegion(regionName);

            return null;
        }
    }
}