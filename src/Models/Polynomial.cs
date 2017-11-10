/*
 * \brief Class to encapsulate real polynomials in a single variable
 * \author Ian DesJardins
 */
using System;
using System.Collections.Generic;
using System.Linq;

namespace BrewTarget.Models
{
    public class Polynomial
    {
        private const double ROOT_PRECISION = 0.0000001;

        public List<double> _coeffs { get; set; }

        //! \brief Get the polynomial's order (highest exponent)
        public int order { get { return _coeffs.Count() - 1; } }

        //! \brief Default constructor
        public Polynomial()
        {
            _coeffs = new List<double>();
        }

        //! \brief Copy constructor
        public Polynomial(Polynomial other)
        {
            _coeffs = new List<double>(other._coeffs);
        }

        //! \brief Constructs the 0 polynomial with given \c order
        public Polynomial(int order)
        {
            _coeffs = new List<double>(Enumerable.Repeat(0.0, order + 1));
        }

        //! \brief Constructor from an array of coefficients
        public Polynomial(double[] coeffs)
        {
            _coeffs = new List<double>(coeffs);
        }

        //! \brief Add a coefficient for x^(\c order() + 1)
        public Polynomial Push(double coeff)
        {
            _coeffs.Add(coeff);
            return this;
        }

        //! \brief Get coefficient of x^n where \c n <= \c order()
        public double Get(int n)
        {
            return _coeffs[n];
        }

        //! \brief Evaluate the polynomial at point \c x
        public double Eval(double x)
        {
            double ret = 0.0;
            int i;

            for (i = order; i > 0; --i)
            {
                ret += _coeffs[i] * IntPow(x, (uint)i);
            }
            ret += _coeffs[0];

            return ret;
        }

        /*!
            * \brief Root-finding by the secant method.
            * 
            * \param x0 - one of two initial \b distinct guesses at the root
            * \param x1 - one of two initial \b distinct guesses at the root
            * \returns \c HUGE_VAL on failure, otherwise a root of the polynomial
            */
        public double RootFind(double x0, double x1)
        {
            var guesses = new double[] { x0, x1 };
            var newGuess = x0;
            var maxAllowableSeparation = Math.Abs(x0 - x1) * 1e3;

            while (Math.Abs(guesses[0] - guesses[1]) > ROOT_PRECISION)
            {
                newGuess = guesses[1] - (guesses[1] - guesses[0]) * Eval(guesses[1]) / (Eval(guesses[1]) - Eval(guesses[0]));

                guesses[0] = guesses[1];
                guesses[1] = newGuess;

                if (Math.Abs(guesses[0] - guesses[1]) > maxAllowableSeparation)
                {
                    return double.MaxValue;
                }
            }

            return newGuess;
        }

        //! \brief returns baseN^pow
        static double IntPow(double baseN, uint pow)
        {
            double ret = 1;
            for (; pow > 0; pow--)
            {
                ret *= baseN;
            }

            return ret;
        }
    }
}