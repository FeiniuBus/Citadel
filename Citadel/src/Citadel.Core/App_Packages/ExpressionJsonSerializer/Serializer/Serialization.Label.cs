﻿using System;
using System.Linq.Expressions;

namespace Citadel.ExpressionJsonSerializer
{
    partial class Serializer
    {
        private bool LabelExpression(Expression expr)
        {
            var expression = expr as LabelExpression;
            if (expression == null) { return false; }

            throw new NotImplementedException();
        }
    }
}
