using System;
using System.Collections.Generic;
using ProjetCombat;

public interface IManaUser
{
    int CurrentMana { get; set; }
    int MaxMana { get; set; }
}
