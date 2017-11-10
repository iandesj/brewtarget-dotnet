/*!
 * \class Algorithms
 * \author Philip G. Lee
 * \author Eric Tamme
 *
 * \brief Beer-related math functions, arithmetic, and CS algorithms.
 */
namespace BrewTarget.IServices
{
    public interface IAlgorithmService
    {
        /*!
            * \brief Given dissolved sugar and wort volume, get SG in Plato
            * 
            * Estimates Plato from kg of dissolved sucrose (\c sugar_kg) and
            * the total wort volume \c wort_l.
            * 
            * \param sugar_kg kilograms of dissolved sucrose or equivalent
            * \param wort_l liters of wort
            */
        double GetPlato(double sugar_kg, double wort_l);
        //! \brief Converts FG to plato, given the OG.
        double OgFgToPlato(double og, double fg);
        //! \brief Gets ABV by using current gravity reading and brix reading.
        double GetABVBySGPlato(double sg, double plato);
        //! \brief Gets ABW from current gravity and plato.
        double GetABWBySGPlato(double sg, double plato);
        //! \brief Gives you the SG from the starting plato and current plato.
        double SgByStartingPlato(double startingPlato, double currentPlato);
        //! \brief Returns the refractive index from plato.
        double RefractiveIndex(double plato);
        //! \brief Corrects the apparent extract 'plato' to the real extract using current gravity 'sg'.
        double RealExtract(double sg, double plato);

        /*
        private:
        // This is the cubic fit to get Plato from specific gravity, measured at 20C
        // relative to density of water at 20C.
        // P = -616.868 + 1111.14(SG) - 630.272(SG)^2 + 135.997(SG)^3
        static Polynomial platoFromSG_20C20C;
        
        // Water density polynomial, given in kg/L as a function of degrees C.
        // 1.80544064e-8*x^3 - 6.268385468e-6*x^2 + 3.113930471e-5*x + 0.999924134
        static Polynomial waterDensityPoly_C;
        
        // Polynomial in degrees Celsius that gives the additive hydrometer
        // correction for a 15C hydrometer when read at a temperature other
        // than 15C.
        static Polynomial hydroCorrection15CPoly;

        // Hide constructors and assignment op.
        Algorithms(){}
        Algorithms(Algorithms const&){}
        Algorithms& operator=(Algorithms const& other){ return *this; }
        ~Algorithms(){}
        */
    }
}