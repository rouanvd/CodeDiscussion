<Query Kind="Program">
  <Output>DataGrids</Output>
  <Namespace>static UserQuery.Option</Namespace>
</Query>

void Main()
{
//  var o = Some( 3 );
  var o = new List<int>{ 1, 2, 3 };
  printVals( new Functor_List<int>(o) );
}


// ---------------------------------------------------------------
// Define a function that works over Functor values
// ---------------------------------------------------------------
void printVals<T>(IFunctor<T> f) {
  f.Map( v => v.ToString() )
   .ForEach( Console.WriteLine );
}

































// ---------------------------------------------------------------
// Define our IFunctor interface
// ---------------------------------------------------------------
interface IFunctor<T> {
  IFunctor<U> Map<U>(Func<T,U> f);
  void ForEach(Action<T> actionF);
}


// ---------------------------------------------------------------
// Define our IFunctor instances for List<> & Option<>
// ---------------------------------------------------------------
class Functor_Option<T> : IFunctor<T>
{
  private Option<T> _v;
  public Functor_Option(Option<T> v)
  {
    _v = v;
  }
  
  public IFunctor<U> Map<U>(Func<T,U> f) {
    if (_v.IsNone)
      return new Functor_Option<U>( None<U>() );
    else
      return new Functor_Option<U>( Some<U>( f(_v.Value) ) );
  }
  
  public void ForEach(Action<T> actionF)
  {
    if (_v.IsSome)
      actionF( _v.Value );
  }
}


class Functor_List<T> : IFunctor<T>
{
  private List<T> _v;
  public Functor_List(List<T> v)
  {
    _v = v;
  }
  
  public IFunctor<U> Map<U>(Func<T,U> f) {
    return new Functor_List<U>( _v.Select( f ).ToList() );
  }
  
  public void ForEach(Action<T> actionF)
  {
    _v.ForEach( actionF );
  }
}


// ---------------------------------------------------------------
// Define our Option<T> type & helper class.
// ---------------------------------------------------------------
public class Option<T>
{
  public T Value {get;}
  
  public bool IsSome {get;private set;}
  public bool IsNone {get;private set;}
    
  public Option() {
    IsSome = false;
    IsNone = true;
  }
  
  public Option(T value) {
    Value = value;
    IsSome = true;
    IsNone = false;
  }
}

public static class Option
{
  public static Option<T> Some<T>(T v) { return new Option<T>(v); }
  public static Option<T> None<T>() { return new Option<T>(); }
}
