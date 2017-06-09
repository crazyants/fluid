﻿using System;
using System.IO;
using System.Text.Encodings.Web;

namespace Fluid.Ast.Values
{
    public abstract class FluidValue : IEquatable<FluidValue>
    {
        public abstract void WriteTo(TextWriter writer, TextEncoder encoder);

        public abstract bool Equals(FluidValue other);

        public abstract bool ToBooleanValue();
        public abstract double ToNumberValue();
        public abstract string ToStringValue();
        public abstract object ToObjectValue();

        public virtual bool IsUndefined()
        {
            return false;
        }

        public virtual bool IsNil()
        {
            return false;
        }

        public static FluidValue Create(object value)
        {
            switch (Type.GetTypeCode(value.GetType()))
            {
                case TypeCode.Boolean:
                    return new BooleanValue(Convert.ToBoolean(value));
                case TypeCode.Byte:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.SByte:
                case TypeCode.Single:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return new NumberValue(Convert.ToDouble(value));
                case TypeCode.Empty:
                    return NilValue.Instance;
                case TypeCode.Object:

                    if (value == null)
                    {
                        return NilValue.Instance;
                    }

                    if (value is FluidValue fluid)
                    {
                        return fluid;
                    }

                    return new ObjectValue(value);
                case TypeCode.DateTime:
                case TypeCode.Char:
                case TypeCode.String:
                    return new StringValue(Convert.ToString(value));
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}