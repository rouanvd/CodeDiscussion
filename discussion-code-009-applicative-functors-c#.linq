<Query Kind="Program">
  <Output>DataGrids</Output>
  <Namespace>static OptionM</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

void Main()
{
  /*
  Applicative Functor
    pure  :: A -> F<A>
    apply :: (F<A -> B>) -> F<A> -> F<B>
    apply :: (F<A -> B -> C>) -> F<A> -> F<B> -> F<C>
    ...
  */
  
//  var v1 = Some(0);
//  var v2 = Some(3);
//  Func<int,int,int> opAdd = (n1, n2) => n1 + n2;
  
//  Apply(Pure( opAdd ), v1, v2)
//    .ForEach(Console.WriteLine);  
  
//  ApplicativeOption.Pure( opAdd )
//    .Ap( v1, v2 )
//    .ForEach(Console.WriteLine);

  var v1 = new List<int>{2, 5, 10};
  var v2 = new List<int>{3, 5};
  Func<int,int,int> opAdd = (n1, n2) => n1 + n2;

  Apply(Pure( opAdd ), v1, v2)
    .ForEach(Console.WriteLine);  



//  
//  ApplicativeList.Pure( opAdd )
//    .Ap( v1, v2 )
//    .ForEach(Console.WriteLine);

//  var v1 = new Task<int>(() => { Thread.Sleep(3000); return 2; });
//  var v2 = new Task<int>(() => { Thread.Sleep(6000); return 3; });
//  Func<int,int,int> opAdd = (n1, n2) => n1 + n2;
//  
//  v1.Start();
//  v2.Start();
//  
//  ApplicativeTask.Pure( opAdd )
//    .Ap( v1, v2 )
//    .ForEach(Console.WriteLine);
}


List<A> Pure<A>(A value) {
  return new List<A> { value };
}

List<C> Apply<A,B,C>(List<Func<A,B,C>> af, List<A> v1, List<B> v2) {
  if (af.Count <= 0 || v1.Count <= 0 || v2.Count <= 0)
    return new List<C>();
    
  var results = new List<C>();
  
  foreach (var af_ in af)
  foreach (var v1_ in v1)
  foreach (var v2_ in v2)
    results.Add( af_(v1_, v2_) );
  
  return results;
    
//  var af_ = af.Value;
//  var v1_ = v1.Value;
//  var v2_ = v2.Value;
//  
//  return Some( af_( v1_, v2_ ) );
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
// Define our Functor instances for List<> & Option<> & Task<>
// ---------------------------------------------------------------
static class FunctorOption
{
  public static Option<U> Map<T,U>(this Option<T> v, Func<T,U> f) 
  {
    if (v.IsSome)
      return Some( f( v.Value ) );
      
    return None<U>();
  }
  
  public static void ForEach<T>(this  Option<T> v, Action<T> actionF)
  {
    if (v.IsSome)
      actionF( v.Value );
  }
}

static class FunctorList
{
  public static List<U> Map<T,U>(this List<T> v, Func<T,U> f) 
  {
    return v.Select( f ).ToList();
  }
  
  public static void ForEach<T>(this List<T> v, Action<T> actionF)
  {
    v.ForEach( actionF );
  }
}

static class FunctorTask
{
  public static Task<U> Map<T,U>(this Task<T> v, Func<T,U> f) 
  {
    return v.ContinueWith(x => f( x.Result ));
  }
  
  public static void ForEach<T>(this Task<T> v, Action<T> actionF)
  {
    v.ContinueWith( r => actionF(r.Result) );
  }
}


// ---------------------------------------------------------------
// Define our Applicative instances for List<> & Option<> & Task<>
// ---------------------------------------------------------------
static class ApplicativeOption
{
  public static Option<T> Pure<T>(T v) {
    return Some( v );
  }
  
  public static Option<U> Ap<T,U>(this Option<Func<T,U>> af, Option<T> v) {
    if (af.IsSome && v.IsSome)
      return v.Map( af.Value );
    
    return None<U>();
  }
  
  public static Option<V> Ap<T,U,V>(this Option<Func<T,U,V>> af, Option<T> v1, Option<U> v2) {
    if (af.IsSome && v1.IsSome && v2.IsSome)
      return Some( af.Value( v1.Value, v2.Value ) );
    
    return None<V>();
  }
}


static class ApplicativeList
{
  public static List<T> Pure<T>(T v) {
    return new List<T>{ v };
  }
  
  public static List<U> Ap<T,U>(this List<Func<T,U>> af, List<T> v) {
    if (af.Any() && v.Any())
      return v.SelectMany(v_ => af.Map(f => f(v_) ) ).ToList();
    
    return new List<U>();
  }
  
  public static List<V> Ap<T,U,V>(this List<Func<T,U,V>> af, List<T> v1, List<U> v2) {
    if (af.Any() && v1.Any() && v2.Any()) {
      var resultList = new List<V>();
      
      foreach (var af_ in af)
      foreach (var v1_ in v1)
      foreach (var v2_ in v2)
        resultList.Add( af_(v1_, v2_) );
        
      return resultList;
    }
    
    return new List<V>();
  }
}


static class ApplicativeTask
{
  public static Task<T> Pure<T>(T v) {
    return Task.Run(() => v);
  }
  
  public static Task<U> Ap<T,U>(this Task<Func<T,U>> af, Task<T> v) {
    return Task.WhenAll( new List<Task>{ af, v } )
               .ContinueWith((t) => {
                 var af_ = af.Result;
                 var v_  = v.Result;
                 return af_( v_ );          
               });
  }
  
  public static Task<V> Ap<T,U,V>(this Task<Func<T,U,V>> af, Task<T> v1, Task<U> v2) {
    return Task.WhenAll( new List<Task>{ af, v1, v2 } )
               .ContinueWith((t) => {
                 var af_  = af.Result;
                 var v1_  = v1.Result;
                 var v2_  = v2.Result;
                 return af_( v1_, v2_ );          
               });
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

public static class OptionM
{
  public static Option<T> Some<T>(T v) { return new Option<T>(v); }
  public static Option<T> None<T>() { return new Option<T>(); }
}