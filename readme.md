# Errors Toolbox
 
This toolbox contains objects and exceptions for error handling.

## Table of Contents

<!-- START doctoc generated TOC please keep comment here to allow auto update -->
<!-- DON'T EDIT THIS SECTION, INSTEAD RE-RUN doctoc TO UPDATE -->

- [Installation](#installation)
- [ExceptionMapper](#exceptionmapper)
  - [Configure mappings](#configure-mappings)
  - [Mapping exceptions](#mapping-exceptions)
  - [Default mappings](#default-mappings)
  - [Resolving exceptions](#resolving-exceptions) 
- [Error object](#error-object)
  - [ToString](#tostring)
- [Exceptions](#exceptions)
  - [BaseException](#baseexception)
	  - [AddMessage](#addmessage)
	  - [AddMessages](#addmessages)
  - [NotFoundException](#notfoundexception)
  - [UnauthorizedException](#unauthorizedexception)
  - [ForbiddenException](#forbiddenexception)
  - [ValidationException](#validationexception)
  - [BadGatewayException](#badgatewayexception)
  - [GatewayTimeoutException](#gatewaytimeoutexception)


<!-- END doctoc generated TOC please keep comment here to allow auto update -->

# Installation

Adding the DataAccess Toolbox to a project is as easy as adding it to the csproj project file:

```xml
  <ItemGroup>
    <PackageReference Include="Digipolis.Errors" Version="5.2.0" />
  </ItemGroup>
``` 

or if your project still works with project.json :

``` json
 "dependencies": {
    "Digipolis.Errors":  "5.2.0", 
 }
```

Alternatively, it can also be added via the NuGet Package Manager interface.

# ExceptionMapper
ExceptionMapper is an abstract class that needs to be implemented in your project to be able to map certain [Exceptions](#exceptions) 
to a specific [Error object](#error-object). This is not limited to the exceptions contained in the toolbox. But can be
any exception the inherits from System.Exception! 

## Configure mappings
To map exceptions to [Error objects](#error-object) you need to implement two functions:
``` csharp
protected abstract void Configure();

protected abstract void CreateDefaultMap(Error error, Exception exception);
``` 
- CreateDefaultMap is needed to resolve an [Error object](#error-object) for exceptions that are not mapped in the
Configure method.

- Configure is where the mappings are made between Exceptions and [Error objects](#error-object).

### Mapping exceptions
To configure mappings you can use two method:
``` csharp
/// <summary>
/// Used to map an exception to an Error object were only Status is set
/// </summary>
protected void CreateMap<TException>(int statusCode) where TException : Exception

/// <summary>
/// Used to map an exception to an Error object
/// </summary>
protected void CreateMap<TException>(Action<Error, TException> configError) where TException : Exception
``` 

#### Example
``` csharp
protected override void Configure()
{
    CreateMap<NotImplementedException>((error, ex) =>
    {
        error.Status = (int) HttpStatusCode.NotFound;
        error.Title = "Methode call not allowed";
        error.Code = "NOTF001";
    });

    CreateMap<UnauthorizedAccessException>((int)HttpStatusCode.Forbidden);
}
``` 

### Default mappings
There are already default mappings provided for [NotFoundException](#notfoundexception), [UnauthorizedException](#unauthorizedexception), [ForbiddenException](#forbiddenexception)
[ValidationException](#validationexception), [BadGatewayException](#badgatewayexception) and [GatewayTimeoutException](#gatewaytimeoutexception) 
also contained in this toolbox. These default mapping can be overriden.

``` csharp
protected override void CreateNotFoundMap(Error error, NotFoundException exception)
{
    error.Title = "not Found";
    error.Code = "NFOUND001";
    error.Status = (int)HttpStatusCode.NotFound;
    error.ExtraInfo = exception.Messages.ToDictionary(ms => ms.Key, ms => (object)ms.Value);
}

protected override void CreateUnauthorizedMap(Error error, UnauthorizedException exception)
{
    error.Title = "Access denied.";
    error.Code = "UNAUTH001";
    error.Status = (int)HttpStatusCode.Unauthorized;
    error.ExtraInfo = exception.Messages.ToDictionary(ms => ms.Key, ms => (object)ms.Value);
}

protected override void CreateForbiddenMap(Error error, ForbiddenException exception)
{
    error.Title = "Forbidden";
    error.Code = "FORBID001";
    error.Status = (int)HttpStatusCode.Forbidden;
    error.ExtraInfo = exception.Messages.ToDictionary(ms => ms.Key, ms => (object)ms.Value);
}

protected override void CreateValidationMap(Error error, ValidationException exception)
{
    error.Title = "Bad request.";
    error.Code = "UNVALI001";
    error.Status = (int)HttpStatusCode.BadRequest;
    error.ExtraInfo = exception.Messages.ToDictionary(ms => ms.Key, ms => (object)ms.Value);
}

protected virtual void CreateBadGateWayMap(Error error, BadGatewayException exception)
{
    error.Title =  "Bad Gateway."
    error.Code =  "GTWAY001";
    error.Status = (int)HttpStatusCode.BadGateway;
    error.ExtraInfo = exception.Messages.ToDictionary(ms => ms.Key, ms => ms.Value);
}

protected virtual void CreateGatewayTimeoutMap(Error error, GatewayTimeoutException exception)
{
    error.Title =  "Gateway Timeout.";
    error.Code =  "GTWAY002";
    error.Status = (int)HttpStatusCode.GatewayTimeout;
    error.ExtraInfo = exception.Messages.ToDictionary(ms => ms.Key, ms => ms.Value);
}
``` 

### Resolving exceptions
This class also provides a Resolve function that will translate an exception to an [Error object](#error-object). 
This function can also be overriden to implement additional or custom logic.

``` csharp
try
{
    ...
}
catch (Exception ex)
{
    var mapper = new ExceptionMapperTester();
    var error = mapper.Resolve(ex)
}
``` 

# Error object

The _**Error**_ object contains the following properties.

``` csharp
public class Error
{
    /// <summary>
    /// A unique id to identify error messages in the logs
    /// </summary>
    public Guid Identifier { get; private set; }

    /// <summary>
    /// A URI to an absolute or relative html resource to identify the problem.
    /// </summary>
    public Uri Type { get; set; }

    /// <summary>
    /// A short description of the error
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// The http Status code 
    /// </summary>
    public int Status { get; set; }

    /// <summary>
    /// A code to identify what error it is.
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Extra parameters to clarify the error
    /// </summary>
    public Dictionary<string, object> ExtraInfo { get; set; }
}
```

An _**Error**_ object can be instantiated by using one of both constructors
 ``` csharp
 public Error(Dictionary<string, object> extraInfo = null)
    : this(Guid.NewGuid(), extraInfo)
{ }

public Error(Guid identifier, Dictionary<string, object> extraInfo = null)
{
    if (identifier == default(Guid))
        throw new ArgumentException("An empty Guid is not allowed", nameof(identifier));

    Identifier = identifier;
    ExtraInfo = extraInfo ?? new Dictionary<string, object>();
}
``` 
As you can see, you can instantiate it by specifying a custom Identifier or by letting the class generate one for you.
This way you can use this Identifier to link several error objects for e.g. logging purposes.

``` csharp
var error = new Error();
Console.WriteLine("id = {0}", error.Id);
//outputs: id = 22699ff7-8496-49b2-8772-4c9e952a39fd
```

The constructor also optionally accepts a dictionary of extra parameters :

``` csharp
var extraInfo = new Dictionary<string, object>();
extraInfo.Add("key1", "message1");
extraInfo.Add("key2", "message2");

//constructors
var error1 = new Error(extraInfo);
var error2 = new Error(Guid.NewGuid(), extraInfo);
``` 

## ToString

The ToString method returns the contents of the object as a string. This can be handy when tracing and debugging.

# Exceptions

## BaseException

A base class for exceptions. It inherits from the standard Exception class and has the following extra field :

``` csharp
public Dictionary<string, IEnumerable<string>> Messages { get; protected set; }
```
- This Messages property will contain all additional messages you want to copy over to the ExtraInfo property 
of the [Error object](#error-object) when resolving it through the [Resolve](#resolving-exceptions) 
function of the [ExceptionMapper](#exceptionmapper).

### AddMessage

A string message or a key and string message can be given as arguments. If only a string message is given, the _**Key**_ field will be _**String.Empty**_.

``` csharp
var error = new Error();
error.AddMessage("aMessage");
error.AddMessage("aKey", "anotherMessage");
``` 

### AddMessages

Adds multiple string messages to the ErrorMessage list. The key for each message will be _**String.Empty**_ unless a 
key is given in the parameters

``` csharp
var message1 = "message1";
var message2 = "messages2";
var messages = new string[] { message1, message2 };

var error = new Error();
error.AddMessages(messages);
error.AddMessages("aKey", messages);
``` 

## NotFoundException

To be used when a requested resource is not found or does not exist.
``` csharp
//constructor
public NotFoundException(string message = "Not found.", string code = "NFOUND001", Exception exception = null, Dictionary < string, IEnumerable<string>> messages = null )
    : base(message, exception, messages)
{}
```
Inherits from [BaseException](#baseexception) and has all the functionality of the base.

## UnauthorizedException

Used when the user does not have sufficient rights.
``` csharp
//constructor
public UnauthorizedException(string message = "Access denied.", string code = "UNAUTH001", Exception exception = null, Dictionary < string, IEnumerable<string>> messages = null )
    : base(message, exception, messages)
{}
```
Inherits from [BaseException](#baseexception) and has all the functionality of the base.

## ForbiddenException

Used when the user is forbidden to access the requested resource.
``` csharp
//constructor
public ForbiddenException(string message = "Forbidden.", string code = "FORBID001", Exception exception = null, Dictionary < string, IEnumerable<string>> messages = null )
    : base(message, exception, messages)
{}
```
Inherits from [BaseException](#baseexception) and has all the functionality of the base.

## ValidationException

Can be used when input validation fails. 
``` csharp
//constructor
public ValidationException(string message = "Bad request.", string code = "UNVALI001", Exception exception = null, Dictionary < string, IEnumerable<string>> messages = null )
    : base(message, exception, messages)
{}
```
Inherits from [BaseException](#baseexception) and has all the functionality of the base.

## BadGatewayException

Can be used when a server can't be reached. 
``` csharp
//constructor
public BadGatewayException(string message = "Bad Gateway.", string code = "GTWAY001", Exception exception = null, Dictionary < string, IEnumerable<string>> messages = null )
    : base(message, exception, messages)
{}
```
Inherits from [BaseException](#baseexception) and has all the functionality of the base.

## GatewayTimeoutException

Can be used when a server can't be reached. 
``` csharp
//constructor
public GatewayTimeoutException(string message = "Gateway timeout.", string code = "GTWAY002", Exception exception = null, Dictionary < string, IEnumerable<string>> messages = null )
    : base(message, exception, messages)
{}
```
Inherits from [BaseException](#baseexception) and has all the functionality of the base.