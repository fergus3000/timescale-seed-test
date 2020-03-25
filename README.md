# timescale-seed-test
Experimental C# project to try seeding data to pgSQL database with the Timescale extension

The example uses dotnet core 3, with the NpgSql ADO.Net dataprovider.

The application will optionally create a database with an event_table, with a very simple schema (given in the dbScripts folder)

It is configurable to write many timeseries for a given period of time at a given datarate, default 1hz (1 event per stream per second)

Writing to db uses the recommended binary copy approach.

While the example works, it is probably not optimal, and perf improvements are welcomed, both with respect to schema and code.

When seeding many series with 1 year of data, I observe a dropoff in write speed after each new series is added. For a project that should ideally be writing hundreds of timeseries this quickly becomes a pain.

First github project!