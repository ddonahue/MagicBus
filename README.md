# MagicBus
Insanely simple service bus written in .NET. Allows your app to use command and event semantics.

I mostly work on this project for my own edification. It is inspired by other projects on GitHub including [MediatR](https://github.com/jbogard/MediatR) and [SeptaBus](https://github.com/jkodroff/SeptaBus). It is also heavily inspired by [NServiceBus](http://particular.net/), and its author [Udi Dahan](http://www.udidahan.com/), whose writings got me interested in this style of development.

MagicBus allows you to create in-memory messages with command and event semantics and send them to their appropriate handlers.

General ideas for future development include:

- Add headers to messages
- Add ability to send a response/acknowledgement when a command is received by its handler
- Allow handlers to stop processing of their descendant handlers (in cases where, for example, you have a base handler that checks permissions).
- Allow asynchronous message sending.


