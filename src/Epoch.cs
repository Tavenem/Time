namespace Tavenem.Time;

/// <summary>
/// An epoch is a unit of time with a (positive) <see cref="Duration"/> and an optional <see
/// cref="Name"/>.
/// </summary>
/// <param name="Duration">
/// <para>
/// The duration of the epoch.
/// </para>
/// <para>
/// Must be positive and nonzero.
/// </para>
/// </param>
/// <param name="Name">
/// The name of the epoch. May be <see langword="null"/>.
/// </param>
public readonly record struct Epoch(Duration Duration, string? Name = null)
{
    /// <summary>
    /// The epochs which make up our universe.
    /// </summary>
    /// <remarks>
    /// Although the Holocene is still officially defined as the current geologic epoch,
    /// indications are strong that the Anthropocene will be formally adopted in the near
    /// future, with a start date set in the mid 20th century. In order to keep <see
    /// cref="CosmicTime"/> instances as simple as possible, this library anticipates that
    /// decision so that current times can be referenced to that point (rather than to the start
    /// of the Holocene, which would require every time to have magnitudes in the tens of
    /// thousands of years).
    /// </remarks>
    public static readonly Epoch[] DefaultEpochs =
    [
        new Epoch(Duration.OnePlanckTime, "Planck Epoch"),
        new Epoch(new Duration(planckTime: 185486000), "Grand unification Epoch"),
        new Epoch(new Duration(planckTime: 1854680000000), "Inflationary Epoch"),
        new Epoch(new Duration(yoctoseconds: Duration.YoctosecondsPerPicosecond), "Electroweak Epoch"),
        new Epoch(new Duration(nanoseconds: 9999, yoctoseconds: Duration.YoctosecondsPerPicosecond * 990), "Quark Epoch"),
        new Epoch(new Duration(nanoseconds: 999990000), "Hadron Epoch"),
        new Epoch(new Duration(nanoseconds: (ulong)Duration.NanosecondsPerSecond * 9), "Lepton Epoch"),
        new Epoch(new Duration(years: 370000), "Photon Epoch"),
        new Epoch(new Duration(years: 299630000), "Cosmic Dark Ages"),
        new Epoch(new Duration(years: 700370000), "Reionization"),
        new Epoch(new Duration(aeons: 8, years: 532430000), "Prior to the formation of Earth"),
        new Epoch(new Duration(years: 536300000), "Hadean Eon"),
        new Epoch(new Duration(aeons: 1, years: 531000000), "Archean Eon"),
        new Epoch(new Duration(aeons: 1, years: 961200000), "Proterozoic Eon"),
        new Epoch(new Duration(years: 53400000), "Cambrian Period"),
        new Epoch(new Duration(years: 41600000), "Ordovician Period"),
        new Epoch(new Duration(years: 24600000), "Silurian Period"),
        new Epoch(new Duration(years: 60300000), "Devonian Period"),
        new Epoch(new Duration(years: 60000000), "Carboniferous Period"),
        new Epoch(new Duration(years: 47000000), "Permian Period"),
        new Epoch(new Duration(years: 50500000), "Triassic Period"),
        new Epoch(new Duration(years: 56300000), "Jurassic Period"),
        new Epoch(new Duration(years: 79000000), "Cretaceous Period"),
        new Epoch(new Duration(years: 42970000), "Paleogene Period"),
        new Epoch(new Duration(years: 17697000), "Miocene Epoch"),
        new Epoch(new Duration(years: 2753000), "Pliocene Epoch"),
        new Epoch(new Duration(years: 2568350), "Pleistocene Epoch"),
        new Epoch(new Duration(years: 11576), "Holocene Epoch"),
    ];

    /// <summary>
    /// The duration of this epoch.
    /// </summary>
    public Duration Duration { get; } = Duration.IsNegative
        || Duration.IsZero
        ? throw new ArgumentException($"{nameof(Duration)} must be positive and non-zero", nameof(Duration))
        : Duration;
}
