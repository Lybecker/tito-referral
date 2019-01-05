using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WebAPI.Validation
{
    public static class ValidationExtensions
    {
        public static IEnumerable<string> Validate(this object @this)
        {
            var context = new ValidationContext(@this, serviceProvider: null, items: null);

            var results = new List<ValidationResult>();

            Validator.TryValidateObject(@this, context, results, true);

            foreach (var validationResult in results)
            {
                yield return validationResult.ToString();
            }
        }

        public static void ValidateAndFailIfError(this object @this)
        {
            var errors = @this.Validate().ToArray();

            if (errors.Any())
            {
                var aggrErrors = string.Join(",", errors);
                var count = errors.Length;
                var configType = @this.GetType().Name;

                throw new ApplicationException($"Found {count} configuration error(s) in {configType}: {aggrErrors}");
            }
        }
    }
}