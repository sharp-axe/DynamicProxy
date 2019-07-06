## Dynamic Proxy

Dynamic Proxy library provides a feature set to wrap an interface instance with decorators and a proxy without writing a wrapper class manually.

It generates a wrapper class for an interface with use of IL-code in runtime. The wrapper class executes user-defined code before, after, or instead of a member of the interface instance. It is possible to wrap functions, properties, indexers, and even events.

Adding a decorator before and after the execution of a member:
```csharp
var builder = new ReflectionProxyBuilder<ITargetInterface>();

Action beforeDecorator = () => Log("Invoke Function()");
Action<int> afterDecorator = (result) => Log($"Function() returns {result}");

builder.AddBeforeFunctionDecorator<int>(t => t.Function, beforeDecorator);
builder.AddAfterFunctionDecorator<int>(t => t.Function, afterDecorator);

var decoratedInstance = builder.Build(instanceToDecorate);
```

Setting a proxy, which decides on the execution of a member:
```csharp
...
Func<Func<int>, int> proxy = (target) => invokeTarget ? target.Invoke() : default(int);

proxyBuilder.SetProxy<int>(t => t.Function, proxy);
...
```

Adding a pair of decorators to guarantee the execution of an after-decorator if an exception has been raised after the execution of a before-decorator:
```csharp
...
Action beforeDecorator = () => Monitor.Enter(synchronization);
Action<int> afterDecorator = (_) => Monitor.Exit(synchronization);

builder.AddPairFunctionDecorators<int>(t => t.Function, beforeDecorator, afterDecorator);
...
```

**Check the demonstration application to get more examples**

#### Advantages:
- Speeds up development and enables clean code writing for some cases
- Gets rid of the need to define wrapper classes but keeps type checking of user-defined code
- Makes it possible to use Aspect-Oriented Programming paradigms

#### Limitations:
- Does not support functions with arguments passed by reference
- Does not support internal interfaces (to be added)
