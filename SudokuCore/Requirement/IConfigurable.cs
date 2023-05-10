namespace MokuSakura.Sudoku.Core.Requirement;

public interface IConfigurable<in TConfigType>
    where TConfigType : class, new()
{
    public void Configure(TConfigType configuration);
}