﻿using System;
using AngularClientGenerator.Visitor;

namespace AngularClientGenerator.DescriptionParts
{
    public class TypeDescriptionPart : IDescriptionPart
    {
        public Type Type { get; }

        public TypeDescriptionPart(Type type)
        {
            this.Type = type;
        }

        public void Accept(IApiVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
