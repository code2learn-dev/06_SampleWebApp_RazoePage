using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace _06_WebApp_RazoePage.WebApi.Common
{
	public class PrivateSetterContractResolver : DefaultContractResolver
	{
		protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
		{
			var prop = base.CreateProperty(member, memberSerialization);
			if (prop is not null)
			{
				var property = member as PropertyInfo;
				if (property != null)
				{
					bool hasPrivateProp = property.GetSetMethod(true) is not null;
					prop.Writable = hasPrivateProp;
				}
			}

			return prop;
		}

		protected override string ResolvePropertyName(string propertyName)
		{
			return propertyName.ToLower();
		}
	}
}
