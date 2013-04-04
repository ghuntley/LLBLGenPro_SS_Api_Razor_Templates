﻿using System.Collections.Generic;
using Northwind.Data.Dtos;
using ServiceStack.FluentValidation;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
	// __LLBLGENPRO_USER_CODE_REGION_START SsSvcAdditionalNamespaces 
	// __LLBLGENPRO_USER_CODE_REGION_END 

namespace Northwind.Data.Validators
{ 
    public partial class TerritoryValidator : AbstractValidator<Territory>, IRequiresHttpRequest
	// __LLBLGENPRO_USER_CODE_REGION_START SsSvcAdditionalInterfaces 
	// __LLBLGENPRO_USER_CODE_REGION_END 
    {
        /// <summary>The HTTP request. Lazily loaded, so only available in the validation delegates.</summary>
        public IHttpRequest HttpRequest { get; set; }

        #region Class Extensibility Methods
        partial void OnCreateValidator();
        #endregion

        private IList<string> ParentValidators { get; set; }

        public TerritoryValidator(): this(null)
        {
        }

        internal TerritoryValidator(IList<string> parentValidators)
        {
            ParentValidators = parentValidators ?? new List<string>();
            OnCreateValidator();
            
	// __LLBLGENPRO_USER_CODE_REGION_START SsSvcBeforeRules 
	// __LLBLGENPRO_USER_CODE_REGION_END             

            //Validation rules for GET requests (READ)
            RuleSet("PkRequest", () =>
                {
                    RuleFor(y => y.TerritoryId).NotEmpty().Length(1, 20);
                });

            //Validation rules for POST requests (CREATE)
            RuleSet(ApplyTo.Post, () =>
                {
                    RuleFor(y => y.TerritoryId).NotEmpty().Length(1, 20);
                });
            
            //Validation rules for PUT and DELETE requests (UPDATE / DELETE)
            RuleSet(ApplyTo.Put | ApplyTo.Delete, () =>
                {
                    RuleFor(y => y.TerritoryId).NotEmpty().Length(1, 20);
                });

            //Common Validation rules for POST and PUT requests (CREATE and UPDATE)
            RuleSet(ApplyTo.Post | ApplyTo.Put, () =>
                {
                    RuleFor(y => y.TerritoryDescription).NotEmpty().Length(1, 50);
                    RuleFor(y => y.RegionId).GreaterThanOrEqualTo(0);

                    //Setup validators on relations (to avoid recursion issues, we will not process any validator types that have already been run)
                    //TODO: To avoid recursion issues, the unfortunate consequence at this time is that some objects may not get validated if they
                    //      have the same validator of a parent object in the graph. We will need to fix this at some point by tracking
                    //      previously validated objects for each type of validator (TBD).
                    if(!ParentValidators.Contains("EmployeeTerritoryValidator")) 
                      RuleFor(x => x.EmployeeTerritories).SetCollectionValidator(new EmployeeTerritoryValidator(new List<string>( ParentValidators ) { { "TerritoryValidator" } })).When(x => x.EmployeeTerritories != null);
                    if(!ParentValidators.Contains("RegionValidator")) 
                      RuleFor(x => x.Region).SetValidator(new RegionValidator(new List<string>( ParentValidators ) { { "TerritoryValidator" } })).When(x => x.Region != null);
                });

	// __LLBLGENPRO_USER_CODE_REGION_START SsSvcAfterRules 
	// __LLBLGENPRO_USER_CODE_REGION_END 

        }
        
	// __LLBLGENPRO_USER_CODE_REGION_START SsSvcAdditionalMethods 
	// __LLBLGENPRO_USER_CODE_REGION_END                           

    }
}