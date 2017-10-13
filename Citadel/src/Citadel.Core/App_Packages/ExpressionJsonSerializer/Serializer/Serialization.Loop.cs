using System;
using System.Linq.Expressions;

namespace Citadel.ExpressionJsonSerializer
{
    partial class Serializer
    {
        private bool LoopExpression(Expression expr)
        {
            var expression = expr as DefaultExpression;
            if (expression == null) { return false; }

            throw new NotImplementedException();
        }
    }
}
