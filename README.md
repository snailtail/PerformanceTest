# PerformanceTest

## What is this?

This is a benchmarking test, to try out the BenchMarkDotNet NuGet package.
I use it to show the performance benefits of using a StringBuilder instead of just concatenating strings.
If you're only making a few concatenations it will not matter, but at scale it will have a huge impact both on speed and memory consumption on the Heap.

I have made three testcases, all of which end up producing a string consisting of n number of the character "x".  
They all share the same for loop logic, the only difference is how the string is built.  

## add_strings

The first one is a classic string concatenation using += assignment.  
Since strings are immutable i guess you can figure out that there will be a number of new strings allocated, and they need to be placed on the heap.

## interpolates_strings

This one uses the interpolated string `myvar=$"{myvar}x";`
Same thing here, immutable string = new strings allocated on the heap.  

## using_stringbuilder

The third one uses the StringBuilder from System.Text, which by a little magic does not allocate new strings every time you append something to it.
Actually it only allocates the string when you call the GetString() method on it.


## Results you say?

Of course, the benchmarks were run with three different values for how many x:es should be appended to she string.
100, 1 000 and 1 000 000.
At 100 you would not notice the difference if you just ran the code in your program.  
The StringBuilder way is one microsecond faster (which makes it three times faster) than the other two at this point, and allocates about 10 kilobytes less on the heap, but you will not notice this without measuring it with tools.  
  
At 1 000 you'll probably not notice much of a difference either. 
The StringBuilder way is now roughly 35 times faster. But you see when we benchmark it, that it is starting to scale quite fast.
Memorywise you are up to about 1 megabytes of allocations on the heap for the two first methods, and roughly 4 kilobytes for the StringBuilder one.

At 1 000 000 you will feel the pain! What took over 1.5 minutes for the two first methods, and allocated some 1 TB (!!!) of memory on the heap, took only 2 milliseconds and needed only 4 megabytes on the heap. *  
And yes, I'm aware that you would seldom concatenate one million of the same characters into one string, and yes the size of that string is the cause of the enormous allocations on the heap. But it's a good reminder to be careful with resources if you can, and this one is very easy to fix.

`* Note: This was run in a Release build from Visual Studio 2022 Preview for Mac on a Macbook Air M1 with 8GB RAM.   
Your numbers may vary.` 

## Actual benchmark output  


|               Method |    Size |                Mean |               Error |            StdDev |       Allocated |
|--------------------- |-------- |--------------------:|--------------------:|------------------:|----------------:|
|          add_strings |     100 |          1,579.3 ns |             7.24 ns |           6.77 ns |         12576 B |
| interpolated_strings |     100 |          1,580.2 ns |             7.49 ns |           7.01 ns |         12576 B |
|  using_stringbuilder |     100 |            409.4 ns |             0.78 ns |           0.61 ns |           768 B |
|          add_strings |    1000 |         71,822.2 ns |           159.99 ns |         124.91 ns |       1025976 B |
| interpolated_strings |    1000 |         71,027.3 ns |           160.13 ns |         133.72 ns |       1025976 B |
|  using_stringbuilder |    1000 |          2,766.0 ns |             3.01 ns |           2.51 ns |          4576 B |
|          add_strings | 1000000 | 96,332,038,304.5 ns | 1,039,946,892.65 ns | 868,402,754.79 ns | 1000137110168 B |
| interpolated_strings | 1000000 | 95,706,342,774.9 ns |   217,939,211.61 ns | 203,860,473.76 ns | 1000136138040 B |
|  using_stringbuilder | 1000000 |      2,708,669.1 ns |        21,950.41 ns |      20,532.42 ns |       4010393 B |

## So?

Well if performance ever might matter, like if resources are limited where your code is running (or cost mucho dineros), you should definitely use the StringBuilder method.
If not - lucky you! Keep on keepin on. :)  