#region region Copyright & License

// Copyright © 2024 - 2025 Aprico Consultants
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#endregion

using System.Data.Common;
using Microsoft.Data.SqlClient;
using Testcontainers.MsSql;
using Testcontainers.Xunit;
using Xunit.Abstractions;

namespace Aprico.Xunit;

internal sealed class MsSqlContainerFixture(IMessageSink messageSink) : DbContainerFixture<MsSqlBuilder, MsSqlContainer>(messageSink)
{
	#region Base Class Member Overrides

	protected override MsSqlBuilder Configure(MsSqlBuilder builder)
	{
		return builder.WithImage("mcr.microsoft.com/mssql/server:2022-latest")
			.WithName("aprico-lib-mssql-test-container");
	}

	#endregion

	#region Base Class Member Overrides

	public override DbProviderFactory DbProviderFactory => SqlClientFactory.Instance;

	#endregion
}
