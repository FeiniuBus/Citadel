using System;
using System.Linq.Expressions;

namespace Citadel.ExpressionJsonSerializer
{
    partial class Serializer
    {
        private bool DynamicExpression(Expression expr)
        {
            var expression = expr as DynamicExpression;
            if (expression == null) { return false; }

            throw new NotImplementedException();
        }
    }
}
