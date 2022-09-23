using Bogus;

namespace UnicornValley.WebAPI.SeedData;

public static class FakerExtensions
{
    public static Faker<T> UsePrivateConstructor<T>(this Faker<T> faker) where T : class
    {
        return faker.CustomInstantiator(f => Activator.CreateInstance(typeof(T), nonPublic: true) as T);
    }
}