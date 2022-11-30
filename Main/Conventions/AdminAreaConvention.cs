using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Authorization;
using System;
using System.Linq;

namespace Main
{
    public class AdminAreaConvention : IControllerModelConvention
    {
        private readonly string area;
        private readonly string policy;

        public AdminAreaConvention(string area, string policy)
        {
            this.area = area;
            this.policy = policy;
        }

        public void Apply(ControllerModel controller)
        {
            if (controller.Attributes.Any(
                a => a is AreaAttribute attr && attr.RouteValue.Equals(area, StringComparison.OrdinalIgnoreCase)) ||
                    controller.RouteValues.Any(x => x.Key.Equals("area", StringComparison.OrdinalIgnoreCase) && x.Value.Equals(area, StringComparison.OrdinalIgnoreCase)))
            {
                controller.Filters.Add(new AuthorizeFilter(policy));
            }
        }
    }
}