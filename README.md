### Overview

This adds vin related transforms to Transformalize using [Vin](https://github.com/dalenewman/Vin).  It is a plug-in compatible with Transformalize 0.3.3-beta.

Build the Autofac project and put it's output into Transformalize's *plugins* folder.

### Usage

```xml
<add name='TestProcess' read-only='false'>
    <entities>
        <add name='TestData'>
            <rows>
                <add vin='11111111111111111' />
                <add vin='2LMDJ6JK0FBL33226' />
                <add vin='JM1BK323071624570' />
            </rows>
            <fields>
                <add name='vin' primary-key='true' />
            </fields>
            <calculated-fields>
                <add name='valid' type='bool' t='copy(vin).vinisvalid()' />
                <add name='year' type='short' t='copy(vin).vingetmodelyear()' />
                <add name='make' t='copy(vin).vingetworldmanufacturer()' />
            </calculated-fields>
        </add>
    </entities>

</add>
```

This produces: 
<pre>
<strong>vin,valid,year,make</strong>
11111111111111111,true,2001,
2LMDJ6JK0FBL33226,true,2015,Lincoln
JM1BK323071624570,false,2007,Mazda
</pre>

### Benchmark

``` ini
BenchmarkDotNet=v0.10.12, OS=Windows 10 Redstone 3 [1709, Fall Creators Update] (10.0.16299.125)
Intel Core i7-7700HQ CPU 2.80GHz (Kaby Lake), 1 CPU, 8 logical cores and 4 physical cores
Frequency=2742188 Hz, Resolution=364.6723 ns, Timer=TSC
  [Host]       : .NET Framework 4.6.2 (CLR 4.0.30319.42000), 32bit LegacyJIT-v4.7.2600.0
  LegacyJitX64 : .NET Framework 4.6.2 (CLR 4.0.30319.42000), 64bit LegacyJIT/clrjit-v4.7.2600.0;compatjit-v4.7.2600.0
  LegacyJitX86 : .NET Framework 4.6.2 (CLR 4.0.30319.42000), 32bit LegacyJIT-v4.7.2600.0

Jit=LegacyJit  Runtime=Clr  
```
|                    Method |          Job | Platform |     Mean |     Error |    StdDev | Scaled | ScaledSD |
|-------------------------- |------------- |--------- |---------:|----------:|----------:|-------:|---------:|
|             &#39;3 test rows&#39; | LegacyJitX64 |      X64 | 1.572 ms | 0.0146 ms | 0.0136 ms |   1.00 |     0.00 |
| &#39;3 rows using transforms&#39; | LegacyJitX64 |      X64 | 2.125 ms | 0.0334 ms | 0.0313 ms |   1.35 |     0.02 |
|                           |              |          |          |           |           |        |          |
|             &#39;3 test rows&#39; | LegacyJitX86 |      X86 | 1.488 ms | 0.0153 ms | 0.0144 ms |   1.00 |     0.00 |
| &#39;3 rows using transforms&#39; | LegacyJitX86 |      X86 | 1.983 ms | 0.0231 ms | 0.0216 ms |   1.33 |     0.02 |
