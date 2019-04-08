<Query Kind="FSharpProgram">
  <Output>DataGrids</Output>
</Query>

type Warehouse = { Name : string }

type Tree<'A> = Leaf of 'A
              | Node of Tree<'A> * Tree<'A>


//type Option<'A> = None
//                | Some of 'A

type Either<'A,'B> = Left of 'A
                   | Right of 'B

type Err = string
               
// foo :: Int -> Option<String>
// bar :: String -> Option<Boolean>

bar( foo(2) )
               
let getWarehouse (warehouseId: int): Either<Err, Warehouse> =
  if warehouseId = 0
    then Left "Could not find warehouse."
    else Right {Name="Warehouse B"}
    
    
let printWarehouseName (warehouseId: int): unit =
  let eitherW = getWarehouse warehouseId
  
  match eitherW with
    | Left err -> Console.WriteLine( err )
    | Right wh -> Console.WriteLine( wh.Name )
    
    
printWarehouseName 0