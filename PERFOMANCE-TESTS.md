# PERFOMANCE BENCHMARKS
 
- You can run manual tests by building and running this [project](https://github.com/vb-consulting/Norm.net/tree/master/BenchmarksConsole).
 
- Query used for testing returns **1 million records** from local test PostgreSQL server, **10 fields** of various types wide. 
 
- Tests for Dapper and Norm will serialize to one **class** and one **record** structure.
 
- Tests for Norm will also do serialization of **tuples** tuple values, named tuples. Since it is the only library that supports that.
 
- **Raw data reader** performance tests are included for comparison.

## Aggregated Results

|Operation|Average Time in Sec.|Average Memory Consuption in KB|
|---------|-----------------------|----------------------------|
|Buffered Dapper `Query<class>`|03,87|316523|
|Buffered Dapper `Query<record>`|03,86|316684|
|Buffered Dapper `QueryAsync<class>`|03,81|332336|
|Buffered Dapper `QueryAsync<record>`|03,85|316922|
|Buffered Dapper `Query<class>`|03,81|316623|
|Buffered Dapper `Query<record>`|03,88|316726|
|Buffered Dapper `QueryAsync<class>`|Not Available|Not Available|
|Buffered Dapper `QueryAsync<record>`|Not Available|Not Available|
|Norm `Read<class>`|03,48|317415|
|Norm `Read<record>`|03,42|316394|
|Norm `ReadAsync<record>`|03,48|315927|
|Norm `ReadAsync<record>`|03,49|315805|
|Norm `Read<built-in types>`|03,45|295535|
|Norm `Read<tuple>`|03,82|298520|
|Raw Data Reader|03,41|315778|
|Norm `ReadAsync<built-in types>`|03,88|295190|
|Norm `ReadAsync<tuple>`|03,96|297957|
|Raw Data Reader Async|03,35|411875|


|Operation|Execution Sec.|Iteration Sec.|Total Sec.|Average Memory Consuption in KB|
|---------|--------------|--------------|----------|-------------------------------|
|Buffered Dapper `Query`|03.92|00.01|03.93|Not Measured|
|Unbuffered Dapper `Query`|00.0000076|03.44|03.44|Not Measured|
|Norm `Read`|00.0000117|03.40|03.40|Not Measured|
|Buffered Dapper `QueryAsync`|03.84|00.01|03.85|317566|
|Unbuffered Dapper `QueryAsync`|Not Available|Not Available|Not Available|Not Available|
|Norm `ReadAsync`|00.0003567|03.20|03.20|315642|

## Test 1
 
### Dapper Buffered Query Class and Record Tests

These tests measure the Dapper **buffered query** (default behavior) performance for **record** and **class**.
 
The buffered query will trigger the iteration immediately and return the already serialized list.
 
- **`Query<class>`** averages in **03,87 seconds** with average memory consuption of **316523 KB**
 
- **`Query<record>`** averages in **03,86 seconds** with average memory consuption of **316684 KB**

- **`QueryAsync<class>`** averages in **03,81 seconds** with average memory consuption of **332336 KB**
 
- **`QueryAsync<record>`** averages in **03,85 seconds** with average memory consuption of **316922 KB**
 
Conclusion: 

- As expected **results are similar**, there are no significant differences between those two executions.

- Also, there is no differnce between sync and async version.

## Test 2
 
### Dapper Unbuffered Query Class and Record Tests

These tests measure Dapper **unbuffered query** performances for **record** and **class**.

Unbuffered queries in Dapper (with parameter ` buffered: false`) do not return a list, 
but rather **generate enumerator that yields results**, similar to Norm default and only behavior.

The resulting enumerator is returned very fast, but, it is still not a list as the first test result. 

To emulate the same type of results, we must convert generated enumeration to a list with **`ToList` LINQ** call.

- Unbuffered **`Query<class>`** averages in **03.81 seconds** with average memory consuption of **316623 KB**

- Unbuffered **`Query<record>`** averages in **03.88 seconds** with average memory consuption of **316726 KB**

- Unbuffered **`QueryAsync`** version is not supported in Dapper.
 
Conclusion: 

- As expected **results are similar**, there are no significant differences between those two executions. 

- There is also **no difference between buffered and unbuffered** versions. Both versions are doing exactly one iteration to generate a return list that consumes most of the resources.

- Async version is not supported.

## Test 3

### Norm Read Class and Record Tests

These tests measure Norm read performances for **record** and **class**.
 
Norm default and only behavior are the same as Dapper unbuffered behavior (with parameter ` buffered: false`).

It does not return a list,  but rather **generate an enumerator that yields results**.

The resulting enumerator is returned very fast, but, it is still not a list, therefore, results must be converted to a list with `ToList` LINQ call to emulate in previous tests.
 
- `Read<class>` averages in **03.48 seconds** with average memory consuption of **317415 KB**
 
- `Read<record>` averages in **03.42 seconds** with average memory consuption of **316394 KB**

- `ReadAsync<class>` averages in **03.48 seconds** with average memory consuption of **315927 KB**

- `ReadAsync<record>` averages in **03.49 seconds** with average memory consuption of **315805 KB**
  
Conclusion: 
 
- Norm serialization to list is **slightly faster**

- **03.48 seconds** vs **03,87 seconds** and **03,81 seconds** for class tests

- **03.42 seconds** vs **03,86 seconds**  and **03.88 seconds** for record tests

- **03.48 seconds** vs **03,81 seconds** (unbuffered async unsupported in Dapper). 

- **03.49 seconds** vs **03,85 seconds** (unbuffered async unsupported in Dapper).

- Memory consumption is virtually the same in all cases, there is no difference here.

## Test 4

### Norm Read Values and Tuples vs Raw Data Reader Tests

These tests measure different types of Norm read functionality, currently not present in Dapper:
 
1) Values (built-in types) `connection.Read<int, string, string, DateTime, int, string, string, DateTime, string, bool>(query).ToList()`:
 
Instead of class or a record, we can use actual resulting types in type parameters. This **returns tuple** with these types.
 
2) Tuples: `connection.Read<(int id1, string foo1, string bar1, DateTime datetime1, int id2, string foo2, string bar2, DateTime datetime2, string longFooBar, bool isFooBar)>(query).ToList();`
 
Using a named tuple as a generic parameter, that, again, returns the **named tuple** as result.
 
There is also a **raw data reader** for comparison that iterates the data reader and populates the list with results.
 
- `Read<built-in types>` averages in **03.45 seconds** with average memory consuption of **295535 KB**
 
- `Read<tuple>` averages in **03.82 seconds** with average memory consuption of **298520 KB**
 
- Raw data reader averages in **03.41 seconds** with average memory consumption of **315778 KB**

- `ReadAsync<built-in types>` averages in **03.88 seconds** with average memory consuption of **295190 KB**
 
- `ReadAsync<tuple>` averages in **03.96 seconds** with average memory consuption of **297957 KB**
 
- Raw data reader async averages in **03.35 seconds** with average memory consumption of **411875 KB**

Conclusion:
 
- Norm Read operation now has **slightly lower memory consumption.** This might be because it Norm in these tests doesn't create a list of instances but a list of tuple values.
 
- Norm `Read<built-in types>` is faster than Dapper serialization to instances and roughly the same as Norm serialization to instances.

- Norm `Read<tuple>` is slightly slower, because of the complicated way how the long tuples are created. Still, performing Similiar as Dapper class serialization.

- Async versions `ReadAsync<built-in types>` and `ReadAsync<tuples>` are slightly slower and they are the same as Dappers.

- Raw data reader performances are similar to Norm performances (except for tuples serialization). 

- Raw data reader async appears to be fastest by small margin but with unusually high memory consuption (around one third higher).

- Other memory consumptions is similar to other tests where serialization to instances was used.

## Test 5

### Dapper Query and Iteration (Buffered and Unbuffered) vs Norm Read and Iteration Tests

This set of tests will emulate a typical application scenario in two steps:
 
1) Get the data with the read or query command. This step is usually performed in the Data Access Layer (DAL).
 
2) Iterate through the result set fetched with the read or the query command in the previous step. This step is usually performed in the service layer or UI layer where you would typically render the UI or generate service results.
 
These tests also demonstrate the difference in execution between Dapper buffered execution (default) vs Dapper unbuffered and Norm read execution.
 
Dapper buffered execution (default) will iterate the result set two times: 
 
1) First time on the first step, to generate the resulting list
 
2) Second in the second step, which is actual data iteration
 
This should give an advantage in performances to Dapper unbuffered and Norm read executions.
 
- Dapper Buffered Query operation averages in **03.92 seconds**, iteration in **00.01 seconds** with total in **03.93 seconds**.

- Dapper Buffered QueryAsync operation averages in **03.84 seconds**, iteration in **00.01 seconds** with total in **03.85 seconds**.
 
- Dapper Unbuffered Query operation averages in **00.0000076 seconds**, iteration in **03.44 seconds** with total in **03.44 seconds**.

- Dapper Buffered QueryAsync is not supported.
 
- Norm Read operation averages in **00.0000117 seconds**, iteration in **03.40 seconds** with total in **03.40 seconds**.

- Norm Read Async operation averages in **00.0003567 seconds**, iteration in **03.20 seconds** with total in **03.20 seconds**.
 
Conclusion:

- In these tests, Dapper Unbuffered is slightly faster than the Dapper Buffered version. It is the same as Norm Read. This is a bit surprising because previous Buffered vs Unbuffered tests didn't show that.
 
- Buffered iteration step is fast, but not as nearly as fast as Dapper Unbuffered and Norm Read enumerator generation. However, values are still small and if we would add some actual work in iteration proportional difference would be even smaller until it fades in irrelevance.

- Unbuffered Dapper Async is not supported, and Norm Async is same as Dapper Unbuffered and faster then Dapper Buffered.

## Test Run Outputs

### Test 1: Dapper Buffered Query Class and Record Tests

|#|Class Elapsed Sec|Class Allocated KB|Record Elapsed Sec|Record Allocated KB|
|-|-----------------|------------------|------------------|-------------------|
|1|04.3551435|315822,03125|03.8481814|316452,71875|
|2|03.9158743|316532,375|03.9516047|316678|
|3|03.7933986|316596,65625|03.7770713|316714,21875|
|4|03.8546169|316605,0625|03.8060569|316718,28125|
|5|03.6912606|316608,96875|03.8742957|316714,3125|
|6|03.7836010|316609,125|03.7913233|316714,21875|
|7|03.8511723|316617|03.7922655|316714,375|
|8|03.8261262|316613,125|03.8048938|316710,25|
|9|03.8032572|316609,0625|03.8469629|316710,34375|
|10|03.8356530|316612,6875|04.1084095|316714,21875|
|**AVG**|**03.8710103**|**316522,625**|**03.8601065**|**316684,09375**|

### Test 2: Dapper Unbuffered Query Class and Record Tests

|#|Class Elapsed Sec|Class Allocated KB|Record Elapsed Sec|Record Allocated KB|
|-|-----------------|------------------|------------------|-------------------|
|1|03.8763397|316620,75|03.8088003|316722,34375|
|2|03.7857128|316613|03.9138215|316714,1875|
|3|03.7760241|316612,96875|03.9169103|316726,34375|
|4|03.8084005|316620,75|03.9407015|316722,3125|
|5|03.7284074|316624,65625|03.9023819|316722,21875|
|6|03.7696535|316628,65625|03.8010464|316734,28125|
|7|03.7638654|316628,90625|03.8417820|316734|
|8|03.7722935|316632,75|03.8738278|316730,15625|
|9|04.0929321|316625,0625|03.9226825|316730,3125|
|10|03.7519075|316628,8125|03.9124436|316726,21875|
|**AVG**|**03.8125536**|**316623,625**|**03.8834397**|**316726,25**|

### Test 3: Norm Read Class and Record Tests

|#|Class Elapsed Sec|Class Allocated KB|Record Elapsed Sec|Record Allocated KB|
|-|-----------------|------------------|------------------|-------------------|
|1|03.4378821|319466,15625|03.4369100|315152,4375|
|2|03.3386022|318091,46875|03.4542863|315782,125|
|3|03.4450170|315090,4375|03.4136544|318070,40625|
|4|03.4415087|319712,5625|03.3664374|315005,03125|
|5|03.5266046|314816,1875|03.3918234|318054,28125|
|6|03.4394083|319681,25|03.3998279|315133,5625|
|7|03.4295879|318014,71875|03.4527482|315594,3125|
|8|03.8369312|314890,8125|03.5015365|318000,4375|
|9|03.4442191|319624,5625|03.4464827|315164,0625|
|10|03.4863456|314760,9375|03.3555425|317990,4375|
|**AVG**|**03.4826106**|**317414,90625**|**03.4219249**|**316394,71875**|

### Test 4: Norm Read Values and Tuples vs Raw Data Reader Tests

|#|Values Elapsed Sec|Values Allocated KB|Tuples Elapsed Sec|Tuples Allocated KB|Raw Reader Elapsed Sec|Raw Reader Allocated KB|
|-|------------------|-------------------|------------------|-------------------|----------------------|-----------------------|
|1|03.4647316|295725,65625|03.8299178|298619,15625|03.4334312|315798,34375|
|2|03.4491284|295531,6875|03.7554375|298644,4375|03.4056276|315802,71875|
|3|03.4305626|295532,375|03.9020512|298474,0625|03.4216978|315787,6875|
|4|03.3663587|295517,5625|03.7420979|298455,46875|03.4311730|315790,375|
|5|03.6447902|295525,65625|04.0520082|298639,4375|03.4735854|315809,46875|
|6|03.4686309|295535,875|03.7608538|298563,5|03.4166460|315797,28125|
|7|03.4436824|295530,9375|03.7199345|298404,5625|03.4085614|315746,1875|
|8|03.4172634|295479,4375|03.7602589|298560,78125|03.3557720|315754,0625|
|9|03.3761650|295486,53125|03.8912301|298458,25|03.3642760|315756,1875|
|10|03.4087460|295480,125|03.8027448|298382,15625|03.4019519|315738,25|
|**AVG**|**03.4470059**|**295534,59375**|**03.8216534**|**298520,1875**|**03.4112722**|**315778,0625**|

### Dapper Query and Iteration (Buffered and Unbuffered) vs Norm Read and Iteration Tests

|#|Dapper Buffered Query|Dapper Buffered Iteration|Daper Buffered Total|Dapper Unbuffered Query|Dapper Unbuffered Iteration|Daper Unbuffered Total|Norm Read|Norm Iteration|Norm Total|
|-|---------------------|-------------------------|--------------------|-----------------------|---------------------------|----------------------|---------|--------------|----------|
|1|04.0194343|00.0100554|04.0294897|00.0000042|03.6281924|03.6281966|00.0000678|03.3713733|03.3714411|
|2|03.9522461|00.0129960|03.9652421|00.0000218|03.3593254|03.3593472|00.0000049|03.4322681|03.4322730|
|3|03.8694808|00.0103090|03.8797898|00.0000044|03.3198512|03.3198556|00.0000048|03.4333911|03.4333959|
|4|03.8975773|00.0098227|03.9074000|00.0000042|03.4170909|03.4170951|00.0000053|03.3037566|03.3037619|
|5|03.9607433|00.0098192|03.9705625|00.0000035|03.3932201|03.3932236|00.0000051|03.4006465|03.4006516|
|6|03.8312938|00.0097832|03.8410770|00.0000214|03.4004915|03.4005129|00.0000058|03.4435005|03.4435063|
|7|04.1863949|00.0109422|04.1973371|00.0000052|03.7033899|03.7033951|00.0000056|03.4464947|03.4465003|
|8|03.8750935|00.0097846|03.8848781|00.0000035|03.4337888|03.4337923|00.0000080|03.3967465|03.3967545|
|9|03.8290565|00.0090333|03.8380898|00.0000035|03.4008843|03.4008878|00.0000051|03.3945239|03.3945290|
|10|03.8103166|00.0098211|03.8201377|00.0000043|03.3881201|03.3881244|00.0000049|03.3899330|03.3899379|
|**AVG**|**03.9231637**|**00.0102366**|**03.9334003**|**00.0000076**|**03.4444354**|**03.4444430**|**00.0000117**|**03.4012634**|**03.4012751**|

## Async Tests Outputs

## Dapper Buffered Query Class and Record Async Tests

|#|Class Elapsed Sec|Class Allocated KB|Record Elapsed Sec|Record Allocated KB|
|-|-----------------|------------------|------------------|-------------------|
|1|00:00:03.8893827|479045,84375|00:00:03.6552850|315984,53125|
|2|00:00:03.5857168|315933,78125|00:00:03.7994569|315971,5|
|3|00:00:03.7164507|315932,5|00:00:03.7309075|315971,4375|
|4|00:00:03.7878422|315857,75|00:00:03.6802020|315957,71875|
|5|00:00:03.8093724|315941,625|00:00:03.9992026|316498,125|
|6|00:00:03.7755876|315918,65625|00:00:04.0282673|319977,28125|
|7|00:00:03.9067539|315940,4375|00:00:03.9522494|316501,4375|
|8|00:00:03.9091274|315931,09375|00:00:03.8735317|315873,8125|
|9|00:00:03.9571722|316922,875|00:00:03.9445057|316507,75|
|10|00:00:03.7992153|315930,625|00:00:03.8492803|319980,78125|
|**AVG**|**00:00:03.8136621**|**332335,53125**|**00:00:03.8512888**|**316922,4375**|

## Dapper Unbuffered Query Class and Record Async Tests

Unsupported.

## Norm Read Class and Record Async Tests

|#|Class Elapsed Sec|Class Allocated KB|Record Elapsed Sec|Record Allocated KB|
|-|-----------------|------------------|------------------|-------------------|
|1|00:00:03.4456945|316495,8125|00:00:03.5193685|316243,96875|
|2|00:00:03.6113696|316288,5625|00:00:03.2919276|315866|
|3|00:00:03.6135184|316348,65625|00:00:03.3879824|315613,46875|
|4|00:00:03.4414767|315379,875|00:00:03.4361594|315819,9375|
|5|00:00:03.5314651|315752,5625|00:00:03.5261258|315529,9375|
|6|00:00:03.5767883|315782,09375|00:00:03.4300437|315686,875|
|7|00:00:03.3635103|315718,59375|00:00:03.6113887|316114,375|
|8|00:00:03.3538976|315698,875|00:00:03.3770096|315806,9375|
|9|00:00:03.3517790|315700,75|00:00:03.9268139|315677,71875|
|10|00:00:03.5524809|316099,8125|00:00:03.3685393|315687,21875|
|**AVG**|**00:00:03.4841980**|**315926,5625**|**00:00:03.4875358**|**315804,65625**|

## Norm Read Values and Tuples vs Raw Data Reader Async Tests

|#|Values Elapsed Sec|Values Allocated KB|Tuples Elapsed Sec|Tuples Allocated KB|Raw Reader Elapsed Sec|Raw Reader Allocated KB|
|-|------------------|-------------------|------------------|-------------------|----------------------|-----------------------|
|1|00:00:03.8857619|294843,46875|00:00:04.0364624|297986,28125|00:00:03.2808635|438010,125|
|2|00:00:03.8230726|295442,875|00:00:04.2547485|297961,5625|00:00:03.5681573|440755,8125|
|3|00:00:03.9360897|295510,8125|00:00:03.8958301|297790,5|00:00:03.3733968|428613,6875|
|4|00:00:03.8789857|295111,3125|00:00:03.8797397|297987,6875|00:00:03.3247153|314770,875|
|5|00:00:03.8228497|295124,53125|00:00:03.9585140|298069,5625|00:00:03.3118912|433585,25|
|6|00:00:03.8502468|295173,0625|00:00:03.8726656|298013,875|00:00:03.2931011|431839,78125|
|7|00:00:03.9492621|295173,46875|00:00:03.8491664|298008,375|00:00:03.3289791|438236,09375|
|8|00:00:03.9286667|295085,21875|00:00:04.0542113|298014,0625|00:00:03.3294143|440311,4375|
|9|00:00:03.8894652|295531,5625|00:00:03.8762145|297816,3125|00:00:03.3242860|437828,84375|
|10|00:00:03.8474220|294903,75|00:00:03.9099232|297918,84375|00:00:03.3709866|314784,59375|
|**AVG**|**00:00:03.8811822**|**295190**|**00:00:03.9587475**|**297956,71875**|**00:00:03.3505791**|**411873,65625**|

## Dapper Buffered Query and Iteration vs Norm Read and Iteration Async Tests

|#|Dapper Buffered Query|Dapper Buffered Iteration|Daper Buffered Total|Dapper Buffered Allocated KB|Norm Read|Norm Iteration|Norm Total|Norm Allocated KB|
|-|---------------------|-------------------------|--------------------|----------------------------|---------|--------------|----------|-----------------|
|1|00:00:03.8236503|00:00:00.0095296|00:00:03.8331799|319181,25|00:00:00.0033425|00:00:03.1587000|00:00:03.1620425|315630,78125|
|2|00:00:03.9058993|00:00:00.0097007|00:00:03.9156000|320981,8125|00:00:00.0000530|00:00:03.0898322|00:00:03.0898852|315631,25|
|3|00:00:03.8312420|00:00:00.0093620|00:00:03.8406040|316905,875|00:00:00.0000149|00:00:03.0917059|00:00:03.0917208|315639,59375|
|4|00:00:03.6923733|00:00:00.0096969|00:00:03.7020702|316965|00:00:00.0000788|00:00:03.3937753|00:00:03.3938541|315634,1875|
|5|00:00:03.9104662|00:00:00.0095077|00:00:03.9199739|316889,75|00:00:00.0000136|00:00:03.2199033|00:00:03.2199169|315641,96875|
|6|00:00:03.9745393|00:00:00.0092819|00:00:03.9838212|317018,875|00:00:00.0000262|00:00:03.2137809|00:00:03.2138071|315651,5|
|7|00:00:03.7513513|00:00:00.0095772|00:00:03.7609285|316944,625|00:00:00.0000090|00:00:03.1819506|00:00:03.1819596|315650,6875|
|8|00:00:03.9221105|00:00:00.0093874|00:00:03.9314979|316960,5|00:00:00.0000093|00:00:03.2503355|00:00:03.2503448|315647,75|
|9|00:00:03.6956532|00:00:00.0104931|00:00:03.7061463|316944,125|00:00:00.0000086|00:00:03.2223068|00:00:03.2223154|315645,84375|
|10|00:00:03.9203320|00:00:00.0095580|00:00:03.9298900|316883,875|00:00:00.0000117|00:00:03.1968850|00:00:03.1968967|315643,9375|
|**AVG**|**00:00:03.8427617**|**00:00:00.0096094**|**00:00:03.8523711**|**317567,5625**|**00:00:00.0003567**|**00:00:03.2019175**|**00:00:03.2022743**|**315641,75**|

