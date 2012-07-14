CqrsSiteEngine
==============

Experimentations in a fully CQRS, no-database website.

This is the beginning of my long-running attempt to build a CMS-ish website backend using CQRS.

As this project is very young, almost everything is minimal or work-in-progress

Primary features:

1. Fully CQRS backend
2. No database - primary data storage uses FileTapeStream to create an append-only event store in the filesystem, which denormalizes to a filesystem read model store.
3. Tools and samples:
	A. WinForms event store explorer
	B. ASP.Net MVC 4 web page

Library Dependencies

1. ProtoBuf-net 2.0.0.480
2. Ncqrs github pull from 7/5/2012
3. ASP.NET MVC 4 RC (For UserInterface project, if you intend do not wish to use that project, this is optional)

Optional Dependencies:

By default, the project uses AzureReadModelStore and AzureSystemEventStore, which require the June 2012 (or newer) version of the Azure SDK. If you do not wish to use Azure Blobs for store, FileReadModelStore and FileSystemEventStore can be used instead.

References:

Primary - How we got rid of the Database on Los Techies - http://lostechies.com/gabrielschenker/2012/06/12/how-we-got-rid-of-the-database/
	Main inspiration, source of quite a bit of code.

1. Getting started with NCQRS - http://ncqrs.org/getting-started/getting-started-with-ncqrs/
	Started by working through this, then upgraded to the lated Ncqrs version, which broke quite a bit (now fixed)