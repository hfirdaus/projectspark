using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class SolarSystemPlanet : Planet
{
    private bool isCollected = true;

    public void SetCollected(bool collected)
    {
        if (isCollected != collected)
        {
            isCollected = collected;
            SetGhost(!collected);
        }
    }

}
