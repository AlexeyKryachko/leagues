﻿using Interfaces.Games.BuisnessLogic.Models;

namespace Interfaces.Games.BuisnessLogic
{
    public interface IGamesManager
    {
        void Create(string leagueId, GameVewModel model);
    }
}
