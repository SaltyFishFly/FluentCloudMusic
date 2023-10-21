using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace FluentCloudMusic.Utils
{
    [AttributeUsage(AttributeTargets.Property)]
    public class JsonMultipleProperty : Attribute
    {
        public string PreferredName { get; }

        public string[] FallbackNames { get; }

        public JsonMultipleProperty(string preferredProperty, params string[] fallbackProperties)
        {
            PreferredName = preferredProperty;
            FallbackNames = fallbackProperties;
        }
    }

    public class JsonMultiplePropertyContractResolver : CamelCasePropertyNamesContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var members = GetSerializableMembers(type);
            var properties = new List<JsonProperty>();
            foreach (var member in members)
            {
                var preferred = CreateProperty(member, memberSerialization);
                properties.Add(preferred);

                var attribute = member.GetCustomAttribute<JsonMultipleProperty>();
                if (attribute == null) continue;

                preferred.PropertyName = attribute.PreferredName;
                foreach (var name in attribute.FallbackNames)
                {
                    var fallback = CreateProperty(member, memberSerialization);
                    fallback.PropertyName = name;
                    properties.Add(fallback);
                }
            }
            return properties;
        }
    }
}
