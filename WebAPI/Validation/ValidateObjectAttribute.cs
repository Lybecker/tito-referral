using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace WebAPI.Validation
{
    /// <summary>
    /// Validates object via the System.ComponentModel.DataAnnotations validation.
    /// DataAnnotations does not recursively validate object properties like object graphs.
    /// </summary>
    /// <example>
    /// public class Person {
    ///   [Required]
    ///   public String Name { get; set; }
    /// 
    ///   [Required, ValidateObject]
    ///   public Address Address { get; set; }
    /// }
    /// </example>
    /// <remarks>Idea from http://www.technofattie.com/2011/10/05/recursive-validation-using-dataannotations.html </remarks>
    public class ValidateObjectAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(value, null, null);

            Validator.TryValidateObject(value, context, results, true);

            if (results.Count != 0)
            {
                var compositeResults = new CompositeValidationResult($"Validation for object property '{validationContext.DisplayName}' failed.");
                results.ForEach(compositeResults.AddResult);

                return compositeResults;
            }

            return ValidationResult.Success;
        }
    }

    public class CompositeValidationResult : ValidationResult
    {
        private readonly List<ValidationResult> _results = new List<ValidationResult>();

        public IEnumerable<ValidationResult> Results
        {
            get
            {
                return _results;
            }
        }

        public CompositeValidationResult(string errorMessage) : base(errorMessage) { }
        public CompositeValidationResult(string errorMessage, IEnumerable<string> memberNames) : base(errorMessage, memberNames) { }
        protected CompositeValidationResult(ValidationResult validationResult) : base(validationResult) { }

        public void AddResult(ValidationResult validationResult)
        {
            _results.Add(validationResult);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine(base.ToString());
            foreach (var result in _results)
            {
                sb.Append("\t").Append(result.ErrorMessage);
            }

            return sb.ToString();
        }
    }
}