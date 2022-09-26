using System.Reflection;

namespace UnicornValley.Domain.Errors;
public class DomainErrorsTests
{
    private static readonly Dictionary<Type, object> _defaultValues = new()
    {
        { typeof(Type), typeof(string) },
        { typeof(string), "text" }
    };

    [Fact]
    public void CreateAllErrors_ShouldWork()
    {
        //find all error category subclasses - Common, Meeting, etc
        var errorCategories = typeof(DomainErrors).GetTypeInfo().DeclaredNestedTypes;

        foreach (var errorCategory in errorCategories)
        {
            //find all error factory method, like Common.NotFoundById
            var errorMethods = errorCategory
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Where(a => a.ReturnType == typeof(DomainError));

            foreach (var errorMethod in errorMethods)
            {
                //get a list of method's parameters
                var parameterInfos = errorMethod.GetParameters();
                var parameters = new object?[parameterInfos.Length];

                for (int i = 0; i < parameterInfos.Length; i++)
                {
                    //set parameter either to a default value (if found), or to default (if value type) or null (if reference type)

                    var parameterType = parameterInfos[i].ParameterType;

                    var defaultValueExists = _defaultValues.TryGetValue(parameterType, out var defaultValue);

                    if (defaultValueExists)
                    {
                        parameters[i] = defaultValue;
                        continue;
                    }

                    parameters[i] = parameterType.IsValueType
                        ? Activator.CreateInstance(parameterType)
                        : null;
                }

                //invoke error creation method - this should throw if any of the errors have an invalid message template
                errorMethod.Invoke(null, parameters);
            }
        }
    }
}
