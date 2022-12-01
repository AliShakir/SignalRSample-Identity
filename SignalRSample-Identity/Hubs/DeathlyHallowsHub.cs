using Microsoft.AspNetCore.SignalR;

namespace SignalRSample_Identity.Hubs
{
    public class DeathlyHallowsHub : Hub
    {
        public Dictionary<string, int> GetRaceStatus()
        {
            return SD.DeathlyHallowRace;
        }
    }
}
