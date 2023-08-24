using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace FluentCloudMusic.Utils
{
    public static class JsonUtils
    {
        public static readonly JsonSerializer Serializer = new JsonSerializer()
        {
            ContractResolver = new MultipleJsonPropertyContractResolver()
        };
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class MultipleJsonProperty : Attribute
    {
        public string PreferredName { get; }

        public string[] FallbackReadNames { get; }

        public MultipleJsonProperty(string preferredProperty, params string[] fallbackProperties)
        {
            PreferredName = preferredProperty;
            FallbackReadNames = fallbackProperties;
        }
    }

    public class MultipleJsonPropertyContractResolver : CamelCasePropertyNamesContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var members = GetSerializableMembers(type);
            var properties = new List<JsonProperty>();
            foreach (var member in members)
            {
                var property = CreateProperty(member, memberSerialization);
                properties.Add(property);

                var fallbackAttribute = member.GetCustomAttribute<MultipleJsonProperty>();
                if (fallbackAttribute == null) continue;

                property.PropertyName = fallbackAttribute.PreferredName;
                foreach (var alternateName in fallbackAttribute.FallbackReadNames)
                {
                    var fallbackProperty = CreateProperty(member, memberSerialization);
                    fallbackProperty.PropertyName = alternateName;
                    properties.Add(fallbackProperty);
                }
            }
            return properties;
        }
    }
}
