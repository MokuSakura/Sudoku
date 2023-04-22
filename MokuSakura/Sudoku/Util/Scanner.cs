using System.Text;

namespace MokuSakura.Sudoku.Util;

public class Scanner : StreamReader
{
    private String currentWord = "";
    private Boolean end;
    private Boolean skipped = false;
    public Scanner(Stream source) : base(source)
    {
    }

    private void ReadNextWord()
    {
        StringBuilder sb = new();
        do
        {
            Int32 next = this.Read();
            if (next < 0)
                break;
            Char nextChar = (Char)next;
            if (Char.IsWhiteSpace(nextChar))
                break;
            sb.Append(nextChar);
        } while (true);

        while ((this.Peek() >= 0) && (Char.IsWhiteSpace((Char)this.Peek())))
            this.Read();
        if (sb.Length > 0)
            currentWord = sb.ToString();
        else
            end = true;
    }
    

    public Int32 NextInt()
    {
        if (!skipped)
        {
            ReadNextWord();
        }
        
        return Int32.Parse(currentWord);
    }

    private void EnsureNext()
    {
        if (!HasNext())
        {
            throw new InvalidOperationException();
        }
    }

    public Double NextDouble()
    {
        if (!skipped)
        {
            ReadNextWord();
        }
        return Double.Parse(currentWord);
    }

    public Boolean HasNext()
    {
        ReadNextWord();
        skipped = true;
        return !end;
    }
}
