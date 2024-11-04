# 0/ EventHighway
EventHighway is Standard-Compliant .NET library for event-driven programming. It is designed to be simple, lightweight, and easy to use.


# 1/ How to Use
## 1.0/ Installation
You must define a connection string that points to a SQL DB Server when initializing the EventHighway client as follows:

```csharp
var eventHighway = new EventHighway("Server=.;Database=EventHighwayDB;Trusted_Connection=True;");
```

## 1.1/ Registering Event Address
In order for an event to be published, it must target a certain `EventAddress`. You can register an `EventAddress` as follows:

```csharp
var eventAddress = new EventAddress 
{
	Id = Guid.NewGuid(),
	Name = "EventAddressName",
	Description = "EventAddressDescription"
	CreatedDate = DateTimeOffset.UtcNow,
	UpdatedDate = DateTimeOffset.UtcNow
};

await eventHighway.EventAddresses.RegisterEventAddressAsync(eventAddress);
```

Make sure you store your `EventAddress` Id in a safe place, as you will need it to publish events to that address.

## 1.2/ Registering Event Listeners
In order to listen to events, you must register an `EventListener` as follows:

```csharp
var eventListener = new EventListener
{
	Id = Guid.NewGuid(),
	Endpoint = "https://my.endpoint.com/api/v1.0/students",
	EventAddressId = SomePreconfiguredEventAddressId,
	CreatedDate = DateTimeOffset.UtcNow,
	UpdatedDate = DateTimeOffset.UtcNow
};

await eventHighway.EventListeners.RegisterEventListenerAsync(eventListener);
```

## 1.3/ Publishing Events
You can publish an event as follows:

```csharp
var event = new Event
{
	Id = Guid.NewGuid(),
	EventAddressId = SomePreconfiguredEventAddressId,
	Content = "SomeStringifiedJsonContent",
	CreatedDate = DateTimeOffset.UtcNow,
	UpdatedDate = DateTimeOffset.UtcNow
};

await eventHighway.Events.PublishEventAsync(event);

```

When an event is published, a notification will be sent to all registered `EventListeners` that are listening to the event's `EventAddress`. A record of the status of the published event per listener will be available through the `ListenerEvent` table in the database.

# Note
This is an early release of a Pub/Sub pattern core library which can be deployed within an API or simple Console Application. It was intentionally built to be platform agnostic so it can process events from anywhere to anywhere.

There are plans for more abstraction and customization in the future, such as:

- Enable plugging anything that implements `IStorageBroker` so consumers can use any storage mechanism or technology they prefer.
- Enable eventing beyond RESTful APIs. Like running the library within one microservice from Service to Service in a LakeHouse model.

