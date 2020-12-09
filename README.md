# **`Norm micro orm`**

_The fastest database mapper for .NET Standard 2.1_

> **the website with tutorial currently under construction**

## Performances

See detailed perfomance benchmarks compared to **Dapper** at [performance tests page](https://github.com/vb-consulting/Norm.net/blob/master/PERFOMANCE-TESTS.md).

## Testing

250 tests for SqlServer, PostgreSQL, SQLite and MySql

![tests](https://github.com/vb-consulting/Norm.net/workflows/tests/badge.svg)

### Local testing

- Under each test project there is a `testsettings.json`. 
- Copy this file and rename it to `testsettings.local.json`. It will be ignored by git.
- Set the key `Default` to the value of your actual, local connection string.
- The key `TestDatabase` contains the name of the test database, which is created and dropped on each testing session, so be careful about that.
- Run `dotnet test`

## Usage

### Get it on Nuget

```txt
> dotnet add package Norm.net
```

### Use in project

```csharp
using Norm;

// ...
// start using database connection extensions
// ...

public record MyRecord(int Id, string Foo, string Bar);
// ...
var records = connection.Query<MyRecord>("select id, foor, bar from my_table");
// ...
```

## Currently supported platforms

- .NET Standard 2.1

## Support

This is open-source software developed and maintained freely without any compensation whatsoever.

If you find it useful please consider rewarding me on my effort by [buying me a beer](https://www.paypal.me/vbsoftware/5)🍻 or [buying me a pizza](https://www.paypal.me/vbsoftware/10)🍕

Or if you prefer bitcoin:
bitcoincash:qp93skpzyxtvw3l3lqqy7egwv8zrszn3wcfygeg0mv

## Licence

Copyright (c) Vedran Bilopavlović - VB Consulting and VB Software 2020
This source code is licensed under the [MIT license](https://github.com/vbilopav/NoOrm.Net/blob/master/LICENSE).
