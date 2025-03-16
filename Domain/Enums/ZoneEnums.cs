using System.Text.Json.Serialization;

namespace Domain.Enums
{
    public class ZoneEnums
    {
        public enum ZoneMembershipType
        {
            Teacher = 1,
            Student = 2
        }

        public enum RelyInvite
        {
            Accept = 1,
            Reject = 2
        }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum MemberQueryType
        {
            Member = 1,
            Pending = 2,
        }

    }
}
