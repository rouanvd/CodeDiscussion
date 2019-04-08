<Query Kind="Program">
  <Output>DataGrids</Output>
  <Namespace>static UserQuery.Option</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

void Main()
{
//  var o1 = Some( 3 );
//  printVals( new Functor_Option<int>(o1) );
//  
//  var o2 = new List<int>{ 1, 2, 3 };  
//  printVals( new Functor_List<int>(o2) );

var o3 = new Task<int>(() => { Thread.Sleep(3000); return 22; });
o3.Start();
printVals( new Functor_Task<int>(o3) );

}


// ---------------------------------------------------------------
// Define a function that works over Functor values
// ---------------------------------------------------------------
void printVals<T>(IFunctor<T> f) {
  f.Map( v => "\"" + v.ToString() + "\"" )
   .ForEach( Console.WriteLine );
}

void printOpt<T>(Option<T> f) {
  f.Map( v => v.ToString() )
   .ForEach( Console.WriteLine );
}


class Functor_Task<T> : IFunctor<T>
{
  private Task<T> _v;
  public Functor_Task(Task<T> v)
  {
    _v = v;
  }
  
  public IFunctor<U> Map<U>(Func<T,U> f) 
  {
    return new Functor_Task<U>( _v.ContinueWith(tsk => f( tsk.Result )) );
  }
  
  public void ForEach(Action<T> actionF)
  {
    _v.ContinueWith( r => actionF(r.Result) );
  }
}


// ---------------------------------------------------------------
// Define functions that return values of type Option<>
// ---------------------------------------------------------------
class Warehouse { public string Name {get;set;} }
class Inventory { public string PartNo {get;set;} }

Option<Warehouse> getWarehouseFromDb(int warehouseId) {
  if (warehouseId == 1)
    return Some( new Warehouse{ Name = "Warehouse 1" } );
    
  return None<Warehouse>();
}

Option<List<Inventory>> getInventoryFromDb(Warehouse wh) {
  if (wh.Name == "Warehouse 1") {
    var inventories = new List<Inventory> 
                      { new Inventory{ PartNo = "Part A" }
                      , new Inventory{ PartNo = "Part B" } };
    return Some( inventories );
  }
    
  return None<List<Inventory>>();
}


































// ---------------------------------------------------------------
// Define our IFunctor interface
// ---------------------------------------------------------------
interface IFunctor<T> {
  IFunctor<U> Map<U>(Func<T,U> f);
  void ForEach(Action<T> actionF);
}


// ---------------------------------------------------------------
// Define our IFunctor instances for List<> & Option<> & Task<>
// ---------------------------------------------------------------
class Functor_Option<T> : IFunctor<T>
{
  private Option<T> _v;
  public Functor_Option(Option<T> v)
  {
    _v = v;
  }
  
  public IFunctor<U> Map<U>(Func<T,U> f) 
  {
    return new Functor_Option<U>( _v.Map( f ) );
  }
  
  public void ForEach(Action<T> actionF)
  {
    _v.ForEach( actionF );
  }
}


class Functor_List<T> : IFunctor<T>
{
  private List<T> _v;
  public Functor_List(List<T> v)
  {
    _v = v;
  }
  
  public IFunctor<U> Map<U>(Func<T,U> f) 
  {
    return new Functor_List<U>( _v.Select( f ).ToList() );
  }
  
  public void ForEach(Action<T> actionF)
  {
    _v.ForEach( actionF );
  }
}































// ---------------------------------------------------------------
// Define our IApplicative interface
// ---------------------------------------------------------------
interface IApplicative<T> {
  IApplicative<U> Map<U>(Func<T,U> f);
  void ForEach(Action<T> actionF);

  IApplicative<U> Ap<U>(IApplicative<Func<T,U>> f);
  IApplicative<V> Ap<U,V>(IApplicative<Func<T,U,V>> f, IApplicative<U> v2);
}


// ---------------------------------------------------------------
// Define our IApplicative instances for List<> & Option<> & Task<>
// ---------------------------------------------------------------
class Applicative_Option {
  public static Applicative_Option<V> Pure<V>(V val) {
    return new Applicative_Option<V>( Some(val) );
  }
}

class Applicative_Option<T> : IApplicative<T>
{
  private Option<T> _value;
  
  public Applicative_Option(Option<T> v)
  {
    _value = v;
  }
  
  
  public Option<T> Value => _value;
  
  
  public IApplicative<U> Map<U>(Func<T,U> f) 
  {
    return new Applicative_Option<U>( _value.Map( f ) );
  }
  
  public void ForEach(Action<T> actionF)
  {
    _value.ForEach( actionF );
  }
  
  
  public IApplicative<V> Pure<V>(V val)
  {
    return new Applicative_Option<V>( Some(val) );
  }

  public IApplicative<U> Ap<U>(IApplicative<Func<T,U>> v)
  {
    if (_value.IsNone)
      return new Applicative_Option<U>( None<U>() );
      
    return (IApplicative<U>)v.Map( f => f(_value.Value) );
  }

  public IApplicative<V> Ap<U, V>(IApplicative<Func<T, U, V>> af, IApplicative<U> v2)
  {
    var v2_ = ((Applicative_Option<U>)v2).Value;
    
    if (_value.IsNone || v2_.IsNone)
      return new Applicative_Option<V>( None<V>() );
      
    return af.Map( f => f(_value.Value, v2_.Value) );
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
  
  #region Functor API
  public Option<U> Map<U>(Func<T, U> f) {
    if (IsSome)
      return Some( f( Value ) );
      
    return None<U>();
  }
  
  public void ForEach(Action<T> a) {
    if (IsSome)
      a( Value );
  }
  #endregion
}

public static class Option
{
  public static Option<T> Some<T>(T v) { return new Option<T>(v); }
  public static Option<T> None<T>() { return new Option<T>(); }
}