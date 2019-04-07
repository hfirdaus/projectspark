using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlanetName
{
	Mercury,
	Venus,
	Earth,
	Mars,
	Jupiter,
	Saturn,
	Uranus,
	Neptune
}

public static class PlanetInfo
{
	public static Dictionary<PlanetName, string> Info = new Dictionary<PlanetName, string>()
	{
		{ PlanetName.Mercury,
			"Mercury is the closest planet to the Sun and also the smallest planet in our solar system. It has a rocky surface with no moons." +
			" Despite being closest to the Sun it is not actually the hottest planet because of its thin atmosphere. " +
			"Mercury has very short years and incredibly long days. The time it takes to orbit around the Sun is only 88 Earth days, " +
			"where as a single “day” on the planet, the time it takes to rotate around once, is actually 59 Earth days!" },
		{ PlanetName.Venus,
			"Venus is the second planet away from the Sun and the hottest planet in our solar system due to its dense atmosphere. One day on " +
			"Venus takes 243 Earth days because it spins incredibly slowly! It also spins backwards, which means the Sun would rise in the " +
			"West and set in the East. Venus is full of volcanic mountains and ridged plateaus. Over 40 spacecraft have visited Venus’ surface " +
			"and it is even believed to have once had water. We can’t get too excited about a new planet to call home though, as the surface " +
			"temperature 465 C! That’s hot enough to melt lead." },
		{ PlanetName.Earth,
			"Earth is the third planet from the Sun and the place we call home! It is the only place that we know supports life and the only " +
			"planet that we have confirmed has water on its surface. While all the other planets are named after Greek and Roman Gods, Earth alone " +
			"is named after a Germanic word meaning “ground”. Because of our atmosphere we are protected from the dangerous radiation in " +
			"space as well as any meteors that get broken up before they reach the Earth’s surface." },
		{ PlanetName.Mars,
			"Mars is commonly referred to as the Red Planet because of the iron minerals in its soil, and is the fourth planet in our solar system. " +
			"It is a cold desert world that is suspected to have once held a thicker atmosphere and perhaps even water. It is the only planet we have" +
			" sent rovers to and the next destination in Man’s journey of space exploration! NASA has announced plans to send men to Mars possibly as early as 2033." },
		{ PlanetName.Jupiter,
			"Jupiter is the largest planet in the solar system. The fifth planet from the Sun it is the first of the gas giants. This means it has a " +
			"solid inner core of the planet but lacks an Earth like surface. Jupiter is home to the Great Red Spot, a giant storm larger than the " +
			"Earth that has raged for over a century. While Jupiter itself could not support life due to its atmosphere of Hydrogen and Helium, " +
			"it is also home to over 75 moons. Some of these moons have oceans beneath their crust, which might support life." },
		{ PlanetName.Saturn,
			"Saturn is famous in our solar system because of its set of dazzling rings made of ice and rock, which can even be seen from Earth on " +
			"a clear night using a telescope. The 6th planet from the Sun, it is a gas giant made of mostly Hydrogen and Helium. " +
			"It cannot support life as we know it but like Jupiter, some of its many moons might. In 2017, the Cassini spacecraft ended its mission by " +
			"intentionally vaporizing into Saturn’s atmosphere." },
		{ PlanetName.Uranus,
			"Uranus is the 7th planet from the Sun and an Ice Giant. It is made out of a hot, dense fluid of icy materials like water, ammonia " +
			"and methane. The methane is what makes it blue! It also has 13 rings, but unlike Saturn, they are a lot smaller and not as prominent. " +
			"It is similar to Venus in that it rotates backwards, but interestingly, this planet rotates on its side! Voyager 2 is the only spacecraft " +
			"to fly by Uranus and as of yet, no spacecraft has orbited the planet itself." },
		{ PlanetName.Neptune,
			"Neptune is the 8th and last planet in our solar system. It is an Ice Giant and the only planet not visible by the naked eye from Earth. " +
			"Similar to Neptune, it is made of fluid icy materials like water, ammonia, and methane over a rocky core. Currently, no spacecraft has " +
			"orbited the planet but Voyager 2 has flown past it. Because of the elliptical orbit of the dwarf planet Pluto, Neptune is sometimes " +
			"further away from the Sun and Earth than Pluto!" }
	};

	public static Dictionary<PlanetName, bool> IsCollected = new Dictionary<PlanetName, bool>()
	{
		{ PlanetName.Mercury, false },
		{ PlanetName.Venus,   false },
		{ PlanetName.Earth,   false },
		{ PlanetName.Mars,    false },
		{ PlanetName.Jupiter, false },
		{ PlanetName.Saturn,  false },
		{ PlanetName.Uranus,  false },
		{ PlanetName.Neptune, false }
	};

	public static Dictionary<PlanetName, Color> Colors = new Dictionary<PlanetName, Color>()
	{
		{ PlanetName.Mercury, Color.HSVToRGB(18f/360f, 0.41f, 0.9f) },
		{ PlanetName.Venus,   Color.HSVToRGB(346f/360f, 0.39f, 0.92f) },
		{ PlanetName.Earth,   Color.HSVToRGB(145f/360f, 0.78f, 0.79f) },
		{ PlanetName.Mars,    Color.HSVToRGB(0f, 0.63f, 0.92f) },
		{ PlanetName.Jupiter, Color.HSVToRGB(28f/360f, 0.69f, 0.95f) },
		{ PlanetName.Saturn,  Color.HSVToRGB(54f/360f, 0.69f, 0.95f) },
		{ PlanetName.Uranus,  Color.HSVToRGB(195f/360f, 0.64f, 0.95f) },
		{ PlanetName.Neptune, Color.HSVToRGB(241f/360f, 0.58f, 0.93f) }
	};
}
