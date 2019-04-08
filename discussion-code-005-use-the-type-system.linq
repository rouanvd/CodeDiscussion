<Query Kind="Program">
  <Output>DataGrids</Output>
</Query>

// thinking functionally is about immutable values and the transformation into other values.
// Imperative thinking is mutating variables / updating


// AvailableSlocLookupType - explain why we did it this way and why it is not an enum

// DefaultAvailableSlocLookup - explain the 


void Main()
{
//  var light = new AnOffLight();
//  light.SwitchOn()
//       .SwitchOff()
//       .ReplaceBulb()
//       .SwitchOn()
//       .SwitchOff();
  
  var light = ALight.NewOffLight();
  light.SwitchOn()
       .SwitchOff()
       .SwitchOn()
       .ReplaceBulb()
       .SwitchOff()
       .SwitchOn();  

}

void UserChangeBulb(string user, ALight<OffL> light)
{
  light.ReplaceBulb();
}


class OffL {}
class OnL {}


class ALight<TState>
{
  public bool IsOn {get;private set;}
  
  private ALight() {}
  
  public static ALight<OnL> NewOnLight() { return new ALight<OnL> { IsOn = true }; }
  public static ALight<OffL> NewOffLight() { return new ALight<OffL> { IsOn = true }; }
    
//  public ALight<OffL> SwitchOff() { return NewOffLight(); }
//  public ALight<OnL> SwitchOn() { return NewOnLight(); }
//  public ALight<TState> ReplaceBulb() { return this; }
  
  
}

static class ALight
{
  public static ALight<OnL> NewOnLight() { return ALight<OffL>.NewOnLight(); }
  public static ALight<OffL> NewOffLight() { return ALight<OffL>.NewOffLight(); }
  
  public static ALight<OffL> SwitchOff(this ALight<OnL> l) { return ALight<OffL>.NewOffLight(); }
  public static ALight<OnL> SwitchOn(this ALight<OffL> l) { return ALight<OffL>.NewOnLight(); }
  public static ALight<T> ReplaceBulb<T>(this ALight<T> l) { return l; }
}



//class ALight
//{
//  public bool IsOn {get;protected set;}
//  
//  public void Foo(ALight other) {
//    other.IsOn = false;
//  }
//}
//
//class AnOnLight : ALight
//{
//  public AnOnLight() { IsOn = true; }
//  public AnOffLight SwitchOff() { return new AnOffLight(); }  
//}
//
//class AnOffLight : ALight
//{
//  public AnOffLight() { IsOn = false; }
//  public AnOnLight SwitchOn() { return new AnOnLight(); }
//  public AnOffLight ReplaceBulb() { return this; }
//}









//class ALight<TState>
//{
//  public bool IsOn {get;private set;}
//  
//  public void SwitchOn() { IsOn = true; }
//  public void SwitchOff() { IsOn = false; }
//  
//  public void ReplaceBulb() {  }
//}


//interface ILight
//{
//  bool IsOn {get;}
//  void SwitchOn();
//  void SwitchOff();
//  
//  void ReplaceBulb();
//}




























// we want to be able to define functions that can only work Light<> values in
// a specific state
Light<Off> replaceLightBulb(Light<Off> l)
{
  return l.ReplaceBulb();
}




// technique is more useful for IMMUTABLE values
class Light<TState>
{
  public bool IsOn {get;private set;}
  
  // need to make the constructor private so clients cannot create a Light<T> value
  // with inconsistent types and state.
  private Light(bool isOn) { IsOn = isOn; }
  
  
  // need smart constructors to let user create valid Light<T> values
  public static Light<On> NewOn()
  {
    return new Light<On>( true );
  }
  
  public static Light<Off> NewOff()
  {
    return new Light<Off>( true );
  }
  
  
  // operations on a Light<T> value that changes its type
  public Light<On> TurnOn()
  {
    return new Light<On>( true );
  }
  
  public Light<Off> TurnOff()
  {
    return new Light<Off>( false );
  }
  
  public Light<TState> ReplaceBulb()
  {
    return this;
  }
}

// types that represents the possible states of a Light
public class On {}
public class Off {}