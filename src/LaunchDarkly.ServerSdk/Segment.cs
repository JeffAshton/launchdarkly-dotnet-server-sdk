using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LaunchDarkly.Client
{
    internal class Segment : IVersionedData
    {
        [JsonProperty(PropertyName = "key")]
        public string Key { get; }
        [JsonProperty(PropertyName = "version")]
        public int Version { get; set; }
        [JsonProperty(PropertyName = "included")]
        internal List<string> Included { get; }
        [JsonProperty(PropertyName = "excluded")]
        internal List<string> Excluded { get; }
        [JsonProperty(PropertyName = "salt")]
        internal string Salt { get; }
        [JsonProperty(PropertyName = "rules")]
        internal List<SegmentRule> Rules { get; }
        [JsonProperty(PropertyName = "deleted")]
        public bool Deleted { get; set; }

        [JsonConstructor]
        internal Segment(string key, int version, List<string> included, List<string> excluded,
                         string salt, List<SegmentRule> rules, bool deleted)
        {
            Key = key;
            Version = version;
            Included = included;
            Excluded = excluded;
            Salt = salt;
            Rules = rules;
            Deleted = deleted;
        }

        internal Segment()
        {
        }

        public bool MatchesUser(User user)
        {
            if (user.Key != null)
            {
                if (Included != null && Included.Contains(user.Key))
                {
                    return true;
                }
                if (Excluded != null && Excluded.Contains(user.Key))
                {
                    return false;
                }
                if (Rules != null)
                {
                    foreach (var rule in Rules)
                    {
                        if (rule.MatchesUser(user, this.Key, this.Salt))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}
