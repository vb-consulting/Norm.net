# PERFOMANCE BENCHMARKS
 
- You can run manual tests by building and running this [project](https://github.com/vb-consulting/Norm.net/tree/master/BenchmarksConsole).
 
- Query used for testing returns **1 million records** from local test PostgreSQL server, **10 fields** of various types wide. 
 
- Tests for Dapper and Norm will serialize to one **class** and one **record** structure.
 
- Tests for Norm will also do serialization of **tuples** tuple values, named tuples. Since it is the only library that supports that.
 
- **Raw data reader** performance tests are included for comparison.

## Test 1
 
### Dapper Buffered Query Class and Record Tests

These tests measure the Dapper **buffered query** (default behavior) performance for **record** and **class**.
 
The buffered query will trigger the iteration immediately and return the already serialized list.
 
- **`Query<class>`** averages in **03,87 seconds** with average memory consuption of **316523 KB**
 
- **`Query<record>`** averages in **03,86 seconds** with average memory consuption of **316684 KB**
 
Conclusion: 

- As expected **results are similar**, there are no significant differences between those two executions.

## Test 2
 
### Dapper Unbuffered Query Class and Record Tests

These tests measure Dapper **unbuffered query** performances for **record** and **class**.

Unbuffered queries in Dapper (with parameter ` buffered: false`) do not return a list, 
but rather **generate enumerator that yields results**, similar to Norm default and only behavior.

The resulting enumerator is returned very fast, but, it is still not a list as the first test result. 

To emulate the same type of results, we must convert generated enumeration to a list with **`ToList` LINQ** call.

- Unbuffered **`Query<class>`** averages in **03.81 seconds** with average memory consuption of **316623 KB**

- Unbuffered **`Query<record>`** averages in **03.88 seconds** with average memory consuption of **316726 KB**
 
Conclusion: 

- As expected **results are similar**, there are no significant differences between those two executions. 

- There is also **no difference between buffered and unbuffered** versions. Both versions are doing exactly one iteration to generate a return list that consumes most of the resources.

## Test 3

### Norm Read Class and Record Tests

These tests measure Norm read performances for **record** and **class**.
 
Norm default and only behavior are the same as Dapper unbuffered behavior (with parameter ` buffered: false`).

It does not return a list,  but rather **generate an enumerator that yields results**.

The resulting enumerator is returned very fast, but, it is still not a list, therefore, results must be converted to a list with `ToList` LINQ call to emulate in previous tests.
 
- `Read<class>` averages in **03.48 seconds** with average memory consuption of **317415 KB**
 
- `Read<record>` averages in **03.42 seconds** with average memory consuption of **316394 KB**

Conclusion: 
 
- Norm serialization to list is **slightly faster**

- **03.48 seconds** vs **03,87 seconds** and **03,81 seconds** for class tests

- **03.42 seconds** vs **03,86 seconds**  and **03.88 seconds** for record tests

- Memory consumption is virtually the same in all cases, there is no difference here.

## Test 4

### Norm Read Values and Tuples vs Raw Data Reader Tests

These tests measure different types of Norm read functionality, currently not present in Dapper:
 
1) Values `connection.Read<int, string, string, DateTime, int, string, string, DateTime, string, bool>(query).ToList()`:
 
Instead of class or a record, we can use actual resulting types in type parameters. This **returns tuple** with these types.
 
2) Tuples: `connection.Read<(int id1, string foo1, string bar1, DateTime datetime1, int id2, string foo2, string bar2, DateTime datetime2, string longFooBar, bool isFooBar)>(query).ToList();`
 
Using a named tuple as a generic parameter, that, again, returns the **named tuple** as result.
 
There is also a **raw data reader** for comparison that iterates the data reader and populates the list with results.
 
- `Read<values>` averages in **03.45 seconds** with average memory consuption of **295535 KB**
 
- `Read<tuple>` averages in **03.82 seconds** with average memory consuption of **298520 KB**
 
- Raw data reader averages in **03.41 seconds** with average memory consumption of **315778 KB**

Conclusion:
 
- Norm Read operation now has **slightly lower memory consumption.** This might be because it Norm in these tests doesn't create a list of instances but a list of tuple values.
 
- Norm `Read<values>` is faster than Dapper serialization to instances and roughly the same as Norm serialization to instances.

- Norm `Read<tuple>` is slightly slower, because of the complicated way how the long tuples are created. Still, performing Similiar as Dapper class serialization.

- Raw data reader performances are similar to Norm performances (except for tuples serialization). 

- Memory consumption is similar to other tests where serialization to instances was used.

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
 
- Dapper Unbuffered Query operation averages in **00.0000076 seconds**, iteration in **03.44 seconds** with total in **03.44 seconds**.
 
- Norm Read operation averages in **00.0000117 seconds**, iteration in **03.40 seconds** with total in **03.40 seconds**.
 
Conclusion:

- In this tests, Dapper Unbuffered is slightly faster than the Dapper Buffered version. It is the same as Norm Read. This is a bit surprising because previous Buffered vs Unbuffered tests didn't show that.
 
- Buffered iteration step is fast, but not as nearly as fast as Dapper Unbuffered and Norm Read enumerator generation. However, values are still small and if we would add some actual work in iteration proportional difference would be even smaller until it fades in irrelevance.

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
