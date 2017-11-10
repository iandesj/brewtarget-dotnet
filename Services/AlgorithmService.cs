using System;
using BrewTarget.Constants;
using BrewTarget.Models;

namespace BrewTarget.Services
{
    public class AlgorithmService
    {
        // This is the cubic fit to get Plato from specific gravity, measured at 20C
        // relative to density of water at 20C.
        // P = -616.868 + 1111.14(SG) - 630.272(SG)^2 + 135.997(SG)^3
        private static Polynomial platoFromSG_20C20C = new Polynomial().Push(-616.868).Push(1111.14).Push(-630.272).Push(135.997);

        // Water density polynomial, given in kg/L as a function of degrees C.
        // 1.80544064e-8*x^3 - 6.268385468e-6*x^2 + 3.113930471e-5*x + 0.999924134
        private static Polynomial waterDensityPoly_C = new Polynomial().Push(0.9999776532).Push(6.557692037e-5).Push(-1.007534371e-5).Push(1.372076106e-7).Push(-1.414581892e-9).Push(5.6890971e-12);

        // Polynomial in degrees Celsius that gives the additive hydrometer
        // correction for a 15C hydrometer when read at a temperature other
        // than 15C.
        private static Polynomial hydroCorrection15CPoly = new Polynomial().Push(-0.911045).Push(-16.2853e-3).Push(5.84346e-3).Push(-15.3243e-6);

        public AlgorithmService() { }

        //! \brief Cross-platform rounding.
        static double Round(double d)
        {
            return Math.Floor(d + 0.5);
        }

        //===================Beer-related stuff=====================

        //! \returns plato of \b sg
        static double SG_20C20C_toPlato(double sg)
        {
            return platoFromSG_20C20C.Eval(sg);
        }
        //! \returns sg of \b plato
        static double PlatoToSG_20C20C(double plato)
        {
            var poly = new Polynomial(platoFromSG_20C20C);
            poly._coeffs[0] -= plato;
            return poly.RootFind(1.000, 1.050);
        }
        //! \returns water density in kg/L at temperature \b celsius
        static double GetWaterDensity_kgL(double celsius)
        {
            return waterDensityPoly_C.Eval(celsius);
        }
        //! \returns additive correction to the 15C hydrometer reading if read at \b celsius
        static double Hydrometer15CCorrection(double celsius)
        {
            return hydroCorrection15CPoly.Eval(celsius) * 1e-3;
        }
        public double GetPlato(double sugar_kg, double wort_l)
        {
            var water_kg = wort_l - sugar_kg / PhysicalConstants.SucroseDensityKgL; // Assumes sucrose vol and water vol add to wort vol.

            return sugar_kg / (sugar_kg + water_kg) * 100.0;
        }
        //! \brief Converts FG to plato, given the OG.
        public double OgFgToPlato(double og, double fg)
        {
            var sp = SG_20C20C_toPlato(og);

            var poly = new Polynomial()
                .Push(1.001843)
                .Push(0.002318474 * sp - 0.000007775 * sp * sp - 0.000000034 * sp * sp * sp - fg)
                .Push(0.00574)
                .Push(0.00003344)
                .Push(0.000000086);

            return poly.RootFind(3, 5);
        }
        //! \brief Gets ABV by using current gravity reading and brix reading.
        public double GetABVBySGPlato(double sg, double plato)
        {
            // Implements the method found at:
            // http://www.byo.com/stories/projects-and-equipment/article/indices/29-equipment/1343-refractometers
            // ABV = [277.8851 - 277.4(SG) + 0.9956(Brix) + 0.00523(Brix2) + 0.000013(Brix3)] x (SG/0.79)

            return (277.8851 - 277.4 * sg + 0.9956 * plato + 0.00523 * plato * plato + 0.000013 * plato * plato * plato) * (sg / 0.79);
        }
        //! \brief Gets ABW from current gravity and plato.
        public double GetABWBySGPlato(double sg, double plato)
        {
            // Implements the method found at:
            // http://primetab.com/formulas.html

            var ri = RefractiveIndex(plato);
            return 1017.5596 - 277.4 * sg + ri * (937.8135 * ri - 1805.1228);
        }
        //! \brief Gives you the SG from the starting plato and current plato.
        public double SgByStartingPlato(double startingPlato, double currentPlato)
        {
            // Implements the method found at:
            // http://primetab.com/formulas.html

            var sp2 = startingPlato * startingPlato;
            var sp3 = sp2 * startingPlato;

            var cp2 = currentPlato * currentPlato;
            var cp3 = cp2 * currentPlato;

            return 1.001843 - 0.002318474 * startingPlato - 0.000007775 * sp2 - 0.000000034 * sp3
                    + 0.00574 * currentPlato + 0.00003344 * cp2 + 0.000000086 * cp3;
        }
        //! \brief Returns the refractive index from plato.
        public double RefractiveIndex(double plato)
        {
            // Implements the method found at:
            // http://primetab.com/formulas.html
            return 1.33302 + 0.001427193 * plato + 0.000005791157 * plato * plato;
        }
        //! \brief Corrects the apparent extract 'plato' to the real extract using current gravity 'sg'.
        public double RealExtract(double sg, double plato)
        {
            var ri = RefractiveIndex(plato);
            return 194.5935 + 129.8 * sg + ri * (410.8815 * ri - 790.8732);
        }
    }
}