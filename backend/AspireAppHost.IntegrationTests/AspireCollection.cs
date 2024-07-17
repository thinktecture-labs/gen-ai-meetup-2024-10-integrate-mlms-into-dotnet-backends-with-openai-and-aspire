using Xunit;

namespace AspireAppHost.IntegrationTests;

[CollectionDefinition(nameof(AspireCollection))]
public sealed class AspireCollection : ICollectionFixture<AspireFixture>;