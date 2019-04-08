<Query Kind="Program">
  <Output>DataGrids</Output>
</Query>

void Main()
{
  var c = CounterFactory.New();
  Console.WriteLine( c.getVal() );
  
  c.inc();
  Console.WriteLine( c.getVal() );
  
  c.dec();
  Console.WriteLine( c.getVal() );  }

// Define other methods and classes here
class CounterFactory {
  
  public static (Func<int> getVal, Action inc, Action dec) New() {
    var counter = 0;
    Func<int> getF = () => counter;
    Action incA = () => counter++;
    Action decA = () => counter--;
    
    return (getF, incA, decA);    
  }
  
}