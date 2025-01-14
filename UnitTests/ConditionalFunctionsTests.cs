﻿using NUnit.Framework;
using System;

namespace JUST.UnitTests
{
    [TestFixture, Category("ConditionalFunctions")]
    public class ConditionalFunctionsTests
    {
        [Test, Category("IfCondition")]
        public void PrimitiveFirstTrueConditionStringResult()
        {
            const string input = "{ \"string\": \"some words\", \"integer\": 123, \"boolean\": true }";
            const string transformer = "{ \"result\": \"#ifcondition(true,#valueof($.boolean),truevalue,falsevalue)\" }";

            var result = JsonTransformer.Transform(transformer, input);

            Assert.AreEqual("{\"result\":\"truevalue\"}", result);
        }

        [Test, Category("IfCondition")]
        public void PrimitiveFirstFalseConditionStringResult()
        {
            const string input = "{ \"string\": \"some words\", \"integer\": 123, \"boolean\": true }";
            const string transformer = "{ \"result\": \"#ifcondition(dummy,#valueof($.string),truevalue,falsevalue)\" }";

            var result = JsonTransformer.Transform(transformer, input);

            Assert.AreEqual("{\"result\":\"falsevalue\"}", result);
        }

        [Test, Category("IfCondition")]
        public void FnFirstTrueConditionStringResult()
        {
            const string input = "{ \"string\": \"some words\", \"integer\": 123, \"boolean\": true }";
            const string transformer = "{ \"result\": \"#ifcondition(#valueof($.boolean),true,truevalue,falsevalue)\" }";

            var result = JsonTransformer.Transform(transformer, input);

            Assert.AreEqual("{\"result\":\"truevalue\"}", result);
        }

        [Test, Category("IfCondition")]
        public void FnFirstFalseConditionStringResult()
        {
            const string input = "{ \"string\": \"some words\", \"integer\": 123, \"boolean\": true }";
            const string transformer = "{ \"result\": \"#ifcondition(#valueof($.integer),555,truevalue,falsevalue)\" }";

            var result = JsonTransformer.Transform(transformer, input);

            Assert.AreEqual("{\"result\":\"falsevalue\"}", result);
        }

        [Test, Category("IfCondition")]
        public void FnBothTrueConditionStringResult()
        {
            const string input = "{ \"string\": \"some words\", \"integer\": 123, \"boolean\": true, \"same_integer\": 123 }";
            const string transformer = "{ \"result\": \"#ifcondition(#valueof($.integer),#valueof($.same_integer),truevalue,falsevalue)\" }";

            var result = JsonTransformer.Transform(transformer, input);

            Assert.AreEqual("{\"result\":\"truevalue\"}", result);
        }

        [Test, Category("IfCondition")]
        public void FnBothFalseConditionStringResult()
        {
            const string input = "{ \"string\": \"some words\", \"integer\": 123, \"boolean\": true, \"same_integer\": 123 }";
            const string transformer = "{ \"result\": \"#ifcondition(#valueof($.string),#valueof($.same_integer),truevalue,falsevalue)\" }";

            var result = JsonTransformer.Transform(transformer, input);

            Assert.AreEqual("{\"result\":\"falsevalue\"}", result);
        }

        [Test, Category("IfCondition")]
        public void TrueFnResult()
        {
            const string input = "{ \"string\": \"some words\", \"integer\": 123, \"boolean\": true, \"same_integer\": 123 }";
            const string transformer = "{ \"result\": \"#ifcondition(#valueof($.integer),#valueof($.same_integer),#valueof($.boolean),falsevalue)\" }";

            var result = JsonTransformer.Transform(transformer, input);

            Assert.AreEqual("{\"result\":true}", result);
        }

        [Test, Category("IfCondition")]
        public void FalseFnResult()
        {
            const string input = "{ \"string\": \"some words\", \"integer\": 123, \"boolean\": true, \"other_integer\": 1235 }";
            const string transformer = "{ \"result\": \"#ifcondition(#valueof($.integer),#valueof($.other_integer),truevalue,#valueof($.other_integer))\" }";

            var result = JsonTransformer.Transform(transformer, input);

            Assert.AreEqual("{\"result\":1235}", result);
        }

        [Test, Category("IfCondition")]
        public void LazyEvaluationTrueCondition()
        {
            const string input = "{ \"string\": \"some words\", \"integer\": 123, \"boolean\": true, \"other_integer\": 1235 }";
            const string transformer = "{ \"result\": \"#ifcondition(#valueof($.boolean),true,#valueof($.other_integer),#valueof(invalid.jsonPath.$))\" }";

            var result = JsonTransformer.Transform(transformer, input);

            Assert.AreEqual("{\"result\":1235}", result);
        }

        [Test, Category("IfCondition")]
        public void LazyEvaluationFalseCondition()
        {
            const string input = "{ \"string\": \"some words\", \"integer\": 123, \"boolean\": true, \"other_integer\": 1235 }";
            const string transformer = "{ \"result\": \"#ifcondition(#valueof($.boolean),false,#valueof(invalid.jsonPath.$),#valueof($.other_integer))\" }";

            var result = JsonTransformer.Transform(transformer, input);

            Assert.AreEqual("{\"result\":1235}", result);
        }

        [Test, Category("IfGroup")]
        public void ConditionalGroupTrueTest()
        {
            const string input = "{ \"Tree\": { \"Branch\": \"leaf\", \"Flower\": \"Rose\" } }";
            const string transformer = "{ \"Result\": { \"Header\": \"JsonTransform\", \"#ifgroup(#exists($.Tree.Branch))\": { \"State\": { \"Value1\": \"#valueof($.Tree.Branch)\", \"Value2\": \"#valueof($.Tree.Flower)\" } } } }";

            var result = JsonTransformer.Transform(transformer, input);

            Assert.AreEqual("{\"Result\":{\"Header\":\"JsonTransform\",\"State\":{\"Value1\":\"leaf\",\"Value2\":\"Rose\"}}}", result);
        }

        [Test, Category("IfGroup")]
        public void ConditionalGroupFalseTest()
        {
            const string input = "{ \"Tree\": { \"Branch\": \"leaf\", \"Flower\": \"Rose\" } }";
            const string transformer = "{ \"Result\": { \"Header\": \"JsonTransform\", \"#ifgroup(#exists($.Tree.Root))\": { \"State\": { \"Value1\": \"#valueof($.Tree.Branch)\", \"Value2\": \"#valueof($.Tree.Flower)\" } } } }";

            var result = JsonTransformer.Transform(transformer, input);

            Assert.AreEqual("{\"Result\":{\"Header\":\"JsonTransform\"}}", result);
        }

        [Test, Category("IfGroup"), Category("Strict")]
        public void ConditionalGroupException()
        {
            const string input = "{ \"Tree\": { \"Branch\": \"leaf\", \"Flower\": \"Rose\" } }";
            const string transformer = "{ \"Result\": { \"Header\": \"JsonTransform\", \"#ifgroup(wrong_val)\": { \"State\": { \"Value1\": \"#valueof($.Tree.Branch)\", \"Value2\": \"#valueof($.Tree.Flower)\" }} } }";

            Assert.Throws<FormatException>(() => JsonTransformer.Transform(transformer, input, new JUSTContext { EvaluationMode = EvaluationMode.Strict }));
        }

        [Test, Category("IfGroup"), Category("FallbackToDefault")]
        public void ConditionalGroupExceptionFallbackToDefault()
        {
            const string input = "{ \"Tree\": { \"Branch\": \"leaf\", \"Flower\": \"Rose\" } }";
            const string transformer = "{ \"Result\": { \"Header\": \"JsonTransform\", \"#ifgroup(wrong_val)\": { \"State\": { \"Value1\": \"#valueof($.Tree.Branch)\", \"Value2\": \"#valueof($.Tree.Flower)\" }} } }";

            var result = JsonTransformer.Transform(transformer, input);

            Assert.AreEqual("{\"Result\":{\"Header\":\"JsonTransform\"}}", result);
        }
    }
}
