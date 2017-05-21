using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Parserproject;

namespace ParserTest
{
    [TestFixture]
    public class TypeUnifierTest
    {
        [Test]
        public void Unify_SameTypeOfTypeVar_returnsNoSubstitutions()
        {
            TypeUnifier tu = new TypeUnifier();
            TypeVar A = new TypeVar();
            TypeVar B = A.Clone();

            TypeSubstitution sigma = tu.Unify(A, B);

            Assert.AreEqual(0, sigma.table.Count);
        }

        [Test]
        public void Unify_SameTypeOfTalType_returnsNoSubstitutions()
        {
            TypeUnifier tu = new TypeUnifier();
            TalType A = new TalType();
            TalType B = new TalType();

            TypeSubstitution sigma = tu.Unify(A, B);

            Assert.AreEqual(0, sigma.table.Count);
        }

        [Test]
        public void Unify_DifferentTypesOfTypeVarToTalType_returnsOneSubstitutions()
        {
            TypeUnifier tu = new TypeUnifier();
            TypeVar A = new TypeVar();
            TalType B = new TalType();

            TypeSubstitution sigma = tu.Unify(A, B);

            Assert.AreEqual(1, sigma.table.Count);
        }

        [Test]
        public void Unify_DifferentTypesOfTypeVarToTalType_SubstitutesCorrectly()
        {
            TypeUnifier tu = new TypeUnifier();
            TypeVar A = new TypeVar();
            TalType B = new TalType();

            TypeSubstitution sigma = tu.Unify(A, B);

            Assert.AreEqual(B, sigma.Substitute(A));
        }


        [Test]
        public void Unify_DifferentTypesOfGeneralFunctionToSpecificFunction_returnsTwoSubstitutions()
        {
            TypeUnifier tu = new TypeUnifier();
            FunctionType A = new FunctionType(new TypeVar(), new TypeVar());
            FunctionType B = new FunctionType(new TalType(), new BoolType());

            TypeSubstitution sigma = tu.Unify(A, B);

            Assert.AreEqual(2, sigma.table.Count);
        }

        [Test]
        public void Unify_DifferentTypesOfGeneralFunctionToSpecificFunction_SubstitutesCorrectly()
        {
            TypeUnifier tu = new TypeUnifier();
            TypeVar a = new TypeVar();
            TypeVar b = new TypeVar();
            FunctionType A = new FunctionType(a, b);
            FunctionType B = new FunctionType(new TalType(), new BoolType());

            TypeSubstitution sigma = tu.Unify(A, B);

            Assert.AreEqual(B, sigma.Substitute(A));
            Assert.AreEqual(new TalType(), sigma.Substitute(a));
            Assert.AreEqual(new BoolType(), sigma.Substitute(b));
        }
    }
}
