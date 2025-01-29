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

using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace Aprico.Xunit;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed class MsSqlDbContextFixture<TContext>(IMessageSink messageSink) : IAsyncLifetime
	where TContext : DbContext, ICreateDbContext<TContext>
{
	#region IAsyncLifetime Members

	public async Task InitializeAsync()
	{
		await ((IAsyncLifetime) _msSqlContainerFixture).InitializeAsync();
		DbContext = TContext.Create(_msSqlContainerFixture.Container.GetConnectionString());
		await DbContext.Database.EnsureDeletedAsync();
		await DbContext.Database.EnsureCreatedAsync();
	}

	public async Task DisposeAsync()
	{
		await DbContext.DisposeAsync();
		await ((IAsyncLifetime) _msSqlContainerFixture).DisposeAsync();
	}

	#endregion

	[SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
	internal TContext DbContext { get; private set; } = null!;

	private readonly MsSqlContainerFixture _msSqlContainerFixture = new(messageSink);
}
