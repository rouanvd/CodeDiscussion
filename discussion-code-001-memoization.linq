<Query Kind="Program">
  <Output>DataGrids</Output>
</Query>

void Main()
{
  "".Where()
    .BlaBla();
    .Apply( ComplexCheck )
    .Where()
    
  
  
  var ls = new List<string>() {"a", "b"};
  printSomething( ls, printStr );
}

bool ComplexCheck(string s)
{
  return s == "Banana";
}

// Define other methods and classes here

//Func<int, string> DoSomeSlowCalcCached()
//{
//  var d = new Dictionary<int, string>();
//  
//  Func<int, string> f = (int v) => {
//    if (d.ContainsKey( v ))
//      return d[ v ];
//      
//    var result = DoSomeSlowCalc( v );
//    d.Add( v, result );
//    
//    return result;
//  };
//  
//  return f;
//}



void printStr(string s)
{
  Console.WriteLine( s );
}


void printSomething(List<string> ls, Action<string> action)
{
  ls.ForEach( action );
}


Func<A, B> MakeMemoized<A,B>(Func<A, B> f)
{
  var d = new Dictionary<A, B>();
  
  Func<A, B> memoizedF = (A v) => {
    if (d.ContainsKey( v ))
      return d[ v ];
      
    var result = f( v );
    d.Add( v, result );
    
    return result;
  };
  
  return memoizedF;
}




string DoSomeSlowCalc(int v, string s)
{
  Thread.Sleep( 5000 );
  return v.ToString();
}


bool DoSomeSlowCalc2(string v)
{
  Thread.Sleep( 5000 );
  return String.IsNullOrWhiteSpace( v );
}