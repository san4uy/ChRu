using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace urlApp
{
    internal class EnumBindingSourceExtension : MarkupExtension
    {
        private readonly Type _type;

        public EnumBindingSourceExtension(Type type)
        {
            if (type is null || !type.IsEnum)
                throw new Exception("Enum type must not be null and of type Enum");
            _type = type;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Enum.GetValues(_type)
                .Cast<object>()
                .Select(e => new { Value = (int)e, DisplayName = e.ToString() });
        }

    }
}
