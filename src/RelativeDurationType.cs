namespace Tavenem.Time
{
    /// <summary>
    /// Indicates the type of time measurement used by a <see cref="RelativeDuration"/> instance.
    /// </summary>
    public enum RelativeDurationType
    {
        /// <summary>
        /// Time is measured absolutely.
        /// </summary>
        Absolute = 0,

        /// <summary>
        /// Time is measured as the proportion of one local day.
        /// </summary>
        ProportionOfDay = 1,

        /// <summary>
        /// Time is measured as the proportion of one local year.
        /// </summary>
        ProportionOfYear = 2,
    }
}
