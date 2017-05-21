﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parserproject
{
    public class TypeUnifier
    {

        private Dictionary<ConstructedType, ConstructedType> bindings = new Dictionary<ConstructedType, ConstructedType>();

        private ConstructedType Binding(ConstructedType T)
        {
            if (bindings.ContainsKey(T))
            {
                return Binding(bindings[T]);
            }
            else
            {
                return T;
            }
        }

        private void Bind(ConstructedType TypeA, ConstructedType TypeB)
        {
            if (!bindings.ContainsKey(TypeA))
            {
                bindings.Add(TypeA, TypeB);
            }
            else
            {
                throw new Exception($"{TypeA.ToString()} is already bound");
            }
        }
        
        public TypeSubstitution Unify(ConstructedType TypeA, ConstructedType TypeB)
        {

            if (UnifyAUX(TypeA, TypeB))
            {
                TypeSubstitution unifier = new TypeSubstitution();

                foreach (TypeVar typeVar in bindings.Keys.OfType<TypeVar>())
                {
                    unifier.Add(typeVar, Binding(typeVar));
                }

                bindings.Clear();

                return unifier;
            }
            else
            {
                throw new Exception($"{TypeA.ToString()} and {TypeB.ToString()} could not be unified!");
            }
        }

        private bool UnifyAUX(ConstructedType TypeA, ConstructedType TypeB)
        {
            TypeA = Binding(TypeA);
            TypeB = Binding(TypeB);

            if (TypeA.Equals(TypeB))
            {
                return true;
            }
            else if (TypeA is TypeVar)
            {
                Bind(TypeA, TypeB);
                return true;
            }

            Bind(TypeB, TypeA);

            if (TypeB is TypeVar)
            {
                return true;
            }
            else if(TypeA is FunctionType && TypeB is FunctionType)
            {
                FunctionType A = TypeA as FunctionType;
                FunctionType B = TypeB as FunctionType;

                return UnifyAUX(A.inputType, B.inputType) && UnifyAUX(A.outputType, B.outputType);
            }
            else if (TypeA is TupleType && TypeB is TupleType)
            {
                TupleType A = TypeA as TupleType;
                TupleType B = TypeB as TupleType;

                return UnifyAUX(A.Element1, B.Element1) && UnifyAUX(A.Element2, B.Element2);
            }
            else if (TypeA is ListType && TypeB is ListType)
            {
                ListType A = TypeA as ListType;
                ListType B = TypeB as ListType;

                return UnifyAUX(A.ListElementType, B.ListElementType);
            }
            else
            {
                return false;
            }
        }
    }
}