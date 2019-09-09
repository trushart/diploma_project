using System;

namespace FIT.Diploma.Shared.DataAccess.Dto
{
    [Serializable]
    public class LeagueInfoDto
    {
        public int LeagueId { get; set; }

        public string LeagueName { get; set; }

        public string LeagueInfo { get; set; }

        public string Description { get; set; }

        public string Location { get; set; }

        public bool Equals(LeagueDto other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return LeagueId == other.LeagueId && LeagueName.Equals(other.LeagueName)
                && LeagueInfo.Equals(other.LeagueInfo) && Location.Equals(other.Location);
        }
    }
}
