using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Okovalenko_task3
{
    public class Okovalenko_task3 : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            // global settings
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = factory.CreateOrganizationService(context.UserId);
            ITracingService tracer = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            if (context.InputParameters != null)
            {
                // Post Image
                Entity user = context.PostEntityImages["PostImage"];

                // Get Lookup record
                var parent = user.GetAttributeValue<EntityReference>("cr486_parent");

                if (parent != null)
                {
                    // Query to get parent field
                    var parentField = service.Retrieve("cr486_kovalenko_2019", parent.Id, new ColumnSet(true));

                    if (parentField != null)
                    {
                        // get Last Name value
                        var lastName = parentField.GetAttributeValue<string>("cr486_lastname");

                        if (lastName != null)
                        {
                            // avoiding infinite loop
                            if (user.GetAttributeValue<string>("cr486_parentlastname") != lastName)
                            {
                                // setting Last Name value to "Parent last Name field"
                                user["cr486_parentlastname"] = lastName;
                                service.Update(user);
                            }
                            else
                            {
                                return;
                            }
                        }
                    };
                }
            }
        }
    }
}