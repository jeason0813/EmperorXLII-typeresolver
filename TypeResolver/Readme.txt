
Intended for use in conjunction with xUnit (https://github.com/xunit/xunit)

More information available at https://bitbucket.org/EmperorXLII/TypeResolver


Apply the InstanceDataAttribute to your Theory tests in order to run the test over all implementing types.  For example:

interface IFoo { }
class Foo1 : IFoo { }
class Foo2 : IFoo { }
 
[Theory]
[InstanceData]
public void TestFoo( IFoo foo ) {
   // Will be called with instances of both Foo1 and Foo2
}
