using System;
using System.Collections.Generic;
using System.Text;

using System.Linq;
using System.Linq.Expressions;


namespace ShoppingBasket.DiscountModel
{
    /// <summary>
    /// The idea for the rulesengine has been taken from the following blog post: https://www.psclistens.com/insight/blog/quickly-build-a-business-rules-engine-using-c-and-lambda-expression-trees/
    /// </summary>
    public class DiscountRule
    {
        /// Author: Cole Francis, Architect
        /// The pre-compiled rules type
        /// 

        ///
        /// A method used to precompile rules for a provided type
        /// 
        public static List<Func<T, bool>> CompileRule<T>(List<T> targetEntity, List<DiscountRule> rules)
        {
            var compiledRules = new List<Func<T, bool>>();
            
            // Loop through the rules and compile them against the properties of the supplied shallow object 
            rules.ForEach(rule =>
            {
                var genericType = Expression.Parameter(typeof(T));
                var key = MemberExpression.Property(genericType, rule.ComparisonPredicate);
                var propertyType = typeof(T).GetProperty(rule.ComparisonPredicate).PropertyType;
                var value = Expression.Constant(Convert.ChangeType(rule.ComparisonValue, propertyType));
                var binaryExpression = Expression.MakeBinary(rule.ComparisonOperator, key, value);

                compiledRules.Add(Expression.Lambda<Func<T,bool>>(binaryExpression, genericType).Compile());
            });
         
            // Return the compiled rules to the caller
            return compiledRules;
        }
        
        ///
        /// Denotes the rules predictate (e.g. Name); comparison operator(e.g. ExpressionType.GreaterThan); value (e.g. "Cole")
        /// 
        public string ComparisonPredicate { get; set; }
        public ExpressionType ComparisonOperator { get; set; }
        public string ComparisonValue { get; set; }

        /// 
        /// The rule method that 
        /// 
        public DiscountRule(string comparisonPredicate, ExpressionType comparisonOperator, string comparisonValue)
        {
            ComparisonPredicate = comparisonPredicate;
            ComparisonOperator = comparisonOperator;
            ComparisonValue = comparisonValue;
        }
    }
}
