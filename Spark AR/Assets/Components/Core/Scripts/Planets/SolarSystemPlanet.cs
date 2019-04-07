using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class SolarSystemPlanet : Planet
{
    public bool isCollected = false;

    public void SetCollected(bool collected)
    {
        isCollected = collected;
        SetGhost(!collected);
    }
}
