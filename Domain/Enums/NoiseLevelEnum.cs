namespace Domain.Enums
{
    public enum NoiseLevelEnum
    {
        /// <summary>
        /// Unknown or not applicable
        /// </summary>
        NA,

        /// <summary>
        /// Minimal or low-level noise
        /// </summary>
        Quiet,

        /// <summary>
        /// Noticeable daytime noise, quiet at night
        /// </summary>
        Moderate,

        /// <summary>
        /// Regular street or neighbor noise
        /// </summary>
        Busy,

        /// <summary>
        /// High traffic, nightlife, or construction nearby
        /// </summary>
        Loud
    }
}
