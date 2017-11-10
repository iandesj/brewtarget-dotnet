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
    }
}