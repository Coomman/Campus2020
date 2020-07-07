using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using thegame.GameObjects;

namespace thegame.Services
{
    public static class GamesRepo
    {
        public static GameCoordinator Op;
        public static GameBoard Board;
    }
}