namespace CS.Edu.Tests.Utils.Models;

class Selectable<T> : Valuable<T>
{
    private bool _isSelected;

    public Selectable() : base(default) { }

    public bool IsSelected
    {
        get => _isSelected;
        set => SetAndRaise(ref _isSelected, value);
    }
}