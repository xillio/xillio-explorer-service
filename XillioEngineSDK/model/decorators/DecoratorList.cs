using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace XillioEngineSDK.model.decorators
{
    public class DecoratorList
    {
        [JsonProperty("container")] public ContainerDecorator ContainerDecorator { get; set; }

        [JsonProperty("contentType")] public ContentTypeDecorator ContentTypeDecorator { get; set; }

        [JsonProperty("created")] public CreatedDecorator CreatedDecorator { get; set; }

        [JsonProperty("description")] public DescriptionDecorator DescriptionDecorator { get; set; }

        [JsonProperty("file")] public FileDecorator FileDecorator { get; set; }

        [JsonProperty("fileSystem")] public FileSystemDecorator FileSystemDecorator { get; set; }

        [JsonProperty("hash")] public HashDecorator HashDecorator { get; set; }

        [JsonProperty("language")] public LanguageDecorator LanguageDecorator { get; set; }

        [JsonProperty("mimeType")] public MimeTypeDecorator MimeTypeDecorator { get; set; }

        [JsonProperty("modified")] public ModifiedDecorator ModifiedDecorator { get; set; }

        [JsonProperty("name")] public NameDecorator NameDecorator { get; set; }

        [JsonProperty("parent")] public ParentDecorator ParentDecorator { get; set; }

        [JsonProperty("preview")] public PreviewDecorator PreviewDecorator { get; set; }

        [JsonProperty("version")] public VersionDecorator VersionDecorator { get; set; }
    }
}