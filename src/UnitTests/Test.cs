#region license
// Transformalize
// Configurable Extract, Transform, and Load
// Copyright 2013-2017 Dale Newman
//  
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//   
//       http://www.apache.org/licenses/LICENSE-2.0
//   
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

using System.Linq;
using Autofac;
using BootStrapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Transformalize.Configuration;
using Transformalize.Contracts;
using Transformalize.Providers.Console;

namespace UnitTests {

    [TestClass]
    public class Test {

        [TestMethod]
        public void BasicTests() {

            var xml = $@"
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

</add>";
            using (var outer = new ConfigurationContainer().CreateScope(xml)) {
                using (var inner = new TestContainer().CreateScope(outer, new ConsoleLogger(LogLevel.Debug))) {

                    var process = inner.Resolve<Process>();
                  
                    var controller = inner.Resolve<IProcessController>();
                    controller.Execute();
                    var rows = process.Entities.First().Rows;

                    Assert.AreEqual(true, rows[0]["valid"]);
                    Assert.AreEqual(true, rows[1]["valid"]);
                    Assert.AreEqual(false, rows[2]["valid"]);

                    Assert.AreEqual((short)2001, rows[0]["year"]);
                    Assert.AreEqual((short)2015, rows[1]["year"]);
                    Assert.AreEqual((short)2007, rows[2]["year"]);

                    Assert.AreEqual("", rows[0]["make"]);
                    Assert.AreEqual("Lincoln", rows[1]["make"]);
                    Assert.AreEqual("Mazda", rows[2]["make"]);
                   
                }
            }



        }
    }
}
