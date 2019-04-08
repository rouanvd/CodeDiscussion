<Query Kind="FSharpProgram">
  <Output>DataGrids</Output>
</Query>

//let (<<) (f2: 'b -> 'c) (f1: 'a -> 'b): 'a -> 'c =
//  fun v -> f2 (f1 v)
//
//let (>>) (f1: 'a -> 'b) (f2: 'b -> 'c) : 'a -> 'c =
//  fun v -> f2 (f1 v)

let mul' (v1: int) (v2: int): int =
  v1 * v2
  
 // dbl :: int -> int
let dbl = mul' 2

// intToString :: int -> Option<string>
let intToString (v: int): string = v.ToString() + "'"

// strToUpper :: string -> string
let strToUpper (v: string): string = v.ToUpper() + "<>"

// doubleNumToString :: Int -> String
let doubleNumToString: int -> string =
  dbl
  >> intToString
  >> strToUpper


let o1 = None
let o2 = Some(2)

let nums = [1; 2; 3; 4; 5]
let r = 
  List.map doubleNumToString nums
  |> List.map strToUpper
  |> List.map strToUpper
  
r.Dump()

// List    :: * -> *
// Option  :: * -> *
// RType   :: * -> *
class RType<T>

* -> *

//IFunctor
//  map :: (T -> U) -> List<T> -> List<U>

// printVal :: IFunctor<T> -> T -> Unit
let printVal functor v1 =
  let ss = functor.map( fun v -> v.ToString(), v1 )
  ss.foreach( Console.WriteLine )


  



















// 1      :: int
// int    :: *
// List   :: * -> * 
// Option :: * -> *
// Either :: * -> * -> *
//List<string> // :: *
//Option<string>
//Either<Err, String>  // :: *


