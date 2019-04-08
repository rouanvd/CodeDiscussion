<Query Kind="FSharpProgram">
  <Output>DataGrids</Output>
</Query>


// f :: String -> String -> String
let f (v1: string) (v2: string): string = 
  v1 + v2
  
//let prefixHello (v1: string): string = "hello " + v1
let prefixHello = f "hello "

 

//let o = f "hello "

let o = prefixHello "world"
Console.WriteLine( o )
