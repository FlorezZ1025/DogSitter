namespace UDEM.DEVOPS.DogSitter.Api.Tests;

[CollectionDefinition("CuidadorApiCollection")]
public class CuidadorApiCollection : ICollectionFixture<DogSitterApiApp> { }

[CollectionDefinition("RazaApiCollection")]
public class RazaApiCollection : ICollectionFixture<DogSitterApiApp> { }

[CollectionDefinition("PerroApiCollection")]
public class PerroApiCollection : ICollectionFixture<DogSitterApiApp> { }