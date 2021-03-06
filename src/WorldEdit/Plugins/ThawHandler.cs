﻿using System.Collections.Generic;

namespace WorldEdit
{
    public class ThawHandler : ChatHandler
    {
        public ThawHandler()
        {
            ChatCommand = "thaw";
            ChatCommandDescription = "Thaw...";
        }

        public override void HandleMessage(IEnumerable<string> args)
        {
            Command("fill ~-15 ~-15 ~-15 ~15 ~15 ~15 water 0 replace ice");
        }
    }
}