// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IoC.cs" company="Web Advanced">
// Copyright 2012 Web Advanced (www.webadvanced.com)
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


using MagicBus.Example.Web.Domain.CustomerDetails.Handlers;
using StructureMap;
using StructureMap.Graph;
namespace MagicBus.Example.Web.DependencyResolution {
    public static class IoC {
        public static IContainer Initialize() {
            ObjectFactory.Initialize(x =>
                        {
                            x.Scan(scan =>
                                    {
                                        scan.TheCallingAssembly();
                                        scan.WithDefaultConventions();
                                    });

							x.For<IBus>().Use<Bus>();
							x.Scan(scanner =>
							{
								scanner.AssemblyContainingType<Bus>();
								scanner.WithDefaultConventions();
							});

							x.Scan(scanner =>
							{
								scanner.AssemblyContainingType<CreateCustomerHandler>();
								scanner.WithDefaultConventions();
								scanner.ConnectImplementationsToTypesClosing(typeof(IHandler<>));
							});

							x.For<SingleInstanceFactory>().Use<SingleInstanceFactory>(ctx => t => ctx.GetInstance(t));
							x.For<MultiInstanceFactory>().Use<MultiInstanceFactory>(ctx => t => ctx.GetAllInstances(t));
                        });
            return ObjectFactory.Container;
        }
    }
}