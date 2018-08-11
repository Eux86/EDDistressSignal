using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Interfaces
{
    public interface IDistressSignal
    {
        IPlayerInfo Player { get; set; }
        ILocation SignalLocation { get; set; }
        DateTime Time { get; set; }
        bool Answered { get; set; }
        IEnumerator<IPlayerInfo> PlayersAnswering { get; set; }
        Guid Id { get; set; }

    }
}
