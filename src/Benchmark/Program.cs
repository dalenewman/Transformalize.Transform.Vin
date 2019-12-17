using Autofac;
using Transformalize.Contracts;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Jobs;
using BenchmarkDotNet.Running;
using Transformalize.Logging;
using Transformalize.Containers.Autofac;
using Transformalize.Configuration;
using Transformalize.Providers.Console;

namespace Benchmark {


   [LegacyJitX64Job, LegacyJitX86Job]
   public class Benchmarks {

      [Benchmark(Baseline = true, Description = "3 test rows")]
      public void TestRows() {
         var logger = new ConsoleLogger();
         using (var outer = new ConfigurationContainer().CreateScope(@"files\bogus.xml", logger)) {
            var process = outer.Resolve<Process>();
            using (var inner = new Container().CreateScope(process, logger)) {
               var controller = inner.Resolve<IProcessController>();
               controller.Execute();
            }
         }
      }

      [Benchmark(Baseline = false, Description = "3 rows using transforms")]
      public void CSharpRows() {
         var logger = new ConsoleLogger();
         using (var outer = new ConfigurationContainer().CreateScope(@"files\bogus-with-transform.xml", logger)) {
            var process = outer.Resolve<Process>();
            using (var inner = new Container().CreateScope(process, logger)) {
               var controller = inner.Resolve<IProcessController>();
               controller.Execute();
            }
         }
      }

   }

   public class Program {
      private static void Main(string[] args) {
         var summary = BenchmarkRunner.Run<Benchmarks>();
      }
   }
}
