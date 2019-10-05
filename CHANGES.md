# version 1.1.0

- All non-generoc result types `IEnumerable<(string name, object value)>` - are replaced with materialized lists, type: `IList<(string name, object value)>`.

- Consequently name/value tuple results are generating lists structure and do not deffer serialization and rhis allowed simplification of extensions. Current list of extensions:
[see here](https://github.com/vbilopav/NoOrm.Net/blob/master/Norm/Extensions/NormExtensions.cs)

- Added extension for O/R Mapping  by using [`FastMember`](https://github.com/mgravell/fast-member) library

Note: 
`FastMember` yields slightly better results then Dapper but it doesn't support Ahead of Time compilation scenarious and members are case sensitive.

- Expeanded generic tuple parameters up to 10 members max. Will be more in future.
