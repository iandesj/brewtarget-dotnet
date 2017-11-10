namespace BrewTarget.Constants
{
    public static class PhysicalConstants
    {
        // brief Sucrose density in kg per L.
        public const double SucroseDensityKgL = 1.587;
        // brief This estimate for grain density is from my own (Philip G. Lee) experiments.
        public const double GrainDensityKgL = 0.963;
        // brief Liquid extract density in kg per L.
        public const double LiquidExtractDensityKgL = 1.412;
        // brief Dry extract density in kg per L.
        public const double DryExtractDensityKgL = SucroseDensityKgL;
        // brief How many liters of water get absorbed by 1 kg of grain.
        public const double GrainAbsorptionLkg = 1.085;
    }
}