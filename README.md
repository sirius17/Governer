# Governer
[![Build status](https://ci.appveyor.com/api/projects/status/afho24kma3psc734?svg=true)](https://ci.appveyor.com/project/sirius17/governer)

The need behind Governer was to create a simple component to do tenant specific api rate limiting for web service apis.

## Overview
Governer uses a windowing mechanism to do rate limiting. Each request increments a gauge for a specific "time window". When the window limit is hit for a specific time window, the governer is triggered to indicate rate limit violations.
Buckets in this case are identified as windows of fixed intervals since a particular epoch time.

The important thing here is that when we are talking about a multi server environment, it is important that each server has the same concept of "time".
Clock differences from server to server would result in each server resolving a request to a different bucket. This would result in an incorrect count of the current traffic. To mitigate this, Governer uses an inbuilt clock which can be synchronized periodically from a list of time servers. This will ensure that irrespective of the system time, Governer will remain latched onto the reference time server that you use.

## Sample usage
``` c#
// One-time setup of Governer parameters.
Governer
	.Configuration
	.WithClock(new NtpTimeServer("pool.ntp.org"))
	.WithStorage("redis connection string")
	.Apply();

// Then on each request you can check rate violations with
var gaugeName = "tenant-id-plus-api-name";
var windowSize = 5 * 60; // 5 minutes
var rate = 10;  //10 rps
var governer = new Governer( new Gauge(gaugeName, windowSize), rate);
if( governer.IsAllowed() == false )
	... code for rate violations.
```
There are a couple of alternate overloads which give you more fine grained control in configuring the library. You can use those to provide your own custom time server, clock or storage implementations.

## Primary components

### Clock
This is the abstraction for time subsystem inside Governer. It depends on a `TimeService` to provide it the latest "correct" reference time. The clock continually corrects itself based on its deviation from the reference time. The `TimeService` itself depends on one or more  `TimeServers`. Time servers represent connectors to external servers which provide time information. E.g., Ntp servers.

### Gauge
Gauges are used to measure the number of api calls being made to a particular gauge. Each gauge has a unique name for which it maintains a count. This name can be used to partition traffic across tenants, apis and other scopes. Windowing of counts is also provided by the gauge.

### Governer
The Governer acts as the entry point into the library and is responsible for the rate limiting logic. 
