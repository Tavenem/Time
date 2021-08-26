namespace Tavenem.Time;

/// <summary>
/// An epoch is a unit of time with a (positive) <see cref="Duration"/> and an optional <see
/// cref="Name"/>.
/// </summary>
public readonly struct Epoch
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
    public static readonly Epoch[] DefaultEpochs = new Epoch[]
    {
        new Epoch(Duration.OnePlanckTime, "Planck Epoch"),
        new Epoch(new Duration(planckTime: 185486000), "Grand unification Epoch"),
        new Epoch(new Duration(planckTime: 1854680000000), "Inflationary Epoch"),
        new Epoch(new Duration(yoctoseconds: Duration.YoctosecondsPerPicosecond), "Electroweak Epoch"),
        new Epoch(new Duration(nanoseconds: 9999, yoctoseconds: Duration.YoctosecondsPerPicosecond * 990), "Quark Epoch"),
        new Epoch(new Duration(nanoseconds: 999990000), "Hadron Epoch"),
        new Epoch(new Duration(nanoseconds: (ulong)Duration.NanosecondsPerSecond * 9), "Lepton Epoch"),
        new Epoch(new Duration(aeons: 3, years: 70000), "Photon Epoch"),
        new Epoch(new Duration(aeons: 2996, years: 30000), "Cosmic Dark Ages"),
        new Epoch(new Duration(aeons: 7003, years: 70000), "Reionization"),
        new Epoch(new Duration(aeons: 85324, years: 30000), "Prior to the formation of Earth"),
        new Epoch(new Duration(aeons: 5672), "Hadean Eon"),
        new Epoch(new Duration(aeons: 15000), "Archean Eon"),
        new Epoch(new Duration(aeons: 19590), "Proterozoic Eon"),
        new Epoch(new Duration(aeons: 556), "Cambrian Period"),
        new Epoch(new Duration(aeons: 416), "Ordovician Period"),
        new Epoch(new Duration(aeons: 246), "Silurian Period"),
        new Epoch(new Duration(aeons: 603), "Devonian Period"),
        new Epoch(new Duration(aeons: 600), "Carboniferous Period"),
        new Epoch(new Duration(aeons: 470), "Permian Period"),
        new Epoch(new Duration(aeons: 563), "Jurassic Period"),
        new Epoch(new Duration(aeons: 790), "Cretaceous Period"),
        new Epoch(new Duration(aeons: 429, years: 70000), "Paleogene Period"),
        new Epoch(new Duration(aeons: 176, years: 97000), "Miocene Epoch"),
        new Epoch(new Duration(aeons: 27, years: 53000), "Pliocene Epoch"),
        new Epoch(new Duration(aeons: 25, years: 68350), "Pleistocene Epoch"),
        new Epoch(new Duration(years: 11576), "Holocene Epoch"),
    };

    /// <summary>
    /// The duration of this epoch.
    /// </summary>
    public Duration Duration { get; }

    /// <summary>
    /// The name of this epoch. May be <see langword="null"/>.
    /// </summary>
    public string? Name { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="Epoch"/>.
    /// </summary>
    /// <param name="duration">The duration of the epoch. Must be positive and nonzero.</param>
    /// <param name="name">The name of the epoch. May be <see langword="null"/>.</param>
    public Epoch(Duration duration, string? name = null)
    {
        if (duration.IsNegative || duration.IsZero)
        {
            throw new ArgumentException($"{nameof(duration)} must be positive and non-zero", nameof(duration));
        }
        Duration = duration;
        Name = name;
    }
}
