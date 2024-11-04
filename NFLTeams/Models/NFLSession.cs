namespace NFLTeams.Models
{
    public class NFLSession
    {
        private const string TeamsKey = "myteams";
        private const string CountKey = "teamcount";
        private const string ConfKey = "conf";
        private const string DivKey = "div";
        private const string NameKey = "name";  // Added NameKey

        private ISession session { get; set; }

        public NFLSession(ISession session) => this.session = session;

        public void SetMyTeams(List<Team> teams)
        {
            session.SetObject(TeamsKey, teams);
            session.SetInt32(CountKey, teams.Count);
        }

        public List<Team> GetMyTeams() =>
            session.GetObject<List<Team>>(TeamsKey) ?? new List<Team>();

        public int? GetMyTeamCount() => session.GetInt32(CountKey);

        public void SetActiveConf(string activeConf) =>
            session.SetString(ConfKey, activeConf);

        public string GetActiveConf() =>
            session.GetString(ConfKey) ?? string.Empty;

        public void SetActiveDiv(string activeDiv) =>
            session.SetString(DivKey, activeDiv);

        public string GetActiveDiv() =>
            session.GetString(DivKey) ?? string.Empty;

        public void RemoveMyTeams()
        {
            session.Remove(TeamsKey);
            session.Remove(CountKey);
        }

        // New methods for NameKey
        public void SetName(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                session.Remove(NameKey);
            }
            else
            {
                session.SetString(NameKey, userName);
            }
        }

        public string GetName() =>
            session.GetString(NameKey) ?? string.Empty;
    }
}
