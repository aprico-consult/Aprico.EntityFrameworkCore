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

using System;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NodaTime;
using NodaTime.Extensions;

namespace Aprico.EntityFrameworkCore.Storage.ValueConversion;

// @formatter:wrap_chained_method_calls chop_if_long
public class InstantToDateTimeConverter(ConverterMappingHints? mappingHints = null) : ValueConverter<Instant?, DateTime?>(
	static instant => instant.HasValue
		? instant.Value.ToDateTimeUtc()
		: null,
	static dateTime => dateTime.HasValue
		? DateTime.SpecifyKind(dateTime.Value, DateTimeKind.Utc).ToInstant()
		: null,
	mappingHints);
