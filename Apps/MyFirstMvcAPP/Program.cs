﻿using BattleCards.Controllers;
using MySUS.HTTP;
using MySUS.MvcFramework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BattleCards
{
    class Program
    {
        static async Task Main(string[] args)
        {                       
            await Host.CreateHostAsync(new Startup(), 80);
        }
    }
}
