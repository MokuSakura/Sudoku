using System.Text;

namespace MokuSakura.Sudoku.Core.Util;

public class Scanner : StreamReader
{
    private string currentWord = "";
    private bool end;
    private bool skipped = false;
    public Scanner(Stream source) : base(source)
    {
    }

    private void ReadNextWord()
    {
        StringBuilder sb = new();
        do
        {
            int next = this.Read();
            if (next < 0)
                break;
            char nextChar = (char)next;
            if (Char.IsWhiteSpace(nextChar))
                break;
            sb.Append(nextChar);
        } while (true);

        while ((this.Peek() >= 0) && (Char.IsWhiteSpace((char)this.Peek())))
            this.Read();
        if (sb.Length > 0)
            currentWord = sb.ToString();
        else
            end = true;
    }
    

    public int NextInt()
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

    public double NextDouble()
    {
        if (!skipped)
        {
            ReadNextWord();
        }
        return Double.Parse(currentWord);
    }

    public bool HasNext()
    {
        ReadNextWord();
        skipped = true;
        return !end;
    }
}
