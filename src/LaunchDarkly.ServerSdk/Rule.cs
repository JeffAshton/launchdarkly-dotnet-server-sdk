using System.Collections.Generic;
using Newtonsoft.Json;

namespace LaunchDarkly.Client
{
    internal class Rule : VariationOrRollout
    {
        [JsonProperty(PropertyName = "id")]
        internal string Id { get; }

        [JsonProperty(PropertyName = "clauses")]
        internal List<Clause> Clauses { get; }

        [JsonProperty(PropertyName = "trackEvents")]
        internal bool TrackEvents { get; }

        [JsonConstructor]
        internal Rule(string id, int? variation, Rollout rollout, List<Clause> clauses, bool trackEvents) : base(variation, rollout)
        {
            Id = id;
            Clauses = clauses;
            TrackEvents = trackEvents;
        }

        internal bool MatchesUser(User user, IFeatureStore store)
        {
            foreach (var c in Clauses)
            {
                if (!c.MatchesUser(user, store))
                {
                    return false;
                }
            }
            return true;
        }
    }
}