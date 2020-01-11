# Performances

PostgreSQL SQL test query that returns a million tuples from server:

```sql
select
    i as id,
    'foo' || i::text as foo,
    'bar' || i::text as bar,
    ('2000-01-01'::date) + (i::text || ' days')::interval as datetime
from generate_series(1, 1000000) as i
```

Following table shows execution times of Dapper read operation and different Norm read operations that yield enumerable results:

| | dapper read ([1](https://github.com/vbilopav/NoOrm.Net/blob/master/PERFOMANCE-TESTS.md#1-dapper-query---read-and-serializes-one-million-rows-from-sql-query)) | norm read ([2](https://github.com/vbilopav/NoOrm.Net/blob/master/PERFOMANCE-TESTS.md#2-norm-read-operation---builds-iterator-over-list-of-name-and-value-tuples)) | norm read ([3](https://github.com/vbilopav/NoOrm.Net/blob/master/PERFOMANCE-TESTS.md#3-norm-read-operation-builds-iterator-over-database-tuples))  | norm read ([4](https://github.com/vbilopav/NoOrm.Net/blob/master/PERFOMANCE-TESTS.md#4-norm-read-operation---builds-iterator-over-namevalue-dictionaries-and-use-it-to-build-iterator-over-testclass-instances)) | norm read ([5](https://github.com/vbilopav/NoOrm.Net/blob/master/PERFOMANCE-TESTS.md#5-norm-read-operation---builds-iterator-over-generic-typed-tuples-and-use-use-it-to-build-iterator-over-testclass-instances)) | norm read ([6](https://github.com/vbilopav/NoOrm.Net/blob/master/PERFOMANCE-TESTS.md#6-norm-read-operation---builds-iterator-over-de-serialized-json-to-class-instances)) | norm read ([7](https://github.com/vbilopav/NoOrm.Net/blob/master/PERFOMANCE-TESTS.md#7-norm-read-operation---builds-iterator-over-class-instances-mapped-with-selectt-or-mapping-extension)) |
| - | --------- | --------  | --------  | --------  | --------  | --------  | --------  |
| 1 | 0:02.907 | 0:00.002 | 0:00.001 | 0:00.001 | 0:00.001 | 0:00.001 | 0:00.001 |
| 2 | 0:02.778 | 0:00.002 | 0:00.001 | 0:00.001 | 0:00.002 | 0:00.001 | 0:00.001 |
| 3 | 0:02.992 | 0:00.002 | 0:00.001 | 0:00.001 | 0:00.001 | 0:00.001 | 0:00.001 |
| 4 | 0:02.765 | 0:00.002 | 0:00.001 | 0:00.001 | 0:00.002 | 0:00.001 | 0:00.001 |
| 5 | 0:02.813 | 0:00.002 | 0:00.001 | 0:00.001 | 0:00.001 | 0:00.001 | 0:00.001 |
| 6 | 0:02.540 | 0:00.001 | 0:00.001 | 0:00.001 | 0:00.002 | 0:00.001 | 0:00.001 |
| 7 | 0:02.813 | 0:00.001 | 0:00.001 | 0:00.001 | 0:00.001 | 0:00.001 | 0:00.001 |
| 8 | 0:02.933 | 0:00.002 | 0:00.001 | 0:00.001 | 0:00.001 | 0:00.001 | 0:00.001 |
| 9 | 0:02.762 | 0:00.001 | 0:00.001 | 0:00.001 | 0:00.001 | 0:00.001 | 0:00.001 |
| 10 | 0:03.282 | 0:00.002 | 0:00.001 | 0:00.001 | 0:00.002 | 0:00.001 | 0:00.001 |
| AVG | **0:02.859** | **0:00.002** | **0:00.001** | **0:00.001** | **0:00.002** | **0:00.001** | **0:00:00.001** |

Following table shows execution times of count operations over enumeration results from same operations.

| | dapper count ([1](https://github.com/vbilopav/NoOrm.Net/blob/master/PERFOMANCE-TESTS.md#1-dapper-query---read-and-serializes-one-million-rows-from-sql-query)) | norm count ([2](https://github.com/vbilopav/NoOrm.Net/blob/master/PERFOMANCE-TESTS.md#2-norm-read-operation---builds-iterator-over-list-of-name-and-value-tuples)) | norm count ([3](https://github.com/vbilopav/NoOrm.Net/blob/master/PERFOMANCE-TESTS.md#3-norm-read-operation-builds-iterator-over-database-tuples)) | norm count ([4](https://github.com/vbilopav/NoOrm.Net/blob/master/PERFOMANCE-TESTS.md#4-norm-read-operation---builds-iterator-over-namevalue-dictionaries-and-use-it-to-build-iterator-over-testclass-instances)) | norm count ([5](https://github.com/vbilopav/NoOrm.Net/blob/master/PERFOMANCE-TESTS.md#5-norm-read-operation---builds-iterator-over-generic-typed-tuples-and-use-use-it-to-build-iterator-over-testclass-instances)) | norm count ([6](https://github.com/vbilopav/NoOrm.Net/blob/master/PERFOMANCE-TESTS.md#6-norm-read-operation---builds-iterator-over-de-serialized-json-to-class-instances)) | norm count ([7](https://github.com/vbilopav/NoOrm.Net/blob/master/PERFOMANCE-TESTS.md#7-norm-read-operation---builds-iterator-over-class-instances-mapped-with-selectt-or-mapping-extension)) |
| - | --------- | --------  | --------  | --------  | --------  | --------  | --------  |
| 1 | 0:00.002 | 0:02.537 | 0:02.391 | 0:02.914 | 0:02.304 | 0:04.219 | 0:03.042 |
| 2 | 0:00.001 | 0:02.319 | 0:02.088 | 0:02.999 | 0:01.999 | 0:03.631 | 0:02.957 |
| 3 | 0:00.001 | 0:02.232 | 0:01.862 | 0:02.488 | 0:02.157 | 0:03.949 | 0:02.719 |
| 4 | 0:00.002 | 0:02.439 | 0:03.111 | 0:03.208 | 0:02.878 | 0:04.799 | 0:03.081 |
| 5 | 0:00.001 | 0:02.216 | 0:02.069 | 0:02.578 | 0:02.179 | 0:03.966 | 0:02.532 |
| 6 | 0:00.001 | 0:02.113 | 0:01.910 | 0:02.541 | 0:02.007 | 0:03.577 | 0:02.506 |
| 7 | 0:00.001 | 0:02.118 | 0:02.201 | 0:03.068 | 0:03.107 | 0:05.965 | 0:03.705 |
| 8 | 0:00.002 | 0:02.596 | 0:02.338 | 0:02.781 | 0:02.483 | 0:04.602 | 0:03.756 |
| 9 | 0:00.002 | 0:02.249 | 0:02.190 | 0:02.780 | 0:02.700 | 0:04.213 | 0:02.866 |
| 10 | 0:00.002 | 0:02.372 | 0:02.229 | 0:02.654 | 0:02.190 | 0:03.911 | 0:03.056 |
| AVG | **0:00.001** | **0:02.319** | **0:02.239** | **0:02.801** | **0:02.400** | **0:04.283** | **0:03.022** |

## 1. Dapper query - read and serializes one million rows from SQL query

```csharp
// Average execution time: 0:02.859
IEnumerable<TestClass> results1 = connection.Query<TestClass>(sql);

// Average execution time: 0:00.001
results1.Count();
```

## 2. Norm read operation - builds iterator over list of name and value tuples

```csharp
// Average execution time: 0:00.002
IEnumerable<IList<(string name, string value)>> results2 = connection.Read(sql);

// Average execution time: 0:02.319
results2.Count();
```

## 3. Norm read operation, builds iterator over database tuples

```csharp
// Average execution time: 0:00.001
IEnumerable<(int, string, string, DateTime)> results3 = connection.Read<int, string, string, DateTime>(sql);

// Average execution time: 0:02.239
results3.Count();
```

## 4. Norm read operation - builds iterator over name/value dictionaries and use it to build iterator over `TestClass` instances

```csharp
// Average execution time: 0:00.001
IEnumerable<TestClass> results4 = connection.Read(sql).SelectDictionaries().Select(dict => new TestClass(dict));

// Average execution time: 0:02.801
results4.Count();
```

## 5. Norm read operation - builds iterator over generic, typed tuples and use use it to build iterator over `TestClass` instances

```csharp
// Average execution time: 0:00.002
IEnumerable<TestClass> results5 = connection.Read<int, string, string, DateTime>(sql).Select(tuple => new TestClass(tuple));

// Average execution time: 0:02.400
results5.Count();
```

## 6. Norm read operation - builds iterator over de-serialized JSON to class instances

```csharp
// Average execution time: 0:00.001
IEnumerable<TestClass> results6 = connection.Json<TestClass>(JsonTestQuery))

// Average execution time: 0:04.283
results5.Count();
```

## 7. Norm read operation - builds iterator over class instances mapped with `Select<T>` O/R mapping extension

```csharp
// Average execution time: 0:00.001
IEnumerable<TestClass> results7 = connection.Read(TestQuery).Select<TestClass>());

// Average execution time: 0:03.022
results7.Count();
```
