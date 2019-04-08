<Query Kind="Program">
  <Output>DataGrids</Output>
</Query>

void Main()
{
}


T returnSameValue<T>(T value) {
  return value;
}


void MakeAnimalTalk(Animal a)
{
  a.MakeSound();
}


interface Animal
{
  void MakeSound();
}


class Dog : Animal
{
  public void MakeSound()
  {
    Console.WriteLine("Woof");
  }
}


class GermanShepard : Dog
{}


class Cat : Animal
{
  public void MakeSound()
  {
    Console.WriteLine("Moew");
  }  
}