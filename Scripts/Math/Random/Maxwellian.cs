// From: http://www.dreamincode.net/forums/topic/162675-random-number/ by rsturley

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RandomNumbers
{
	/*
	 * Generates random numbers with a Maxwell-Boltzmann distribution.
	 * http://www.dreamincode.net/forums/topic/162675-random-number/ by rsturley
	 * 
	 */
	public class Maxwellian
	{
		Random rand;
		
		public Maxwellian()
		{
			rand = new Random();
		}
		
		/// <summary>
		/// return a random number with a Maxwellian distribution of the
		/// form (E^(-(x^2/(2 \[Sigma]^2))) Sqrt[2/\[Pi]] x^2)/\[Sigma]^3
		/// </summary>
		/// <param name="sigma">distribtuion width form the formula above</param>
		/// <returns>random nubmer with Maxwellian distribution</returns>
		public double Next(double sigma){
			return sigma * inverseCDF(rand.NextDouble());
		}
		
		double inverseCDF(double s)
		{
			double x;
			if (s < 0.346691)
			{
				x = 1.0642622768277956e-6 * Math.Sqrt(2.1347980205189531e12 * Math.Pow(s, 0.6666666666666666) +
				                                      1.0323831697580514e12 * Math.Pow(s, 1.3333333333333333) + 6.775644406955992e11 * s * s +
				                                      5.0331929839908344e11 * Math.Pow(s, 2.6666666666666665) +
				                                      3.9996925883820276e11 * Math.Pow(s, 3.3333333333333335) + 3.3163359755658606e11 * Math.Pow(s, 4) +
				                                      2.831279191020564e11 * Math.Pow(s, 4.666666666666667));
			}
			else if (s < 0.796567)
			{
				x = 0.014527571050476862 + s * (9.294742731011485 +
				                                s * (-37.43519439668487 + s * (105.40885039276776 +
				                               s * (-183.6190579712269 + s * (196.7067172374017 +
				                               s * (-118.99569035897035 + 31.66450688674963 * s))))));
			}
			else
			{
				x = (-665.6950297578719 + s * (13652.127578664917 +
				                               s * (-47641.68123439248 + s * (73592.91391855325 +
				                               s * (-60775.51067786972 + (27088.54652502034 - 5250.64365989075 * s) * s))))) /
					(2004.625993588326 + s * (-5790.900684890041 + (5572.531218092137 - 1786.242621106215 * s) * s));
			}
			return x;
		}
	}
}
