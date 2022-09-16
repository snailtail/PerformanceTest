using System.Text;
using BenchmarkDotNet.Attributes;
namespace PerformanceTest;

[MemoryDiagnoser(false)]
public class BenchMark
{
    [Params(100,1_000,1_000_000)]
	public int Size { get; set; }

	[Benchmark]
	public void add_strings()
	{
		string x = string.Empty;
		for(int n = 0; n< Size; n++)
		{
			x += "x";
		}
	}

        [Benchmark]
        public void interpolated_strings()
        {
            string x = string.Empty;
            for (int n = 0; n < Size; n++)
            {
                x = $"{x}x";
            }
        }

    [Benchmark]
    public void using_stringbuilder()
    {
        var sb = new StringBuilder();
        for (int n = 0; n < Size; n++)
        {
            sb.Append("x");
        }
        string x = sb.ToString();
    }
    }

