# Microservices Description

The Zodiac application comprises of 5 microservices.  These are briefly described below - along with links to the documentation for each individual microservice:

## sirmione-web

Sirmione is the main Zodiac website.  It's an asp.net core application (netcoreapp3.1).  It is essentially a single page which allows the user to invoke each of the 12 different api calls (these are sread over the microsrvices).  The api is invoked, and the results are displayed.  Each api call does something different (maybe throw an exception, maybe take a long time) and the technologies bhind the apis is also different - this makes it easy to demonstrate how application insights can monitor such an application with many different parts.

[documentation](/sirmione-web/README.md) 

## limone-api

[documentation](/limone-api/README.md) 

## scorpio-api

[documentation](/scorpio-api/README.md) 

## virgoqueuehandler

[documentation](/VirgoQueueHandler/README.md) 

## libraqueuehandler

[documentation](/LibraQueueHandler/README.md) 
