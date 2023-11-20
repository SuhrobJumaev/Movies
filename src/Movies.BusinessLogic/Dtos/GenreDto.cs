


namespace Movies.BusinessLogic;

public readonly struct GenreDto
{
    /// <summary> Код ошибки если 1 успешно -1 не успешно </summary>
    public int Id { get; init; }
    public string Name { get; init; }
}
